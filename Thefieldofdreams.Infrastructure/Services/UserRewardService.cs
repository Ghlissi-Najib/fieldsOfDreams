using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Domain.Interfaces;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class UserRewardService : IUserRewardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;

        public UserRewardService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<IEnumerable<UserRewardDto>> GetAllAsync()
        {
            var items = await _context.UserRewards.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<UserRewardDto>> GetByUserIdAsync(Guid userId)
        {
            var items = await _unitOfWork.UserRewards.GetUserRewardsByUserIdAsync(userId);
            return items.Select(MapToDto);
        }

        public async Task<UserRewardDto?> GetByIdAsync(Guid id)
        {
            var entity = await _unitOfWork.UserRewards.GetUserRewardByIdAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<UserRewardDto> CreateAsync(CreateUserRewardRequestDto dto, string? createdBy)
        {
            var reward = await _unitOfWork.Rewards.GetRewardByIdAsync(dto.RewardId);

            var entity = new UserReward
            {
                UserId = dto.UserId,
                RewardId = dto.RewardId,
                RewardName = reward?.Name ?? string.Empty,
                RewardDescription = reward?.Description,
                RewardType = reward?.Type ?? default,
                PointsSpent = reward?.PointsRequired ?? 0,
                DeliveryMethod = dto.DeliveryMethod,
                DeliveryEmail = dto.DeliveryEmail,
                SourceCampaign = dto.SourceCampaign,
                AcquisitionChannel = dto.AcquisitionChannel,
                ClaimedAt = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await _unitOfWork.UserRewards.CreateUserRewardAsync(entity);
            _unitOfWork.Complete();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateUserRewardRequestDto dto, string? updatedBy)
        {
            var entity = await _unitOfWork.UserRewards.GetUserRewardByIdAsync(id);
            if (entity is null) return false;

            entity.IsUsed = dto.IsUsed;
            entity.UsageLocation = dto.UsageLocation;
            entity.UsageDeviceInfo = dto.UsageDeviceInfo;
            entity.SatisfactionRating = dto.SatisfactionRating;
            entity.FeedbackComment = dto.FeedbackComment;
            if (dto.IsUsed && entity.UsedAt is null)
                entity.UsedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _unitOfWork.UserRewards.UpdateUserRewardAsync(entity);
            _unitOfWork.Complete();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _unitOfWork.UserRewards.GetUserRewardByIdAsync(id);
            if (entity is null) return false;

            await _unitOfWork.UserRewards.DeleteUserRewardAsync(id);
            _unitOfWork.Complete();
            return true;
        }

        private static UserRewardDto MapToDto(UserReward u) => new()
        {
            Id = u.Id,
            UserId = u.UserId,
            RewardId = u.RewardId,
            ClaimedAt = u.ClaimedAt,
            UsedAt = u.UsedAt,
            IsUsed = u.IsUsed,
            IsExpired = u.IsExpired,
            RewardName = u.RewardName,
            RewardDescription = u.RewardDescription,
            RewardType = u.RewardType,
            PointsSpent = u.PointsSpent,
            UsageCode = u.UsageCode,
            DeliveryMethod = u.DeliveryMethod,
            QrCodeValue = u.QrCodeValue,
            BarcodeValue = u.BarcodeValue,
            PartnerRedemptionCode = u.PartnerRedemptionCode,
            PartnerName = u.PartnerName,
            ExpiryDate = u.ExpiryDate,
            SatisfactionRating = u.SatisfactionRating,
            FeedbackComment = u.FeedbackComment,
            AcquisitionChannel = u.AcquisitionChannel
        };
    }
}
