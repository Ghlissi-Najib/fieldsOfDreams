using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface ISponsorService
    {
        Task<IEnumerable<SponsorDto>> GetAllAsync();
        Task<SponsorDto?> GetByIdAsync(Guid id);
        Task<SponsorDto> CreateAsync(CreateSponsorRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateSponsorRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
