using LLQE.Common.Interfaces;

namespace LLQE.DeepseekNode.Daemons
{
    public class DeepseekTopicConsumer : DaemonConsumer
    {
        public DeepseekTopicConsumer(string topicName, string nodeName, ILogger<DeepseekTopicConsumer> logger) : base(topicName, nodeName, logger) { }

        public override void HandleMessage(string message)
        {
            _logger.LogInformation($"Обработка сообщения: {message}");
            // TODO: Добавить здесь свою логику обработки сообщения
        }
    }
}
