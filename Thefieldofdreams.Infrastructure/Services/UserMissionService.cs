using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class UserMissionService : IUserMissionService
    {
        private readonly ApplicationDbContext _context;

        public UserMissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserMissionDto>> GetAllAsync()
        {
            var items = await _context.UserMissions.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<UserMissionDto>> GetByUserIdAsync(Guid userId)
        {
            var items = await _context.UserMissions.Where(u => u.UserId == userId).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<UserMissionDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.UserMissions.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<UserMissionDto> CreateAsync(CreateUserMissionRequestDto dto, string? createdBy)
        {
            var entity = new UserMission
            {
                UserId = dto.UserId,
                MissionId = dto.MissionId,
                CreatedBy = createdBy
            };

            _context.UserMissions.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateProgressAsync(Guid id, UpdateUserMissionProgressDto dto, string? updatedBy)
        {
            var entity = await _context.UserMissions.FindAsync(id);
            if (entity is null) return false;

            entity.CurrentProgress = dto.CurrentProgress;
            entity.IsCompleted = dto.IsCompleted;
            entity.PointsAwarded = dto.PointsAwarded;
            if (dto.IsCompleted && entity.CompletedAt is null)
                entity.CompletedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.UserMissions.FindAsync(id);
            if (entity is null) return false;

            _context.UserMissions.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static UserMissionDto MapToDto(UserMission u) => new()
        {
            Id = u.Id,
            UserId = u.UserId,
            MissionId = u.MissionId,
            CurrentProgress = u.CurrentProgress,
            IsCompleted = u.IsCompleted,
            CompletedAt = u.CompletedAt,
            PointsAwarded = u.PointsAwarded
        };
    }
}
