using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface ICampaignService
    {
        Task<IEnumerable<CampaignDto>> GetAllAsync();
        Task<IEnumerable<CampaignDto>> GetBySponsorIdAsync(Guid sponsorId);
        Task<CampaignDto?> GetByIdAsync(Guid id);
        Task<CampaignDto> CreateAsync(CreateCampaignRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateCampaignRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
