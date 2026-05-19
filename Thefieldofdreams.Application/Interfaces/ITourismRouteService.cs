using Thefieldofdreams.Application.DTOs;

namespace Thefieldofdreams.Application.Interfaces
{
    public interface ITourismRouteService
    {
        Task<IEnumerable<TourismRouteDto>> GetAllAsync();
        Task<TourismRouteDto?> GetByIdAsync(Guid id);
        Task<TourismRouteDto> CreateAsync(CreateTourismRouteRequestDto dto, string? createdBy);
        Task<bool> UpdateAsync(Guid id, UpdateTourismRouteRequestDto dto, string? updatedBy);
        Task<bool> DeleteAsync(Guid id);
    }
}
