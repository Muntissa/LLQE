namespace LLQE.Common.Interfaces
{
    public interface IRequestAI
    {
        Task<string> SendRequestAsync(string model, string message, CancellationToken cancellationToken = default);
    }
}
