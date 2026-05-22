import { Routes } from '@angular/router';

import { About } from './components/shared/about/about';
import { Swipe } from './components/features/swipe/swipe';
import { Leaderboard } from './components/features/leaderboard/leaderboard';
import { Matchs } from './components/features/matchs/matchs';
import { Predictions } from './components/features/predictions/predictions';
import { Sponsors } from './components/features/sponsors/sponsors';
import { CruiseSquad } from './components/features/cruise-squad/cruise-squad';
import { BoardingPass } from './components/features/boarding-pass/boarding-pass';
import { Login } from './components/shared/auth/login/login';
import { Register } from './components/shared/auth/register/register';
import { authGuard } from './guards/auth-guard';
import { Home } from './components/shared/home/home';
import { Profil } from './components/shared/auth/profil/profil';
import { noAuthGuard } from './guards/no-auth.guard';
import { Dashboard } from './components/shared/dashboard/dashboard';
import { Settings } from './components/shared/auth/settings/settings';
import { roleGuard } from './guards/role.guard';
import { Partners } from './components/features/partners/partners';

// New feature components
import { Missions } from './components/features/missions/missions';
import { Campaigns } from './components/features/campaigns/campaigns';
import { QRCodes } from './components/features/qr-codes/qr-codes';
import { QRScanner } from './components/features/qr-scanner/qr-scanner';
import { Locations } from './components/features/locations/locations';
import { TourismRoutes } from './components/features/tourism-routes/tourism-routes';
import { Rewards } from './components/features/rewards/rewards';
import { MyRewards } from './components/features/my-rewards/my-rewards';
import { WalletComponent } from './components/features/wallet/wallet';
import { Referrals } from './components/features/referrals/referrals';

export const routes: Routes = [

  // Public routes
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: Home },
  { path: 'about', component: About },
  { path: 'leaderboard', component: Leaderboard },
  { path: 'matchs', component: Matchs },
  { path: 'sponsors', component: Sponsors },
  { path: 'locations', component: Locations },
  { path: 'tourism-routes', component: TourismRoutes },
  { path: 'rewards', component: Rewards },

  // Auth
  { path: 'login', component: Login, canActivate: [noAuthGuard] },
  { path: 'register', component: Register, canActivate: [noAuthGuard] },

  // Passenger + Admin features
  { path: 'swipe', component: Swipe, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'passenger'] } },
  { path: 'predictions', component: Predictions, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'passenger'] } },
  { path: 'cruise-squad', component: CruiseSquad, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'passenger'] } },
  { path: 'boarding-pass', component: BoardingPass, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'passenger'] } },
  { path: 'missions', component: Missions, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'passenger'] } },
  { path: 'my-rewards', component: MyRewards, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'passenger'] } },
  { path: 'wallet', component: WalletComponent, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'passenger'] } },
  { path: 'referrals', component: Referrals, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'passenger'] } },

  // Merchant + Admin features (manage QR codes, scan)
  { path: 'qr-codes', component: QRCodes, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'merchant'] } },
  { path: 'qr-scanner', component: QRScanner, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'merchant', 'passenger'] } },

  // Admin + Merchant management
  { path: 'campaigns', component: Campaigns, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'merchant'] } },

  // Admin-only management
  { path: 'partners', component: Partners, canActivate: [authGuard, roleGuard], data: { roles: ['admin'] } },

  // Profile / Settings (all authenticated roles)
  { path: 'profile', component: Profil, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'merchant', 'partner', 'passenger'] } },
  { path: 'settings', component: Settings, canActivate: [authGuard, roleGuard], data: { roles: ['admin', 'merchant', 'partner', 'passenger'] } },

  // Role-specific dashboards
  { path: 'dashboard', component: Dashboard, canActivate: [authGuard] },
  { path: 'admin/dashboard', component: Dashboard, canActivate: [authGuard, roleGuard], data: { roles: ['admin'] } },
  { path: 'merchant/dashboard', component: Dashboard, canActivate: [authGuard, roleGuard], data: { roles: ['merchant'] } },
  { path: 'partner/dashboard', component: Dashboard, canActivate: [authGuard, roleGuard], data: { roles: ['partner'] } },
  { path: 'passenger/dashboard', component: Dashboard, canActivate: [authGuard, roleGuard], data: { roles: ['passenger'] } },

  { path: '**', redirectTo: '/home' }
];
