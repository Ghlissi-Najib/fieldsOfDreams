import { CampaignType } from './campaign';

export interface ActiveCampaignSummary {
  id: string;
  name: string;
  type: CampaignType;
  endDate: string;
  priority: number;
}

export interface OrchestrationState {
  userId: string;
  userRole: string;
  userPoints: number;
  activeCampaigns: ActiveCampaignSummary[];
  visibleRewardIds: string[];
  validQRCodeIds: string[];
  enabledFlows: Record<string, boolean>;
}
