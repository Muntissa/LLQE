using LLQE.Common.Interfaces;

namespace LLQE.ChatgptNode.Daemons
{
    public class ChathptTopicConsumer : ConsumerDaemon
    {
        public ChathptTopicConsumer(IConfiguration configuration, ILogger<ConsumerDaemon> logger, IProducer producer, IRequestAI requestAI) : base(configuration, logger, producer, requestAI)
        {
        }
    }
}
