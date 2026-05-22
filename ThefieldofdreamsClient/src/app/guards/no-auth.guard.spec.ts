import { TestBed } from '@angular/core/testing';
import { CanActivateFn, Router } from '@angular/router';
import { vi } from 'vitest';

import { noAuthGuard } from './no-auth.guard';
import { AuthService } from '../services/auth.service';

describe('noAuthGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => noAuthGuard(...guardParameters));

  const navigate = vi.fn();
  const authServiceMock = {
    isLoggedIn: vi.fn(() => false)
  };

  beforeEach(() => {
    navigate.mockReset();
    authServiceMock.isLoggedIn.mockReset();
    authServiceMock.isLoggedIn.mockReturnValue(false);

    TestBed.configureTestingModule({
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: { navigate } }
      ]
    });
  });

  it('should allow navigation when user is not authenticated', () => {
    const result = executeGuard({} as any, {} as any);
    expect(result).toBe(true);
    expect(navigate).not.toHaveBeenCalled();
  });

  it('should redirect authenticated users to dashboard', () => {
    authServiceMock.isLoggedIn.mockReturnValue(true);

    const result = executeGuard({} as any, {} as any);

    expect(result).toBe(false);
    expect(navigate).toHaveBeenCalledWith(['/dashboard']);
  });
});
