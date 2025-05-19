namespace LLQE.Common.Interfaces
{
    public interface ITopicInitializer
    {
        public string CallbackTopic { get; set; }
        public string ReceiveTopic { get; set; }

        Task EnsureTopicExistsAsync(string topicName, int numPartitions = 1, short replicationFactor = 1, CancellationToken cancellationToken = default);
    }
}
