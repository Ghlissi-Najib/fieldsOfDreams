import { isPlatformBrowser } from '@angular/common';
import { inject, PLATFORM_ID } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  // Server has no localStorage — let the client handle auth entirely
  if (!isPlatformBrowser(inject(PLATFORM_ID))) return true;

  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true;
  }

  authService.setRedirectUrl(state.url);
  router.navigate(['/login']);
  return false;
};
