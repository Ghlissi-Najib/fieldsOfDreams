export type UserRole = 'admin' | 'merchant' | 'partner' | 'passenger';

export type Permission =
  | 'viewAllBookings'
  | 'viewOwnBookings'
  | 'postLiveSlots'
  | 'viewAggregateAnalytics'
  | 'viewFullAnalytics'
  | 'playGames'
  | 'validatePerks'
  | 'manageUsers'
  | 'managePartners'
  | 'manageRewards'
  | 'accessGoogleSheetsDashboard'
  | 'controlMakeScenarios'
  | 'selectWinners'
  | 'viewOwnTicketsAndRewards'
  | 'nfcCheckIn'
  | 'enterGiveaway';

export const ROLE_PERMISSIONS: Record<UserRole, Permission[]> = {
  admin: [
    'viewAllBookings',
    'viewOwnBookings',
    'postLiveSlots',
    'viewAggregateAnalytics',
    'viewFullAnalytics',
    'validatePerks',
    'manageUsers',
    'managePartners',
    'manageRewards',
    'accessGoogleSheetsDashboard',
    'controlMakeScenarios',
    'selectWinners',
    'viewOwnTicketsAndRewards',
  ],
  merchant: ['viewOwnBookings', 'postLiveSlots', 'validatePerks'],
  partner: ['viewAggregateAnalytics'],
  passenger: ['playGames', 'viewOwnTicketsAndRewards', 'nfcCheckIn', 'enterGiveaway'],
};
