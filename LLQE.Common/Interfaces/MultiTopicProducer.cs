using Confluent.Kafka;
using LLQE.Common.Entities;
using Microsoft.Extensions.Options;

namespace LLQE.Common.Interfaces
{
public class MultiTopicProducer
{
    private readonly KafkaSettings _kafkaSettings;
    private readonly ProducerConfig _producerConfig;

    public MultiTopicProducer(IOptions<KafkaSettings> kafkaOptions)
    {
        _kafkaSettings = kafkaOptions.Value;
        _producerConfig = new ProducerConfig
        {
            BootstrapServers = _kafkaSettings.BootstrapServers
        };
    }

    public async Task ProduceToAllRequestTopicsAsync(string message, CancellationToken cancellationToken = default)
    {
        using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
        var tasks = _kafkaSettings.RequestTopics.Select(topic =>
            producer.ProduceAsync(topic, new Message<Null, string> { Value = message }, cancellationToken)
        );

        await Task.WhenAll(tasks);
    }
}
}
