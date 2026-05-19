using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IDashboardService
    {
        // Legacy
        Task<DashboardStatsDto> GetSummaryAsync();

        // Role-based rich dashboards
        Task<AdminDashboardDto>     GetAdminDashboardAsync();
        Task<PassengerDashboardDto> GetPassengerDashboardAsync(Guid userId);
        Task<MerchantDashboardDto>  GetMerchantDashboardAsync(Guid userId);
    }
}
