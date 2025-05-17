using Confluent.Kafka;
using LLQE.Common.Services;



var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

// If serializers are not specified, default serializers from
// `Confluent.Kafka.Serializers` will be automatically used where
// available. Note: by default strings are encoded as UTF8.
using (var p = new ProducerBuilder<Null, string>(config).Build())
{
    try
    {
        var dr = await p.ProduceAsync("DeepseekPrompts", new Message<Null, string> { Value = "Привет дипсик" });
        Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
    }
    catch (ProduceException<Null, string> e)
    {
        Console.WriteLine($"Delivery failed: {e.Error.Reason}");
    }
}
/*var conf = new ConsumerConfig
{
    GroupId = "test-consumer-group",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Earliest
};

using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
{
    c.Subscribe("TestTopic");

    CancellationTokenSource cts = new CancellationTokenSource();
    Console.CancelKeyPress += (_, e) =>
    {
        e.Cancel = true;
        cts.Cancel();
    };

    try
    {
        while(true)
        {
            try
            {
                var cr = c.Consume(cts.Token);
                Console.WriteLine($"Полученное сообщение: '{cr.Value}' из {cr.TopicPartitionOffset}");
            }
            catch(ConsumeException e)
            {
                Console.WriteLine($"Ошибка: {e.Error.Reason}");
            }
        }
    }
    catch(OperationCanceledException)
    {
        c.Close();
    }
}*/