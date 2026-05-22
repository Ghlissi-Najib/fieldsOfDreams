using Thefieldofdreams.Application.Interfaces;

namespace Thefieldofdreams.API.Services
{
    public class GpsPollingBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<GpsPollingBackgroundService> _logger;

        private static readonly TimeSpan PollingInterval = TimeSpan.FromHours(4);
        private static readonly TimeSpan StaleThreshold = TimeSpan.FromHours(4);

        public GpsPollingBackgroundService(
            IServiceScopeFactory scopeFactory,
            ILogger<GpsPollingBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckStaleTechnicianLocationsAsync(stoppingToken);

                try
                {
                    await Task.Delay(PollingInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async Task CheckStaleTechnicianLocationsAsync(CancellationToken cancellationToken)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var technicianService = scope.ServiceProvider.GetRequiredService<IUserService>();

                var technicians = await technicianService.GetAllAsync();
                var staleThresholdTime = DateTime.UtcNow - StaleThreshold;

                var staleTechnicians = technicians
                    .Where(t => t.LastLocationUpdate is null || t.LastLocationUpdate < staleThresholdTime)
                    .ToList();

                if (staleTechnicians.Count > 0)
                {
                    _logger.LogInformation(
                        "GPS polling: {Count} technician(s) have not reported their location in the last 4 hours.",
                        staleTechnicians.Count);
                }
                else
                {
                    _logger.LogInformation("GPS polling: All technicians have up-to-date locations.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GPS polling check.");
            }
        }
    }

}
