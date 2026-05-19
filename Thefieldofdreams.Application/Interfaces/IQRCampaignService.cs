using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface IQRCampaignService
    {
        Task<IEnumerable<QRCampaignDto>> GetAllAsync();
        Task<IEnumerable<QRCampaignDto>> GetByCampaignIdAsync(Guid campaignId);
        Task<QRCampaignDto?> GetByIdAsync(Guid id);
        Task<QRCampaignDto> CreateAsync(CreateQRCampaignRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateQRCampaignRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
