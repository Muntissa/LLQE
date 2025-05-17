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
        public readonly string _model;

        private readonly string _topicName;
        private readonly string _nodeName;
        private readonly ConsumerConfig _consumerConfig;

        public ConsumerDaemon(IConfiguration configuration, ILogger<ConsumerDaemon> logger, IRequestAI requestAI)
        {
            _topicName = configuration["Kafka:ReceiveTopic"];
            _nodeName = configuration["Kafka:CallbackTopic"];
            _model = configuration["ApiSettings:Model"];
            _logger = logger;
            _requestAI = requestAI;

            _consumerConfig = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };
            _requestAI = requestAI;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Kafka {_nodeName} consumer сервис стартовал.");

            await Task.Run(() =>
            {
                using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
                {
                    consumer.Subscribe(_topicName);

                    try
                    {
                        while (!stoppingToken.IsCancellationRequested)
                        {
                            try
                            {
                                var consumeResult = consumer.Consume(stoppingToken);
                                _logger.LogInformation($"Получено сообщение: '{consumeResult.Message.Value}' на топике {consumeResult.Topic}, partition {consumeResult.Partition}, offset {consumeResult.Offset}");

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

        public virtual async Task HandleMessage(string message, CancellationToken cancellationToken) =>  _logger.LogInformation($"Обработка сообщения: {message}");
        
    }
}
