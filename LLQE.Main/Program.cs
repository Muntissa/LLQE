using Confluent.Kafka;
using LLQE.Common.Services;

var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

using (var p = new ProducerBuilder<Null, string>(config).Build())
{
    try
    {
        var dr = await p.ProduceAsync("DeepseekPrompts", new Message<Null, string> { Value = Console.ReadLine() });
        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
    }
    catch (ProduceException<Null, string> e)
    {
        Console.WriteLine($"Delivery failed: {e.Error.Reason}");
    }
}
