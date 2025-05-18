using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LLQE.Common.Interfaces
{
    public abstract class ConsumerDaemon : BackgroundService
    {
        public readonly ILogger<ConsumerDaemon> _logger;
        public readonly IRequestAI _requestAI;
        public readonly IProducer _producer;
        public readonly string _model;

        private readonly string _receiveTopic;
        protected readonly string _callbackTopic;
        private readonly string _nodeName;

        private readonly ConsumerConfig _consumerConfig;

        public ConsumerDaemon(IConfiguration configuration, ILogger<ConsumerDaemon> logger)
        {
            _receiveTopic = configuration["Kafka:ReceiveTopic"];

            _logger = logger;

            _consumerConfig = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
        }

        public ConsumerDaemon(IConfiguration configuration, ILogger<ConsumerDaemon> logger, IProducer producer, IRequestAI requestAI)
        {
            _receiveTopic = configuration["Kafka:ReceiveTopic"];
            _callbackTopic = configuration["Kafka:CallbackTopic"];
            _nodeName = configuration["ApiSettings:NodeName"];
            _model = configuration["ApiSettings:Model"];

            _logger = logger;
            _producer = producer;
            _requestAI = requestAI;

            _consumerConfig = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Kafka {_nodeName} consumer сервис стартовал.");

            await Task.Run(() =>
            {
                using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
                {
                    consumer.Subscribe(_receiveTopic);

                    try
                    {
                        while (!stoppingToken.IsCancellationRequested)
                        {
                            try
                            {
                                var consumeResult = consumer.Consume(stoppingToken);
                                _logger.LogInformation($"Получено сообщение на топике {consumeResult.Topic}, offset {consumeResult.Offset}");

                                HandleMessage(consumeResult.Message.Value, stoppingToken);

                                consumer.Commit(consumeResult);
                            }
                            catch (ConsumeException ex)
                            {
                                _logger.LogError($"Ошибка при получении сообщения: {ex.Error.Reason}");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        consumer.Close();
                        _logger.LogInformation($"Kafka {_nodeName} consumer сервис закрыт.");
                    }
                }
            }, stoppingToken);
        }

        public virtual async Task HandleMessage(string message, CancellationToken cancellationToken)
        {
            var words = message.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var shortMessage = string.Join(' ', words.Take(8));

            if (words.Length > 8)
                shortMessage += "...";

            _logger.LogInformation($"Обработка сообщения: {shortMessage}");

            var response = await _requestAI.SendRequestAsync(_model, message, cancellationToken);

            _producer.ProduceAsync(_callbackTopic, response);
        }
    }
}
