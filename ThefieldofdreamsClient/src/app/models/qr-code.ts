export type QRCodeType = 'Tourism' | 'Sponsor' | 'Mission' | 'Reward' | 'Checkpoint' | 'HiddenGem';

export interface QRCode {
  id: string;
  code: string;
  title: string;
  description?: string;
  type: QRCodeType;
  sponsorId?: string;
  locationId?: string;
  pointsReward: number;
  maxScans: number;
  currentScanCount: number;
  expiryDate?: string;
  isLimitedTime: boolean;
}

export interface CreateQRCodeRequest {
  code: string;
  title: string;
  description?: string;
  type: QRCodeType;
  sponsorId?: string;
  locationId?: string;
  pointsReward: number;
  maxScans: number;
  expiryDate?: string;
  isLimitedTime: boolean;
}

export interface UpdateQRCodeRequest {
  title: string;
  description?: string;
  type: QRCodeType;
  sponsorId?: string;
  locationId?: string;
  pointsReward: number;
  maxScans: number;
  expiryDate?: string;
  isLimitedTime: boolean;
}

export interface QRScan {
  id: string;
  userId: string;
  qrCodeId: string;
  pointsAwarded: number;
  locationData?: string;
  deviceInfo?: string;
  scannedAt: string;
}

export interface CreateQRScanRequest {
  qrCodeId: string;
  locationData?: string;
  deviceInfo?: string;
}
