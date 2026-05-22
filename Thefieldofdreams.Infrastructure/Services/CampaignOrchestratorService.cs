using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class CampaignOrchestratorService : ICampaignOrchestrator
    {
        private readonly ApplicationDbContext _context;

        public CampaignOrchestratorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Campaign>> GetActiveCampaignsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Campaigns
                .Where(c => c.Status == CampaignStatus.Active
                         && c.StartDate <= now
                         && c.EndDate >= now)
                .OrderByDescending(c => c.Priority)
                .ToListAsync();
        }

        public async Task<bool> IsRewardAvailableAsync(Guid rewardId, Guid userId)
        {
            var reward = await _context.Rewards.FindAsync(rewardId);
            if (reward is null || !reward.IsActive)
                return false;

            if (reward.ExpiryDate.HasValue && reward.ExpiryDate.Value < DateTime.UtcNow)
                return false;

            if (reward.Quantity > 0 && reward.ClaimedCount >= reward.Quantity)
                return false;

            return true;
        }

        public async Task<bool> IsQRCodeValidAsync(Guid qrCodeId, Guid userId)
        {
            var qrCode = await _context.QRCodes.FindAsync(qrCodeId);
            if (qrCode is null || !qrCode.IsActive)
                return false;

            if (qrCode.ExpiryDate.HasValue && qrCode.ExpiryDate.Value < DateTime.UtcNow)
                return false;

            if (qrCode.MaxScans > 0 && qrCode.CurrentScanCount >= qrCode.MaxScans)
                return false;

            // Check whether any linked QRCampaign is active and the user hasn't exceeded their per-user scan limit
            var qrCampaign = await _context.QRCampaigns
                .Include(qc => qc.Campaign)
                .FirstOrDefaultAsync(qc => qc.QRCodeId == qrCodeId && qc.IsActive);

            if (qrCampaign is not null)
            {
                var now = DateTime.UtcNow;

                if (qrCampaign.EndDate.HasValue && qrCampaign.EndDate.Value < now)
                    return false;

                if (qrCampaign.Campaign is not null && qrCampaign.Campaign.Status != CampaignStatus.Active)
                    return false;

                var userScanCount = await _context.QRScans
                    .CountAsync(s => s.QRCodeId == qrCodeId && s.UserId == userId);

                if (qrCampaign.MaxScansPerUser > 0 && userScanCount >= qrCampaign.MaxScansPerUser)
                    return false;
            }

            return true;
        }

        public async Task<OrchestrationStateDto> GetOrchestrationStateAsync(Guid userId, string role, int userPoints)
        {
            var activeCampaigns = (await GetActiveCampaignsAsync()).ToList();

            var visibleRewards = await _context.Rewards
                .Where(r => r.IsActive
                    && (r.ExpiryDate == null || r.ExpiryDate > DateTime.UtcNow)
                    && (r.Quantity == 0 || r.ClaimedCount < r.Quantity))
                .Select(r => r.Id)
                .ToListAsync();

            var validQRCodes = await _context.QRCodes
                .Where(q => q.IsActive
                    && (q.ExpiryDate == null || q.ExpiryDate > DateTime.UtcNow)
                    && (q.MaxScans == 0 || q.CurrentScanCount < q.MaxScans))
                .Select(q => q.Id)
                .ToListAsync();

            // Per-user scan exclusions (QR codes the user has already maxed out)
            var userScanCounts = await _context.QRScans
                .Where(s => s.UserId == userId)
                .GroupBy(s => s.QRCodeId)
                .Select(g => new { QRCodeId = g.Key, Count = g.Count() })
                .ToListAsync();

            var maxScansLookup = await _context.QRCampaigns
                .Where(qc => qc.IsActive)
                .Select(qc => new { qc.QRCodeId, qc.MaxScansPerUser })
                .ToListAsync();

            foreach (var sc in userScanCounts)
            {
                var limit = maxScansLookup.FirstOrDefault(m => m.QRCodeId == sc.QRCodeId);
                if (limit is not null && limit.MaxScansPerUser > 0 && sc.Count >= limit.MaxScansPerUser)
                    validQRCodes.Remove(sc.QRCodeId);
            }

            var enabledFlows = BuildEnabledFlows(role, activeCampaigns);

            return new OrchestrationStateDto
            {
                UserId = userId.ToString(),
                UserRole = role,
                UserPoints = userPoints,
                ActiveCampaigns = activeCampaigns.Select(c => new ActiveCampaignSummaryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Type = c.Type,
                    EndDate = c.EndDate,
                    Priority = c.Priority
                }).ToList(),
                VisibleRewardIds = visibleRewards,
                ValidQRCodeIds = validQRCodes,
                EnabledFlows = enabledFlows
            };
        }

        private static Dictionary<string, bool> BuildEnabledFlows(string role, List<Campaign> activeCampaigns)
        {
            var hasQR = activeCampaigns.Any(c => c.Type is CampaignType.QRBased or CampaignType.Hybrid);
            var hasPrediction = activeCampaigns.Any(c => c.Type is CampaignType.PredictionBased or CampaignType.Hybrid);
            var hasReferral = activeCampaigns.Any(c => c.Type == CampaignType.ReferralBased);
            var hasTourism = activeCampaigns.Any(c => c.Type == CampaignType.TourismBased);

            return new Dictionary<string, bool>
            {
                ["predictions"] = hasPrediction && role is "passenger",
                ["qrScanning"] = hasQR && role is "passenger",
                ["rewardClaim"] = role is "passenger",
                ["perkValidation"] = role is "merchant" or "admin",
                ["referral"] = hasReferral && role is "passenger",
                ["tourismRoutes"] = hasTourism && role is "passenger",
                ["campaignManagement"] = role is "admin",
                ["liveSlots"] = role is "merchant",
            };
        }
    }
}
