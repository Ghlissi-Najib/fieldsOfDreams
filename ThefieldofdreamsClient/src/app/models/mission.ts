export type MissionType =
  | 'PredictMatch'
  | 'ScanQRCode'
  | 'ShareReferral'
  | 'CompleteTourismRoute'
  | 'InviteFriend'
  | 'DailyLogin'
  | 'ReachLeaderboardRank'
  | 'MakePredictionStreak';

export type MissionDifficulty = 'Easy' | 'Medium' | 'Hard' | 'Expert';

export interface Mission {
  id: string;
  title: string;
  description?: string;
  type: MissionType;
  requiredProgress: number;
  pointsReward: number;
  xpValue?: number;
  difficulty: MissionDifficulty;
  startDate?: string;
  endDate?: string;
  isDaily: boolean;
  requiredUserLevel?: number;
}

export interface UserMission {
  id: string;
  userId: string;
  missionId: string;
  currentProgress: number;
  isCompleted: boolean;
  completedAt?: string;
  pointsAwarded: number;
}

export interface CreateMissionRequest {
  title: string;
  description?: string;
  type: MissionType;
  requiredProgress: number;
  pointsReward: number;
  xpValue?: number;
  difficulty: MissionDifficulty;
  startDate?: string;
  endDate?: string;
  isDaily: boolean;
  requiredUserLevel?: number;
}

export interface UpdateMissionRequest extends CreateMissionRequest {}

export interface UpdateUserMissionProgressRequest {
  currentProgress: number;
  isCompleted: boolean;
  pointsAwarded: number;
}
