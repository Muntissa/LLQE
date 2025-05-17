using Confluent.Kafka;

namespace LLQE.Common.Services
{
    public class ConsumerLLQE
    {
        string _topicName;
        public ConsumerLLQE(string topicName)
        {
            _topicName = topicName;
        }

        public void ConsumingMessages()
        {
            var conf = new ConsumerConfig
            {
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(_topicName);

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Полученное сообщение: '{cr.Message.Value}'");

                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Ошибка: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
        }
    }
}
