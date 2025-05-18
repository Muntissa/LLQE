using LLQE.Common.Interfaces;

namespace LLQE.WebApp.Daemons
{
    public class DeepseekTopicCallback : ConsumerDaemon
    {
        public DeepseekTopicCallback(IConfiguration configuration, ILogger<ConsumerDaemon> logger) : base(configuration, logger)
        {

        }

        public override async Task HandleMessage(string message, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Обработка сообщения: {message}");
        }
    }
}
