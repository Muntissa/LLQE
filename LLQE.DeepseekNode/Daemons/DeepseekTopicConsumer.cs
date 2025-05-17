using LLQE.Common.Interfaces;
using System.Threading;

namespace LLQE.DeepseekNode.Daemons
{
    public class DeepseekTopicConsumer : ConsumerDaemon
    {
        public DeepseekTopicConsumer(IConfiguration confguration, ILogger<DeepseekTopicConsumer> logger, IRequestAI requestAI) : base(confguration, logger, requestAI) { }

        public override async Task HandleMessage(string message, CancellationToken token)
        {
            _logger.LogInformation($"Обработка сообщения: {message}");

            var response = await _requestAI.SendRequestAsync(_model, message, token);

            _logger.LogInformation($"AI Response: {response}");

        }
    }
}
