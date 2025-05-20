using Confluent.Kafka;
using LLQE.Common.Entities;
using LLQE.Common.Extensions;
using LLQE.Common.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class MultiTopicConsumer : BackgroundService
{
    private readonly ILogger<MultiTopicConsumer> _logger;
    private readonly KafkaSettings _kafkaSettings;
    private readonly ConsumerConfig _config;
    private readonly NodeMessagesStore _store;

    public MultiTopicConsumer(
        IOptions<KafkaSettings> kafkaOptions,
        ILogger<MultiTopicConsumer> logger,
        NodeMessagesStore store)
    {
        _kafkaSettings = kafkaOptions.Value;
        _logger = logger;
        _store = store;

        _config = new ConsumerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers,
            GroupId = "llqe-webapp-multitopic-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            using var consumer = new ConsumerBuilder<Ignore, string>(_config).Build();
            consumer.Subscribe(_kafkaSettings.ResponseTopics);

            _logger.LogInformation($"Подписка на топики: {string.Join(", ", _kafkaSettings.ResponseTopics)}");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = consumer.Consume(stoppingToken);
                        var topic = cr.Topic;
                        var message = cr.Message.Value;

                        if (_kafkaSettings.NodeResponseTopics.TryGetValue(topic, out var nodeName))
                        {
                            _logger.LogInformation($"Ответ от узла {nodeName}: {message.Truncate()}");
                            HandleNodeResponse(nodeName, message);
                        }
                        else
                        {
                            _logger.LogWarning($"Неизвестный топик: {topic}");
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Ошибка Kafka: {ex.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                consumer.Close();
                _logger.LogInformation("Kafka consumer остановлен.");
            }
        }, stoppingToken);
    }

    public void HandleNodeResponse(string nodeName, string message)
    {
        _logger.LogInformation($"Обработка ответа от {nodeName}: {message.Truncate()}");
        _store.SetMessage(nodeName, message);
    }
}
