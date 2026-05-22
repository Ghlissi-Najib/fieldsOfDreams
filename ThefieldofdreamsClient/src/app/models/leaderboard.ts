export interface LeaderboardEntry {
  id: string;
  userId: string;
  username?: string;
  totalPoints: number;
  weeklyPoints: number;
  monthlyPoints: number;
  rank: number;
  leaderboardType?: string; // 'Global' | 'Weekly' | 'Monthly' | 'Ship'
  cruiseSessionId?: string;
  shipName?: string;
  lastUpdated: string;
}

export interface CreateLeaderboardEntryRequest {
  userId: string;
  username?: string;
  totalPoints: number;
  weeklyPoints: number;
  monthlyPoints: number;
  rank: number;
  leaderboardType?: string;
  cruiseSessionId?: string;
  shipName?: string;
}

export interface UpdateLeaderboardEntryRequest {
  username?: string;
  totalPoints: number;
  weeklyPoints: number;
  monthlyPoints: number;
  rank: number;
  leaderboardType?: string;
  cruiseSessionId?: string;
  shipName?: string;
}
