import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { UserRole } from '../models/rbac';

interface RoleRouteData {
  roles?: UserRole[];
}

export const roleGuard: CanActivateFn = (route) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const allowedRoles = ((route.data ?? {}) as RoleRouteData).roles ?? [];
  if (allowedRoles.length === 0) {
    return true;
  }

  if (authService.hasAnyRole(allowedRoles)) {
    return true;
  }

  router.navigate(['/dashboard']);
  return false;
};
