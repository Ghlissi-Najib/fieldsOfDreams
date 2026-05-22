export type RewardType =
  | 'DiscountCoupon'
  | 'FreeDrink'
  | 'Merchandise'
  | 'TicketUpgrade'
  | 'VIPAccess'
  | 'DigitalBadge'
  | 'Cashback';

export interface Reward {
  id: string;
  name: string;
  description?: string;
  pointsRequired: number;
  type: RewardType;
  imageUrl?: string;
  quantity: number;
  claimedCount: number;
  expiryDate?: string;
  isVIPOnly: boolean;
}

export interface CreateRewardRequest {
  name: string;
  description?: string;
  pointsRequired: number;
  type: RewardType;
  imageUrl?: string;
  quantity: number;
  expiryDate?: string;
  isVIPOnly: boolean;
}

export interface UpdateRewardRequest extends CreateRewardRequest {}

export interface UserReward {
  id: string;
  userId: string;
  rewardId: string;
  claimedAt: string;
  usedAt?: string;
  isUsed: boolean;
  isExpired: boolean;
  rewardName: string;
  rewardDescription?: string;
  rewardType: RewardType;
  pointsSpent: number;
  usageCode?: string;
  deliveryMethod?: string;
  qrCodeValue?: string;
  barcodeValue?: string;
  partnerRedemptionCode?: string;
  partnerName?: string;
  expiryDate?: string;
  satisfactionRating?: number;
  feedbackComment?: string;
  acquisitionChannel?: string;
}

export interface ClaimRewardRequest {
  userId: string;
  rewardId: string;
  deliveryMethod?: string;
  deliveryEmail?: string;
  sourceCampaign?: string;
  acquisitionChannel?: string;
}
