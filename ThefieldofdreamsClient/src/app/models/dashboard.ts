// ─── Shared building blocks ──────────────────────────────────────────────────
export interface ChartSeriesDto {
  labels: string[];
  values: number[];
}

export interface ChartSeriesDecimalDto {
  labels: string[];
  values: number[];
}

export interface SeriesData {
  name: string;
  values: number[];
}

export interface MultiSeriesChartDto {
  labels: string[];
  series: SeriesData[];
}

// ─── Admin dashboard ─────────────────────────────────────────────────────────
export interface AdminSummaryDto {
  totalUsers: number;
  totalPredictions: number;
  totalQRScans: number;
  totalRewards: number;
  activeCampaigns: number;
  missionsCompleted: number;
  totalWalletBalance: number;
  totalReferrals: number;
}

export interface MatchStatsDto {
  total: number;
  live: number;
  scheduled: number;
  completed: number;
}

export interface PredictionAccuracyDto {
  correct: number;
  wrong: number;
  pending: number;
}

export interface LeaderboardRowDto {
  rank: number;
  userName: string;
  points: number;
  wins: number;
}

export interface AdminDashboardDto {
  summary: AdminSummaryDto;
  usersByRole: ChartSeriesDto;
  matchStats: MatchStatsDto;
  weeklyActivity: MultiSeriesChartDto;
  predictionAccuracy: PredictionAccuracyDto;
  rewardsByType: ChartSeriesDto;
  qrScansByCode: ChartSeriesDto;
  topLeaderboard: LeaderboardRowDto[];
  walletTrend: ChartSeriesDecimalDto;
}

// ─── Passenger dashboard ─────────────────────────────────────────────────────
export interface PassengerSummaryDto {
  totalPoints: number;
  totalPredictions: number;
  correctPredictions: number;
  pendingPredictions: number;
  missionsCompleted: number;
  rewardsClaimed: number;
  qrScansCount: number;
  walletBalance: number;
  referralsCount: number;
  routesCompleted: number;
}

export interface MissionProgressDto {
  name: string;
  currentProgress: number;
  required: number;
  type: string;
  isCompleted: boolean;
}

export interface PassengerDashboardDto {
  summary: PassengerSummaryDto;
  predictionAccuracy: PredictionAccuracyDto;
  weeklyPredictions: ChartSeriesDto;
  pointsTrend: ChartSeriesDto;
  activityBreakdown: ChartSeriesDto;
  missionProgress: MissionProgressDto[];
}

// ─── Merchant dashboard ──────────────────────────────────────────────────────
export interface MerchantSummaryDto {
  totalQRCodes: number;
  totalScans: number;
  activeCampaigns: number;
  totalPointsGiven: number;
  uniqueUsersScanned: number;
}

export interface QRCodePerformanceDto {
  code: string;
  title: string;
  scans: number;
  maxScans: number;
  pointsAwarded: number;
}

export interface MerchantDashboardDto {
  summary: MerchantSummaryDto;
  scansByDay: MultiSeriesChartDto;
  qrCodePerformance: QRCodePerformanceDto[];
  pointsGivenByCode: ChartSeriesDto;
}

// ─── Legacy ──────────────────────────────────────────────────────────────────
export interface DashboardStats {
  totalUsers: number;
  totalMatches: number;
  liveMatches: number;
  completedMatches: number;
  totalPredictions: number;
  totalRewards: number;
  totalLeaderboardEntries: number;
}
