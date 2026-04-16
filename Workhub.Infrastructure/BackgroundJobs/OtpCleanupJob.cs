using Workhub.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Workhub.Infrastructure.BackgroundJobs;

public class OtpCleanupJob
{
    private readonly AppDataContext _context;
    private readonly ILogger<OtpCleanupJob> _logger;

    public OtpCleanupJob(AppDataContext context, ILogger<OtpCleanupJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task CleanupExpiredOtps()
    {
        try
        {
            var expiredOtps = await _context.Otps
                .Where(o => o.ExpiryTime < DateTime.UtcNow)
                .ToListAsync();

            if (expiredOtps.Any())
            {
                _context.Otps.RemoveRange(expiredOtps);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Removed {Count} expired OTPs", expiredOtps.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired OTPs");
        }
    }
}
