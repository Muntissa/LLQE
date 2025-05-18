using Confluent.Kafka;
using LLQE.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LLQE.Common.Services
{
    public class ProducerService : IProducer, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ILogger<ProducerService> _logger;
        public ProducerService(IConfiguration configuration, ILogger<ProducerService> logger)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _logger = logger;
        }
        public async Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default)
        {
            try
            {
                var deliveryResult = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message }, cancellationToken);
                _logger.LogInformation($"Сообщение доставлено в '{deliveryResult.Topic} с порядком @{deliveryResult.Offset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError($"Отправка не удалась: {e.Error.Reason}");
                throw;
            }
        }
        public void Dispose()
        {
            _producer?.Dispose();
        }

    }
}
