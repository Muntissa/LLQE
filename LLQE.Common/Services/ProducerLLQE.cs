using Confluent.Kafka;
using LLQE.Common.Interfaces;

namespace LLQE.Common.Services
{
    public class ProducerLLQE : IProducer
    {
        public async Task ProduceMessage(string topicName, string prompt)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(topicName,
                        new Message<Null, string> { Value = prompt });

                    Console.WriteLine($"Prompt отправлен в {dr.Topic}");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Отправка не удалась: {e.Error.Reason}");
                }
            }
        }
    }
}
