using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class MissionService : IMissionService
    {
        private readonly ApplicationDbContext _context;

        public MissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MissionDto>> GetAllAsync()
        {
            var items = await _context.Set<Mission>().ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<MissionDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<Mission>().FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<MissionDto> CreateAsync(CreateMissionRequestDto dto, string? createdBy)
        {
            var entity = new Mission
            {
                Title = dto.Title,
                Description = dto.Description,
                Type = dto.Type,
                RequiredProgress = dto.RequiredProgress,
                PointsReward = dto.PointsReward,
                XPValue = dto.XPValue,
                Difficulty = dto.Difficulty,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsDaily = dto.IsDaily,
                RequiredUserLevel = dto.RequiredUserLevel,
                CreatedBy = createdBy
            };

            _context.Set<Mission>().Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateMissionRequestDto dto, string? updatedBy)
        {
            var entity = await _context.Set<Mission>().FindAsync(id);
            if (entity is null) return false;

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.Type = dto.Type;
            entity.RequiredProgress = dto.RequiredProgress;
            entity.PointsReward = dto.PointsReward;
            entity.XPValue = dto.XPValue;
            entity.Difficulty = dto.Difficulty;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.IsDaily = dto.IsDaily;
            entity.RequiredUserLevel = dto.RequiredUserLevel;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Set<Mission>().FindAsync(id);
            if (entity is null) return false;

            _context.Set<Mission>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static MissionDto MapToDto(Mission m) => new()
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            Type = m.Type,
            RequiredProgress = m.RequiredProgress,
            PointsReward = m.PointsReward,
            XPValue = m.XPValue,
            Difficulty = m.Difficulty,
            StartDate = m.StartDate,
            EndDate = m.EndDate,
            IsDaily = m.IsDaily,
            RequiredUserLevel = m.RequiredUserLevel
        };
    }
}
