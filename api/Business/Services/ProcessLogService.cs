using StargateAPI.Business.Data;
using StargateAPI.Business.IServices;
using Microsoft.EntityFrameworkCore;

namespace StargateAPI.Business.Services
{
    public class ProcessLogService : IProcessLogService
    {
        private readonly StargateContext _context;

        public ProcessLogService(StargateContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string action, string status, string message, CancellationToken cancellationToken = default)
        {
            await _context.ProcessLogs.AddAsync(new ProcessLog
            {
                Action = action,
                Status = status,
                Message = message,
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}