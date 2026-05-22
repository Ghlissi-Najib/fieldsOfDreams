using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class TourismRouteService : ITourismRouteService
    {
        private readonly ApplicationDbContext _context;

        public TourismRouteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourismRouteDto>> GetAllAsync()
        {
            var items = await _context.TourismRoutes.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<TourismRouteDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.TourismRoutes.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<TourismRouteDto> CreateAsync(CreateTourismRouteRequestDto dto, string? createdBy)
        {
            var entity = new TourismRoute
            {
                Name = dto.Name,
                Description = dto.Description,
                TotalPointsReward = dto.TotalPointsReward,
                EstimatedDurationMinutes = dto.EstimatedDurationMinutes,
                IsHiddenGemRoute = dto.IsHiddenGemRoute,
                CreatedBy = createdBy
            };

            _context.TourismRoutes.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateTourismRouteRequestDto dto, string? updatedBy)
        {
            var entity = await _context.TourismRoutes.FindAsync(id);
            if (entity is null) return false;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.TotalPointsReward = dto.TotalPointsReward;
            entity.EstimatedDurationMinutes = dto.EstimatedDurationMinutes;
            entity.IsHiddenGemRoute = dto.IsHiddenGemRoute;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.TourismRoutes.FindAsync(id);
            if (entity is null) return false;

            _context.TourismRoutes.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static TourismRouteDto MapToDto(TourismRoute r) => new()
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            TotalPointsReward = r.TotalPointsReward,
            EstimatedDurationMinutes = r.EstimatedDurationMinutes,
            IsHiddenGemRoute = r.IsHiddenGemRoute
        };
    }
}
