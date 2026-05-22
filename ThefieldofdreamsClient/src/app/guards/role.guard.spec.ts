import { TestBed } from '@angular/core/testing';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { vi } from 'vitest';

import { roleGuard } from './role.guard';
import { AuthService } from '../services/auth.service';

describe('roleGuard', () => {
  const executeGuard: CanActivateFn = (route, state) => TestBed.runInInjectionContext(() => roleGuard(route, state));

  it('allows access when user has one of the allowed roles', () => {
    const navigate = vi.fn();
    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: { hasAnyRole: vi.fn().mockReturnValue(true) } },
        { provide: Router, useValue: { navigate } },
      ],
    });

    const route = { data: { roles: ['admin'] } } as unknown as ActivatedRouteSnapshot;
    const result = executeGuard(route, {} as RouterStateSnapshot);

    expect(result).toBe(true);
    expect(navigate).not.toHaveBeenCalled();
  });

  it('redirects to dashboard when user does not have required role', () => {
    const navigate = vi.fn();
    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: { hasAnyRole: vi.fn().mockReturnValue(false) } },
        { provide: Router, useValue: { navigate } },
      ],
    });

    const route = { data: { roles: ['admin'] } } as unknown as ActivatedRouteSnapshot;
    const result = executeGuard(route, {} as RouterStateSnapshot);

    expect(result).toBe(false);
    expect(navigate).toHaveBeenCalledWith(['/dashboard']);
  });
});
