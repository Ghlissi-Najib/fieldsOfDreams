using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class CampaignLifecycleBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CampaignLifecycleBackgroundService> _logger;
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);

        public CampaignLifecycleBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<CampaignLifecycleBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Campaign lifecycle service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await TickAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in campaign lifecycle tick.");
                }

                await Task.Delay(Interval, stoppingToken);
            }
        }

        private async Task TickAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var now = DateTime.UtcNow;

            // Scheduled → Active when StartDate has arrived
            var toActivate = await context.Campaigns
                .Where(c => c.Status == CampaignStatus.Scheduled && c.StartDate <= now)
                .ToListAsync();

            foreach (var c in toActivate)
            {
                c.Status = CampaignStatus.Active;
                c.UpdatedAt = now;
                _logger.LogInformation("Campaign '{Name}' ({Id}) activated.", c.Name, c.Id);
            }

            // Active → Completed when EndDate has passed
            var toComplete = await context.Campaigns
                .Where(c => c.Status == CampaignStatus.Active && c.EndDate < now)
                .ToListAsync();

            foreach (var c in toComplete)
            {
                c.Status = CampaignStatus.Completed;
                c.UpdatedAt = now;
                _logger.LogInformation("Campaign '{Name}' ({Id}) completed.", c.Name, c.Id);
            }

            if (toActivate.Count > 0 || toComplete.Count > 0)
                await context.SaveChangesAsync();
        }
    }
}
