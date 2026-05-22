import { UserRole } from './rbac';

export interface User {
  id: string;
  name: string;
  firstName?: string;
  lastName?: string;
  email: string;
  phone?: string;
  avatar?: string;
  profilePhotoUrl?: string;
  accountType: 'player' | 'business';
  role?: UserRole;
  merchantId?: string;
  partnerId?: string;
  tickets: number;
  points: number;
  totalPoints?: number;
  currentLevel?: number;
  experiencePoints?: number;
  rank?: number;
  verified: boolean;
  isActive?: boolean;
  emailConfirmed?: boolean;
  createdAt: Date | string;
  updatedAt?: Date | string;
  lastLoginAt?: Date | string;
  referralCode?: string;
  referralCount?: number;
  referredByUserId?: string;
  walletId?: string;
  refreshToken?: string;
  refreshTokenExpiryTime?: Date | string;
}

export interface AuthResponse {
  accessToken?: string;
  refreshToken?: string;
  expiresAt?: string;
  userId?: string;
  email?: string;
  firstName?: string;
  lastName?: string;
  profilePhotoUrl?: string;
  role?: string;
  merchantId?: string;
  partnerId?: string;
  roles?: string[];
  
  // Frontend-only helper fields (populated by service)
  token?: string;
  success?: boolean;
  user?: User;
  message?: string;
  requiresVerification?: boolean;
  otpSent?: boolean;
}

export interface LoginCredentials {
  email: string;
  password: string;
  rememberMe?: boolean;
}

export interface RegisterData {
  firstName: string;
  lastName: string;
  name: string;
  email: string;
  phone?: string;
  password: string;
  accountType: 'player' | 'business';
  gdprConsent: boolean;
  referralCode?: string;
  role?: string;
}

export interface UpdateProfileRequest {
  firstName?: string;
  lastName?: string;
  name?: string;
  email?: string;
  phone?: string;
  avatar?: string;
  profilePhotoUrl?: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export interface AccountProfile {
  id: string;
  name: string;
  firstName?: string;
  lastName?: string;
  email: string;
  /** Backend DTO uses phoneNumber; `phone` is kept as an alias for legacy compatibility. */
  phone?: string;
  phoneNumber?: string;
  avatar?: string;
  profilePhotoUrl?: string;
  accountType: 'player' | 'business';
  verified: boolean;
  isActive?: boolean;
  createdAt: Date | string;
  updatedAt?: Date | string;
  tickets?: number;
  points?: number;
  totalPoints?: number;
  currentLevel?: number;
  roles?: string[];
}
