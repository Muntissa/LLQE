using Confluent.Kafka;
using Confluent.Kafka.Admin;
using LLQE.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LLQE.Common.Services
{
    public class TopicInitializer : ITopicInitializer
    {
        private readonly string _bootstrapServers;
        private readonly ILogger<TopicInitializer> _logger;

        public string CallbackTopic { get; set; }
        public string ReceiveTopic { get; set; }

        public TopicInitializer(IConfiguration configuration, ILogger<TopicInitializer> logger)
        {
            _bootstrapServers = configuration["Kafka:BootstrapServers"];
            ReceiveTopic = configuration["Kafka:ReceiveTopic"];
            CallbackTopic = configuration["Kafka:CallbackTopic"];
            _logger = logger;
        }

        public async Task EnsureTopicExistsAsync(string topicName, int numPartitions = 1, short replicationFactor = 1, CancellationToken cancellationToken = default)
        {
            var adminConfig = new AdminClientConfig { BootstrapServers = _bootstrapServers };

            using (var adminClient = new AdminClientBuilder(adminConfig).Build())
            {
                try
                {
                    var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
                    bool topicExist = metadata.Topics.Any(t => t.Topic == topicName);

                    if (!topicExist)
                    {
                        _logger.LogInformation($"Топик '{topicName}' не найден. Создание...");

                        var topicSpec = new TopicSpecification
                        {
                            Name = topicName,
                            NumPartitions = numPartitions,
                            ReplicationFactor = replicationFactor
                        };

                        await adminClient.CreateTopicsAsync(new[] { topicSpec });
                        _logger.LogInformation($"Топик '{topicName}' успешно создан.");
                    }
                    else
                    {
                        _logger.LogInformation($"Топик '{topicName}' уже существует.");
                    }
                }
                catch (CreateTopicsException e)
                {
                    if(e.Results.Any(r => r.Error.Code == ErrorCode.TopicAlreadyExists))
                    {
                        _logger.LogWarning($"Топик '{topicName}' уже существует (CreateTipicsException).");
                    }
                    else
                    {
                        _logger.LogError(e, $"Ошибка при создании топика '{topicName}'.");
                        throw;
                    }
                }
            }
        }
    }
}
