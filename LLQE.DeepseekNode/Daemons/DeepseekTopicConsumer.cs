using LLQE.Common.Interfaces;

namespace LLQE.DeepseekNode.Daemons
{
    public class DeepseekTopicConsumer : ConsumerDaemon
    {
        public DeepseekTopicConsumer(IConfiguration confguration, ILogger<DeepseekTopicConsumer> logger, IProducer producer, IRequestAI requestAI) : 
            base(confguration, logger, producer, requestAI) { }
    }
}
