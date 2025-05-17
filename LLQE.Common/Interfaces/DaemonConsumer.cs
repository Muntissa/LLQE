using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LLQE.Common.Interfaces
{
    public abstract class DaemonConsumer : BackgroundService
    {
        private readonly string _topicName;
        private readonly string _nodeName;
        private readonly ConsumerConfig _consumerConfig;
        public readonly ILogger<DaemonConsumer> _logger;
        public readonly IRequestAI _requestAI;

        public DaemonConsumer(string topicName, string nodeName, ILogger<DaemonConsumer> logger, IRequestAI requestAI)
        {
            _topicName = topicName;
            _logger = logger;
            _nodeName = nodeName;
            _requestAI = requestAI;

            _consumerConfig = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
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
                        _logger.LogInformation($"Kafka {_nodeName} consumer закрыт.");
                    }
                }
            }, stoppingToken);
        }

        public virtual async Task HandleMessage(string message, CancellationToken cancellationToken) =>  _logger.LogInformation($"Обработка сообщения: {message}");
        
    }
}
