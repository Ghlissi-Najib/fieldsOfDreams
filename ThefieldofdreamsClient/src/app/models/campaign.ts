export type CampaignType = 'QRBased' | 'PredictionBased' | 'ReferralBased' | 'TourismBased' | 'Hybrid';
export type CampaignStatus = 'Draft' | 'Scheduled' | 'Active' | 'Paused' | 'Completed' | 'Cancelled';
export type CampaignRuleType =
  | 'RequireRole'      // {"role":"passenger"}
  | 'MinimumPoints'    // {"points":100}
  | 'MaxScansPerUser'  // {"max":3}
  | 'TargetUserGroup'  // {"group":"VIP"}
  | 'TimeWindow'       // {"startHour":9,"endHour":18}
  | 'GeoZone'          // {"wkt":"POLYGON((lon lat,...))"}
  | 'PortArrival';     // {"portLocationId":"guid"}

export interface CampaignRule {
  id: string;
  campaignId: string;
  ruleType: CampaignRuleType;
  parametersJson: string;
}

export interface Campaign {
  id: string;
  name: string;
  description?: string;
  sponsorId: string;
  type: CampaignType;
  startDate: string;
  endDate: string;
  budget?: number;
  totalImpressions: number;
  totalClicks: number;
  totalConversions: number;
  status: CampaignStatus;
  priority: number;
}

export interface CreateCampaignRequest {
  name: string;
  description?: string;
  sponsorId: string;
  type: CampaignType;
  startDate: string;
  endDate: string;
  budget?: number;
  status?: CampaignStatus;
  priority?: number;
}

export interface UpdateCampaignRequest {
  name: string;
  description?: string;
  type: CampaignType;
  startDate: string;
  endDate: string;
  budget?: number;
  status: CampaignStatus;
  priority?: number;
}

export interface QRCampaign {
  id: string;
  campaignName: string;
  description?: string;
  campaignId: string;
  qrCodeId: string;
  maxScansPerUser: number;
  totalScanLimit: number;
  currentScanCount: number;
  basePointsReward: number;
  bonusPointsForFirstScan?: number;
  rewardCode?: string;
  discountPercentage?: string;
  startDate?: string;
  endDate?: string;
  isActive: boolean;
  targetUserGroup?: string;
  minimumUserLevel?: number;
  uniqueUsersScanned: number;
  conversionCount: number;
  conversionRate?: number;
  successMessage?: string;
  redirectUrl?: string;
  sponsorOfferCode?: string;
  isSponsorExclusive: boolean;
}

export interface CreateQRCampaignRequest {
  campaignName: string;
  description?: string;
  campaignId: string;
  qrCodeId: string;
  maxScansPerUser: number;
  totalScanLimit: number;
  basePointsReward: number;
  bonusPointsForFirstScan?: number;
  rewardCode?: string;
  discountPercentage?: string;
  startDate?: string;
  endDate?: string;
  isActive: boolean;
  targetUserGroup?: string;
  minimumUserLevel?: number;
  successMessage?: string;
  redirectUrl?: string;
  sponsorOfferCode?: string;
  isSponsorExclusive: boolean;
}

export interface UpdateQRCampaignRequest extends CreateQRCampaignRequest {}
