using LLQE.Common.Interfaces;
using System.Threading;

namespace LLQE.DeepseekNode.Daemons
{
    public class DeepseekTopicConsumer : DaemonConsumer
    {
        public DeepseekTopicConsumer(string topicName, string nodeName, ILogger<DeepseekTopicConsumer> logger, IRequestAI requestAI) : base(topicName, nodeName, logger, requestAI) { }

        public override async Task HandleMessage(string message, CancellationToken token)
        {
            var model = "deepseek-chat";
            _logger.LogInformation($"Обработка сообщения: {message}");
            var response = await _requestAI.SendRequestAsync(model, message, token);
            _logger.LogInformation($"AI Response: {response}");

        }
    }
}
