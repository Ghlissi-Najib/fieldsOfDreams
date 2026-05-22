import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  // Public routes — safe to prerender (no auth required)
  { path: 'home', renderMode: RenderMode.Prerender },
  { path: 'about', renderMode: RenderMode.Prerender },
  { path: 'leaderboard', renderMode: RenderMode.Prerender },
  { path: 'matchs', renderMode: RenderMode.Prerender },
  { path: 'sponsors', renderMode: RenderMode.Prerender },
  { path: 'locations', renderMode: RenderMode.Prerender },
  { path: 'tourism-routes', renderMode: RenderMode.Prerender },
  { path: 'rewards', renderMode: RenderMode.Prerender },
  { path: 'login', renderMode: RenderMode.Prerender },
  { path: 'register', renderMode: RenderMode.Prerender },

  // Auth-protected routes — client-side only (SSR has no localStorage/token)
  { path: 'dashboard', renderMode: RenderMode.Client },
  { path: 'admin/dashboard', renderMode: RenderMode.Client },
  { path: 'merchant/dashboard', renderMode: RenderMode.Client },
  { path: 'partner/dashboard', renderMode: RenderMode.Client },
  { path: 'passenger/dashboard', renderMode: RenderMode.Client },
  { path: 'profile', renderMode: RenderMode.Client },
  { path: 'settings', renderMode: RenderMode.Client },
  { path: 'swipe', renderMode: RenderMode.Client },
  { path: 'predictions', renderMode: RenderMode.Client },
  { path: 'cruise-squad', renderMode: RenderMode.Client },
  { path: 'boarding-pass', renderMode: RenderMode.Client },
  { path: 'missions', renderMode: RenderMode.Client },
  { path: 'my-rewards', renderMode: RenderMode.Client },
  { path: 'wallet', renderMode: RenderMode.Client },
  { path: 'referrals', renderMode: RenderMode.Client },
  { path: 'qr-codes', renderMode: RenderMode.Client },
  { path: 'qr-scanner', renderMode: RenderMode.Client },
  { path: 'campaigns', renderMode: RenderMode.Client },
  { path: 'partners', renderMode: RenderMode.Client },

  // Fallback
  { path: '**', renderMode: RenderMode.Prerender },
];
