namespace Thefieldofdreams.Application.DTOs
{
    // ─── Shared building blocks ───────────────────────────────────────────────
    public class ChartSeriesDto
    {
        public List<string> Labels { get; set; } = [];
        public List<int>    Values { get; set; } = [];
    }

    public class ChartSeriesDecimalDto
    {
        public List<string>  Labels { get; set; } = [];
        public List<decimal> Values { get; set; } = [];
    }

    public class MultiSeriesChartDto
    {
        public List<string>       Labels  { get; set; } = [];
        public List<SeriesData>   Series  { get; set; } = [];
    }

    public class SeriesData
    {
        public required string  Name   { get; set; }
        public List<int>        Values { get; set; } = [];
    }

    // ─── Admin dashboard ─────────────────────────────────────────────────────
    public class AdminDashboardDto
    {
        public AdminSummaryDto         Summary             { get; set; } = new();
        public ChartSeriesDto          UsersByRole         { get; set; } = new();
        public MatchStatsDto           MatchStats          { get; set; } = new();
        public MultiSeriesChartDto     WeeklyActivity      { get; set; } = new();
        public PredictionAccuracyDto   PredictionAccuracy  { get; set; } = new();
        public ChartSeriesDto          RewardsByType       { get; set; } = new();
        public ChartSeriesDto          QRScansByCode       { get; set; } = new();
        public List<LeaderboardRowDto> TopLeaderboard      { get; set; } = [];
        public ChartSeriesDecimalDto   WalletTrend         { get; set; } = new();
    }

    public class AdminSummaryDto
    {
        public int     TotalUsers         { get; set; }
        public int     TotalPredictions   { get; set; }
        public int     TotalQRScans       { get; set; }
        public int     TotalRewards       { get; set; }
        public int     ActiveCampaigns    { get; set; }
        public int     MissionsCompleted  { get; set; }
        public decimal TotalWalletBalance { get; set; }
        public int     TotalReferrals     { get; set; }
    }

    public class MatchStatsDto
    {
        public int Total     { get; set; }
        public int Live      { get; set; }
        public int Scheduled { get; set; }
        public int Completed { get; set; }
    }

    public class PredictionAccuracyDto
    {
        public int Correct { get; set; }
        public int Wrong   { get; set; }
        public int Pending { get; set; }
    }

    public class LeaderboardRowDto
    {
        public int    Rank     { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int    Points   { get; set; }
        public int    Wins     { get; set; }
    }

    // ─── Passenger dashboard ─────────────────────────────────────────────────
    public class PassengerDashboardDto
    {
        public PassengerSummaryDto     Summary            { get; set; } = new();
        public PredictionAccuracyDto   PredictionAccuracy { get; set; } = new();
        public ChartSeriesDto          WeeklyPredictions  { get; set; } = new();
        public ChartSeriesDto          PointsTrend        { get; set; } = new();
        public ChartSeriesDto          ActivityBreakdown  { get; set; } = new();
        public List<MissionProgressDto> MissionProgress   { get; set; } = [];
    }

    public class PassengerSummaryDto
    {
        public int     TotalPoints         { get; set; }
        public int     TotalPredictions    { get; set; }
        public int     CorrectPredictions  { get; set; }
        public int     PendingPredictions  { get; set; }
        public int     MissionsCompleted   { get; set; }
        public int     RewardsClaimed      { get; set; }
        public int     QRScansCount        { get; set; }
        public decimal WalletBalance       { get; set; }
        public int     ReferralsCount      { get; set; }
        public int     RoutesCompleted     { get; set; }
    }

    public class MissionProgressDto
    {
        public string Name            { get; set; } = string.Empty;
        public int    CurrentProgress { get; set; }
        public int    Required        { get; set; }
        public string Type            { get; set; } = string.Empty;
        public bool   IsCompleted     { get; set; }
    }

    // ─── Merchant dashboard ──────────────────────────────────────────────────
    public class MerchantDashboardDto
    {
        public MerchantSummaryDto        Summary            { get; set; } = new();
        public MultiSeriesChartDto       ScansByDay         { get; set; } = new();
        public List<QRCodePerformanceDto> QRCodePerformance { get; set; } = [];
        public ChartSeriesDto            PointsGivenByCode  { get; set; } = new();
    }

    public class MerchantSummaryDto
    {
        public int TotalQRCodes      { get; set; }
        public int TotalScans        { get; set; }
        public int ActiveCampaigns   { get; set; }
        public int TotalPointsGiven  { get; set; }
        public int UniqueUsersScanned { get; set; }
    }

    public class QRCodePerformanceDto
    {
        public string Code     { get; set; } = string.Empty;
        public string Title    { get; set; } = string.Empty;
        public int    Scans    { get; set; }
        public int    MaxScans { get; set; }
        public int    PointsAwarded { get; set; }
    }

    // Legacy – kept for backwards compat
    public class DashboardStatsDto
    {
        public int TotalUsers             { get; set; }
        public int TotalMatches           { get; set; }
        public int LiveMatches            { get; set; }
        public int CompletedMatches       { get; set; }
        public int TotalPredictions       { get; set; }
        public int TotalRewards           { get; set; }
        public int TotalLeaderboardEntries { get; set; }
    }
}
