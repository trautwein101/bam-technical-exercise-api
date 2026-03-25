namespace StargateAPI.Business.IServices
{
    public interface IProcessLogService
    {
        Task LogAsync(string action, string status, string message, CancellationToken cancellationToken = default);
    }
}