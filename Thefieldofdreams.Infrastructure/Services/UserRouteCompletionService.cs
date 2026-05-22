using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class UserRouteCompletionService : IUserRouteCompletionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public UserRouteCompletionService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<UserRouteCompletionDto>> GetAllAsync()
        {
            var items = await _context.UserRouteCompletions.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<UserRouteCompletionDto>> GetByUserIdAsync(Guid userId)
        {
            var items = await _unitOfWork.UserRouteCompletions.GetUserRouteCompletionsByUserIdAsync(userId);
            return items.Select(MapToDto);
        }

        public async Task<UserRouteCompletionDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.UserRouteCompletions.GetUserRouteCompletionByIdAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<UserRouteCompletionDto> CreateAsync(CreateUserRouteCompletionRequestDto dto, string? createdBy)
        {
            var entity = new UserRouteCompletion
            {
                UserId = dto.UserId,
                RouteId = dto.RouteId,
                TotalLocationsInRoute = dto.TotalLocationsInRoute,
                StartedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await _unitOfWork.UserRouteCompletions.CreateUserRouteCompletionAsync(entity);
            _unitOfWork.Complete();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateUserRouteCompletionRequestDto dto, string? updatedBy)
        {
            var entity = await _unitOfWork.UserRouteCompletions.GetUserRouteCompletionByIdAsync(id);
            if (entity is null) return false;

            entity.IsCompleted = dto.IsCompleted;
            entity.CompletionTimeMinutes = dto.CompletionTimeMinutes;
            entity.LocationsVisited = dto.LocationsVisited;
            entity.PointsEarned = dto.PointsEarned;
            entity.BonusPointsEarned = dto.BonusPointsEarned;
            entity.BonusAwarded = dto.BonusAwarded;
            entity.RewardCode = dto.RewardCode;
            entity.IsPerfectCompletion = dto.IsPerfectCompletion;
            entity.IsSpeedRun = dto.IsSpeedRun;
            entity.RankPosition = dto.RankPosition;
            entity.IsSharedOnSocial = dto.IsSharedOnSocial;
            entity.Rating = dto.Rating;
            entity.ReviewComment = dto.ReviewComment;
            entity.EarnedBadge = dto.EarnedBadge;
            entity.BadgeName = dto.BadgeName;
            entity.ExperiencePointsGained = dto.ExperiencePointsGained;
            if (dto.IsCompleted && entity.CompletedAt is null)
                entity.CompletedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _unitOfWork.UserRouteCompletions.UpdateUserRouteCompletionAsync(entity);
            _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.UserRouteCompletions.GetUserRouteCompletionByIdAsync(id);
            if (entity is null) return false;

            await _unitOfWork.UserRouteCompletions.DeleteUserRouteCompletionAsync(id);
            _unitOfWork.Complete();
            return true;
        }

        private static UserRouteCompletionDto MapToDto(UserRouteCompletion u) => new()
        {
            Id = u.Id,
            UserId = u.UserId,
            RouteId = u.RouteId,
            StartedAt = u.StartedAt,
            CompletedAt = u.CompletedAt,
            IsCompleted = u.IsCompleted,
            CompletionTimeMinutes = u.CompletionTimeMinutes,
            LocationsVisited = u.LocationsVisited,
            TotalLocationsInRoute = u.TotalLocationsInRoute,
            PointsEarned = u.PointsEarned,
            BonusPointsEarned = u.BonusPointsEarned,
            BonusAwarded = u.BonusAwarded,
            RewardCode = u.RewardCode,
            IsPerfectCompletion = u.IsPerfectCompletion,
            IsSpeedRun = u.IsSpeedRun,
            RankPosition = u.RankPosition,
            IsSharedOnSocial = u.IsSharedOnSocial,
            Rating = u.Rating,
            ReviewComment = u.ReviewComment,
            EarnedBadge = u.EarnedBadge,
            BadgeName = u.BadgeName,
            ExperiencePointsGained = u.ExperiencePointsGained
        };
    }
}
