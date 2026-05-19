using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Domain.Entities;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface ICampaignOrchestrator
    {
        Task<IEnumerable<Campaign>> GetActiveCampaignsAsync();
        Task<bool> IsRewardAvailableAsync(Guid rewardId, Guid userId);
        Task<bool> IsQRCodeValidAsync(Guid qrCodeId, Guid userId);
        Task<OrchestrationStateDto> GetOrchestrationStateAsync(Guid userId, string role, int userPoints);
    }
}
