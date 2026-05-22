export interface TourismRoute {
  id: string;
  name: string;
  description?: string;
  totalPointsReward: number;
  estimatedDurationMinutes: number;
  isHiddenGemRoute: boolean;
}

export interface CreateTourismRouteRequest {
  name: string;
  description?: string;
  totalPointsReward: number;
  estimatedDurationMinutes: number;
  isHiddenGemRoute: boolean;
}

export interface UpdateTourismRouteRequest extends CreateTourismRouteRequest {}

export interface UserRouteCompletion {
  id: string;
  userId: string;
  routeId: string;
  startedAt: string;
  completedAt?: string;
  isCompleted: boolean;
  completionTimeMinutes: number;
  locationsVisited: number;
  totalLocationsInRoute: number;
  pointsEarned: number;
  bonusPointsEarned: number;
  bonusAwarded: boolean;
  rewardCode?: string;
  isPerfectCompletion: boolean;
  isSpeedRun: boolean;
  rankPosition?: number;
  isSharedOnSocial: boolean;
  rating?: number;
  reviewComment?: string;
  earnedBadge: boolean;
  badgeName?: string;
  experiencePointsGained: number;
}
