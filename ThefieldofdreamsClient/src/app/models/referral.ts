export interface Referral {
  id: string;
  referrerUserId: string;
  referredUserId: string;
  pointsAwarded: number;
  isCompleted: boolean;
  createdAt: string;
}

export interface CreateReferralRequest {
  referrerUserId: string;
  referredUserId: string;
  pointsAwarded: number;
}
