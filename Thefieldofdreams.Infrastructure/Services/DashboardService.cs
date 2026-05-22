using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thefieldofdreams.Application.DTOs;
using Thefieldofdreams.Application.Interfaces;
using Thefieldofdreams.Domain.Entities;
using Thefieldofdreams.Infrastructure.Data;
using Thefieldofdreams.Infrastructure.Identity;

namespace Thefieldofdreams.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DashboardService(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ─── Legacy ───────────────────────────────────────────────────────────
        public async Task<DashboardStatsDto> GetSummaryAsync()
        {
            return new DashboardStatsDto
            {
                TotalUsers             = await _userManager.Users.CountAsync(),
                TotalMatches           = await _context.Matchs.CountAsync(),
                LiveMatches            = await _context.Matchs.CountAsync(m => m.Status == MatchStatus.Live),
                CompletedMatches       = await _context.Matchs.CountAsync(m => m.Status == MatchStatus.Completed),
                TotalPredictions       = await _context.Predictions.CountAsync(),
                TotalRewards           = await _context.Rewards.CountAsync(),
                TotalLeaderboardEntries = await _context.LeaderboardEntries.CountAsync()
            };
        }

        // ─── Admin dashboard ──────────────────────────────────────────────────
        public async Task<AdminDashboardDto> GetAdminDashboardAsync()
        {
            var now = DateTime.UtcNow;
            var weekAgo = now.AddDays(-6);
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => now.AddDays(-6 + i).Date)
                .ToList();

            // Summary counts (parallel)
            var totalUsers       = await _userManager.Users.CountAsync();
            var totalPredictions = await _context.Predictions.CountAsync();
            var totalQRScans     = await _context.QRScans.CountAsync();
            var totalRewards     = await _context.Rewards.CountAsync();
            var activeCampaigns  = await _context.Campaigns.CountAsync(c => c.Status == CampaignStatus.Active);
            var missionsDone     = await _context.UserMissions.CountAsync(um => um.IsCompleted);
            var totalBalance     = await _context.Wallets.SumAsync(w => (decimal?)w.Balance) ?? 0m;
            var totalReferrals   = await _context.Referrals.CountAsync();

            // Users by role
            var adminUsers     = (await _userManager.GetUsersInRoleAsync("Admin")).Count;
            var merchantUsers  = (await _userManager.GetUsersInRoleAsync("Merchant")).Count;
            var partnerUsers   = (await _userManager.GetUsersInRoleAsync("Partner")).Count;
            var passengerUsers = (await _userManager.GetUsersInRoleAsync("Passenger")).Count;

            // Match stats
            var matchStats = new MatchStatsDto
            {
                Total     = await _context.Matchs.CountAsync(),
                Live      = await _context.Matchs.CountAsync(m => m.Status == MatchStatus.Live),
                Scheduled = await _context.Matchs.CountAsync(m => m.Status == MatchStatus.Scheduled),
                Completed = await _context.Matchs.CountAsync(m => m.Status == MatchStatus.Completed)
            };

            // Weekly activity – predictions + QR scans per day (last 7 days)
            var predsByDay = await _context.Predictions
                .Where(p => p.PredictedAt >= weekAgo)
                .GroupBy(p => p.PredictedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            var scansByDay = await _context.QRScans
                .Where(s => s.ScannedAt >= weekAgo)
                .GroupBy(s => s.ScannedAt.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync();

            var weeklyActivity = new MultiSeriesChartDto
            {
                Labels = last7Days.Select(d => d.ToString("ddd")).ToList(),
                Series =
                [
                    new SeriesData
                    {
                        Name   = "Predictions",
                        Values = last7Days.Select(d => predsByDay.FirstOrDefault(x => x.Date == d)?.Count ?? 0).ToList()
                    },
                    new SeriesData
                    {
                        Name   = "QR Scans",
                        Values = last7Days.Select(d => scansByDay.FirstOrDefault(x => x.Date == d)?.Count ?? 0).ToList()
                    }
                ]
            };

            // Prediction accuracy
            var correct = await _context.Predictions.CountAsync(p => p.IsCorrect);
            var pending = await _context.Predictions.CountAsync(p => p.Status == PredictionStatus.Pending);
            var wrong   = totalPredictions - correct - pending;

            // Rewards by type
            var rewardTypes = await _context.Rewards
                .GroupBy(r => r.Type)
                .Select(g => new { Type = g.Key.ToString(), Count = g.Count() })
                .ToListAsync();

            // QR scans by code (top 8)
            var qrByCode = await _context.QRScans
                .Include(s => s.QRCode)
                .GroupBy(s => s.QRCode.Title)
                .Select(g => new { Title = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(8)
                .ToListAsync();

            // Top leaderboard
            var topBoard = await _context.LeaderboardEntries
                .OrderByDescending(e => e.TotalPoints)
                .Take(5)
                .Select(e => new LeaderboardRowDto
                {
                    Rank     = e.Rank,
                    UserName = e.Username ?? "Unknown",
                    Points   = e.TotalPoints,
                    Wins     = 0
                })
                .ToListAsync();

            // Wallet balance trend (last 7 days – sum balance per day based on wallet creation, approximation)
            var walletTrend = new ChartSeriesDecimalDto
            {
                Labels = last7Days.Select(d => d.ToString("ddd")).ToList(),
                Values = last7Days.Select(_ => totalBalance / 7).ToList() // simplified trend
            };

            return new AdminDashboardDto
            {
                Summary = new AdminSummaryDto
                {
                    TotalUsers         = totalUsers,
                    TotalPredictions   = totalPredictions,
                    TotalQRScans       = totalQRScans,
                    TotalRewards       = totalRewards,
                    ActiveCampaigns    = activeCampaigns,
                    MissionsCompleted  = missionsDone,
                    TotalWalletBalance = totalBalance,
                    TotalReferrals     = totalReferrals
                },
                UsersByRole = new ChartSeriesDto
                {
                    Labels = ["Admin", "Merchant", "Partner", "Passenger"],
                    Values = [adminUsers, merchantUsers, partnerUsers, passengerUsers]
                },
                MatchStats         = matchStats,
                WeeklyActivity     = weeklyActivity,
                PredictionAccuracy = new PredictionAccuracyDto { Correct = correct, Wrong = wrong, Pending = pending },
                RewardsByType = new ChartSeriesDto
                {
                    Labels = rewardTypes.Select(r => r.Type).ToList(),
                    Values = rewardTypes.Select(r => r.Count).ToList()
                },
                QRScansByCode = new ChartSeriesDto
                {
                    Labels = qrByCode.Select(q => q.Title ?? "Unknown").ToList(),
                    Values = qrByCode.Select(q => q.Count).ToList()
                },
                TopLeaderboard = topBoard,
                WalletTrend    = walletTrend
            };
        }

        // ─── Passenger dashboard ──────────────────────────────────────────────
        public async Task<PassengerDashboardDto> GetPassengerDashboardAsync(Guid userId)
        {
            var now     = DateTime.UtcNow;
            var weekAgo = now.AddDays(-6);
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => now.AddDays(-6 + i).Date)
                .ToList();

            // Predictions
            var preds           = await _context.Predictions.Where(p => p.UserId == userId).ToListAsync();
            var correct         = preds.Count(p => p.IsCorrect);
            var pending         = preds.Count(p => p.Status == PredictionStatus.Pending);
            var wrong           = preds.Count - correct - pending;

            // Wallet
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);

            // Missions
            var missions        = await _context.UserMissions
                .Include(um => um.Mission)
                .Where(um => um.UserId == userId)
                .ToListAsync();
            var missionsDone    = missions.Count(m => m.IsCompleted);

            // Rewards claimed
            var rewardsClaimed  = await _context.UserRewards.CountAsync(ur => ur.UserId == userId);

            // QR scans
            var qrScans         = await _context.QRScans.CountAsync(s => s.UserId == userId);

            // Referrals
            var referrals       = await _context.Referrals.CountAsync(r => r.ReferrerUserId == userId);

            // Routes completed
            var routesDone      = await _context.UserRouteCompletions.CountAsync(rc => rc.UserId == userId && rc.IsCompleted);

            // Weekly predictions per day
            var predsByDay = preds
                .Where(p => p.PredictedAt >= weekAgo)
                .GroupBy(p => p.PredictedAt.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            // Points trend – cumulative points earned per day over last 7 days
            var pointsByDay = preds
                .Where(p => p.PredictedAt >= weekAgo)
                .GroupBy(p => p.PredictedAt.Date)
                .ToDictionary(g => g.Key, g => g.Sum(p => p.PointsEarned));

            int running = (wallet?.Points ?? 0) - pointsByDay.Values.Sum();
            var pointsTrend = last7Days.Select(d =>
            {
                running += pointsByDay.GetValueOrDefault(d, 0);
                return running;
            }).ToList();

            // Activity breakdown
            var qrCount      = await _context.QRScans.CountAsync(s => s.UserId == userId);
            var routeCount   = await _context.UserRouteCompletions.CountAsync(rc => rc.UserId == userId);
            var rewardCount  = rewardsClaimed;
            var referralCount = referrals;

            // Mission progress (top 5 active)
            var missionProgress = missions
                .Where(m => !m.IsCompleted)
                .Take(5)
                .Select(m => new MissionProgressDto
                {
                    Name            = m.Mission?.Title ?? "Mission",
                    CurrentProgress = m.CurrentProgress,
                    Required        = m.Mission?.RequiredProgress ?? 1,
                    Type            = m.Mission?.Type.ToString() ?? "",
                    IsCompleted     = m.IsCompleted
                })
                .ToList();

            return new PassengerDashboardDto
            {
                Summary = new PassengerSummaryDto
                {
                    TotalPoints        = wallet?.Points ?? 0,
                    TotalPredictions   = preds.Count,
                    CorrectPredictions = correct,
                    PendingPredictions = pending,
                    MissionsCompleted  = missionsDone,
                    RewardsClaimed     = rewardsClaimed,
                    QRScansCount       = qrScans,
                    WalletBalance      = wallet?.Balance ?? 0m,
                    ReferralsCount     = referrals,
                    RoutesCompleted    = routesDone
                },
                PredictionAccuracy = new PredictionAccuracyDto
                {
                    Correct = correct,
                    Wrong   = wrong,
                    Pending = pending
                },
                WeeklyPredictions = new ChartSeriesDto
                {
                    Labels = last7Days.Select(d => d.ToString("ddd")).ToList(),
                    Values = last7Days.Select(d => predsByDay.GetValueOrDefault(d, 0)).ToList()
                },
                PointsTrend = new ChartSeriesDto
                {
                    Labels = last7Days.Select(d => d.ToString("ddd")).ToList(),
                    Values = pointsTrend
                },
                ActivityBreakdown = new ChartSeriesDto
                {
                    Labels = ["Predictions", "QR Scans", "Routes", "Rewards", "Referrals"],
                    Values = [preds.Count, qrCount, routeCount, rewardCount, referralCount]
                },
                MissionProgress = missionProgress
            };
        }

        // ─── Merchant dashboard ───────────────────────────────────────────────
        public async Task<MerchantDashboardDto> GetMerchantDashboardAsync(Guid userId)
        {
            var now     = DateTime.UtcNow;
            var weekAgo = now.AddDays(-6);
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => now.AddDays(-6 + i).Date)
                .ToList();

            // QR codes owned by this user's sponsor
            var qrCodes   = await _context.QRCodes.ToListAsync();
            var allScans  = await _context.QRScans.Include(s => s.QRCode).ToListAsync();

            var totalScans       = allScans.Count;
            var totalPointsGiven = allScans.Sum(s => s.PointsAwarded);
            var uniqueUsers      = allScans.Select(s => s.UserId).Distinct().Count();
            var activeCampaigns  = await _context.Campaigns.CountAsync(c => c.Status == CampaignStatus.Active);

            // Scans per day (last 7)
            var scansByDay = allScans
                .Where(s => s.ScannedAt >= weekAgo)
                .GroupBy(s => s.ScannedAt.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            var pointsPerDay = allScans
                .Where(s => s.ScannedAt >= weekAgo)
                .GroupBy(s => s.ScannedAt.Date)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.PointsAwarded));

            var scansByDayChart = new MultiSeriesChartDto
            {
                Labels = last7Days.Select(d => d.ToString("ddd")).ToList(),
                Series =
                [
                    new SeriesData
                    {
                        Name   = "Scans",
                        Values = last7Days.Select(d => scansByDay.GetValueOrDefault(d, 0)).ToList()
                    },
                    new SeriesData
                    {
                        Name   = "Points Given",
                        Values = last7Days.Select(d => pointsPerDay.GetValueOrDefault(d, 0)).ToList()
                    }
                ]
            };

            // QR code performance
            var qrPerf = qrCodes.Select(qr =>
            {
                var scansForCode = allScans.Where(s => s.QRCodeId == qr.Id).ToList();
                return new QRCodePerformanceDto
                {
                    Code         = qr.Code,
                    Title        = qr.Title,
                    Scans        = scansForCode.Count,
                    MaxScans     = qr.MaxScans,
                    PointsAwarded = scansForCode.Sum(s => s.PointsAwarded)
                };
            })
            .OrderByDescending(q => q.Scans)
            .Take(8)
            .ToList();

            // Points given by code (for pie chart)
            var pointsGivenByCode = new ChartSeriesDto
            {
                Labels = qrPerf.Select(q => q.Title).ToList(),
                Values = qrPerf.Select(q => q.PointsAwarded).ToList()
            };

            return new MerchantDashboardDto
            {
                Summary = new MerchantSummaryDto
                {
                    TotalQRCodes       = qrCodes.Count,
                    TotalScans         = totalScans,
                    ActiveCampaigns    = activeCampaigns,
                    TotalPointsGiven   = totalPointsGiven,
                    UniqueUsersScanned = uniqueUsers
                },
                ScansByDay         = scansByDayChart,
                QRCodePerformance  = qrPerf,
                PointsGivenByCode  = pointsGivenByCode
            };
        }
    }
}
