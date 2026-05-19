using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationDto>> GetAllAsync();
        Task<LocationDto?> GetByIdAsync(Guid id);
        Task<LocationDto> CreateAsync(CreateLocationRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateLocationRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
