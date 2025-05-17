namespace LLQE.Common.Interfaces
{
    public interface IProducer
    {
        Task ProduceAsync(string topic, string message, CancellationToken cancellationToken = default);
    }
}
