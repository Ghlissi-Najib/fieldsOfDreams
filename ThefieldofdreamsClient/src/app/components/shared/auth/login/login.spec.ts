import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter, Router } from '@angular/router';
import { of } from 'rxjs';
import { vi } from 'vitest';

import { Login } from './login';
import { AuthService } from '../../../../services/auth.service';
import { ToastService } from '../../../../services/toast.service';

describe('Login', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let authServiceMock: any;
  let router: Router;

  beforeEach(async () => {
    authServiceMock = {
      isLoggedIn: vi.fn(() => false),
      getRedirectUrl: vi.fn(() => null),
      login: vi.fn(() => of({ accessToken: 'token' })),
      register: vi.fn(() => of({ requiresVerification: true })),
      requestPasswordReset: vi.fn(() => of({})),
      googleLogin: vi.fn(() => of({})),
      whatsappLogin: vi.fn(() => of({ otpSent: false })),
      verifyWhatsAppOtp: vi.fn(() => of({}))
    };

    await TestBed.configureTestingModule({
      imports: [Login],
      providers: [
        provideRouter([]),
        { provide: AuthService, useValue: authServiceMock },
        { provide: ToastService, useValue: { show: vi.fn() } }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    router = TestBed.inject(Router);
    vi.spyOn(router, 'navigate').mockResolvedValue(true);
    fixture.detectChanges();
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should redirect to dashboard after successful login', async () => {
    component.loginForm.patchValue({ email: 'test@example.com', password: 'secret123', rememberMe: true });

    component.onLogin();
    await fixture.whenStable();

    expect(router.navigate).toHaveBeenCalledWith(['/dashboard']);
  });
});
