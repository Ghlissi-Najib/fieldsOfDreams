using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class QRCampaignService : IQRCampaignService
    {
        private readonly ApplicationDbContext _context;

        public QRCampaignService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QRCampaignDto>> GetAllAsync()
        {
            var items = await _context.QRCampaigns.ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<IEnumerable<QRCampaignDto>> GetByCampaignIdAsync(Guid campaignId)
        {
            var items = await _context.QRCampaigns.Where(q => q.CampaignId == campaignId).ToListAsync();
            return items.Select(MapToDto);
        }

        public async Task<QRCampaignDto?> GetByIdAsync(Guid id)
        {
            var entity = await _context.QRCampaigns.FindAsync(id);
            return entity is null ? null : MapToDto(entity);
        }

        public async Task<QRCampaignDto> CreateAsync(CreateQRCampaignRequestDto dto, string? createdBy)
        {
            var entity = new QRCampaign
            {
                CampaignName = dto.CampaignName,
                Description = dto.Description,
                CampaignId = dto.CampaignId,
                QRCodeId = dto.QRCodeId,
                MaxScansPerUser = dto.MaxScansPerUser,
                TotalScanLimit = dto.TotalScanLimit,
                BasePointsReward = dto.BasePointsReward,
                BonusPointsForFirstScan = dto.BonusPointsForFirstScan,
                RewardCode = dto.RewardCode,
                DiscountPercentage = dto.DiscountPercentage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive,
                TargetUserGroup = dto.TargetUserGroup,
                MinimumUserLevel = dto.MinimumUserLevel,
                SuccessMessage = dto.SuccessMessage,
                RedirectUrl = dto.RedirectUrl,
                SponsorOfferCode = dto.SponsorOfferCode,
                IsSponsorExclusive = dto.IsSponsorExclusive,
                CreatedBy = createdBy
            };

            _context.QRCampaigns.Add(entity);
            await _context.SaveChangesAsync();
            return MapToDto(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateQRCampaignRequestDto dto, string? updatedBy)
        {
            var entity = await _context.QRCampaigns.FindAsync(id);
            if (entity is null) return false;

            entity.CampaignName = dto.CampaignName;
            entity.Description = dto.Description;
            entity.MaxScansPerUser = dto.MaxScansPerUser;
            entity.TotalScanLimit = dto.TotalScanLimit;
            entity.BasePointsReward = dto.BasePointsReward;
            entity.BonusPointsForFirstScan = dto.BonusPointsForFirstScan;
            entity.RewardCode = dto.RewardCode;
            entity.DiscountPercentage = dto.DiscountPercentage;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.IsActive = dto.IsActive;
            entity.TargetUserGroup = dto.TargetUserGroup;
            entity.MinimumUserLevel = dto.MinimumUserLevel;
            entity.SuccessMessage = dto.SuccessMessage;
            entity.RedirectUrl = dto.RedirectUrl;
            entity.SponsorOfferCode = dto.SponsorOfferCode;
            entity.IsSponsorExclusive = dto.IsSponsorExclusive;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = updatedBy;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.QRCampaigns.FindAsync(id);
            if (entity is null) return false;

            _context.QRCampaigns.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static QRCampaignDto MapToDto(QRCampaign q) => new()
        {
            Id = q.Id,
            CampaignName = q.CampaignName,
            Description = q.Description,
            CampaignId = q.CampaignId,
            QRCodeId = q.QRCodeId,
            MaxScansPerUser = q.MaxScansPerUser,
            TotalScanLimit = q.TotalScanLimit,
            CurrentScanCount = q.CurrentScanCount,
            BasePointsReward = q.BasePointsReward,
            BonusPointsForFirstScan = q.BonusPointsForFirstScan,
            RewardCode = q.RewardCode,
            DiscountPercentage = q.DiscountPercentage,
            StartDate = q.StartDate,
            EndDate = q.EndDate,
            IsActive = q.IsActive,
            TargetUserGroup = q.TargetUserGroup,
            MinimumUserLevel = q.MinimumUserLevel,
            UniqueUsersScanned = q.UniqueUsersScanned,
            ConversionCount = q.ConversionCount,
            ConversionRate = q.ConversionRate,
            SuccessMessage = q.SuccessMessage,
            RedirectUrl = q.RedirectUrl,
            SponsorOfferCode = q.SponsorOfferCode,
            IsSponsorExclusive = q.IsSponsorExclusive
        };
    }
}
