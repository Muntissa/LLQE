namespace LLQE.Common.Interfaces
{
    public interface IProducer
    {
        public Task ProduceMessage(string topicName, string prompt);
    }
}
