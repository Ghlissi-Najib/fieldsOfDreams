using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class RewardService : IRewardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RewardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RewardDto>> GetAllAsync()
        {
            var now = DateTime.UtcNow;
            var items = await _unitOfWork.Rewards.GetAllRewardsAsync();
            return items
                .Where(r => r.IsActive
                    && (r.ExpiryDate == null || r.ExpiryDate > now)
                    && (r.Quantity == 0 || r.ClaimedCount < r.Quantity))
                .Select(MapToDto);
        }

        public async Task<RewardDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.Rewards.GetRewardByIdAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<RewardDto> CreateAsync(CreateRewardRequestDto dto, string? createdBy)
        {
            var entity = new Reward
            {
                Name = dto.Name,
                Description = dto.Description,
                PointsRequired = dto.PointsRequired,
                Type = dto.Type,
                ImageUrl = dto.ImageUrl,
                Quantity = dto.Quantity,
                ExpiryDate = dto.ExpiryDate,
                IsVIPOnly = dto.IsVIPOnly,
                CreatedBy = createdBy
            };

            await _unitOfWork.Rewards.CreateRewardAsync(entity);
            _unitOfWork.Complete();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateRewardRequestDto dto, string? updatedBy)
        {
            var entity = await _unitOfWork.Rewards.GetRewardByIdAsync(id);
            if (entity is null) return false;

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.PointsRequired = dto.PointsRequired;
            entity.Type = dto.Type;
            entity.ImageUrl = dto.ImageUrl;
            entity.Quantity = dto.Quantity;
            entity.ExpiryDate = dto.ExpiryDate;
            entity.IsVIPOnly = dto.IsVIPOnly;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _unitOfWork.Rewards.UpdateRewardAsync(entity);
            _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.Rewards.GetRewardByIdAsync(id);
            if (entity is null) return false;

            await _unitOfWork.Rewards.DeleteRewardAsync(id);
            _unitOfWork.Complete();
            return true;
        }

        private static RewardDto MapToDto(Reward r) => new()
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            PointsRequired = r.PointsRequired,
            Type = r.Type,
            ImageUrl = r.ImageUrl,
            Quantity = r.Quantity,
            ClaimedCount = r.ClaimedCount,
            ExpiryDate = r.ExpiryDate,
            IsVIPOnly = r.IsVIPOnly
        };
    }
}
