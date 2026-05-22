export interface Sponsor {
  id: string;
  name: string;
  logoUrl?: string;
  websiteUrl?: string;
  description?: string;
  tier?: string;
  isActive: boolean;
  createdAt: string;
}
