import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { vi } from 'vitest';

import { Navbar } from './navbar';
import { AuthService } from '../../../services/auth.service';

describe('Navbar', () => {
  let component: Navbar;
  let fixture: ComponentFixture<Navbar>;
  const currentUser$ = new BehaviorSubject<any>(null);

  beforeEach(async () => {
    currentUser$.next(null);

    await TestBed.configureTestingModule({
      imports: [Navbar],
      providers: [
        provideRouter([]),
        {
          provide: AuthService,
          useValue: {
            currentUser$,
            getCurrentUser: () => currentUser$.value,
            logout: vi.fn(),
            hasAnyRole: vi.fn().mockReturnValue(true),
            hasPermission: vi.fn().mockReturnValue(true),
          }
        }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Navbar);
    component = fixture.componentInstance;
    await fixture.whenStable();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show register and sign in when logged out', async () => {
    currentUser$.next(null);
    fixture.detectChanges();
    await fixture.whenStable();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.fod-btn-register')?.textContent).toContain('Register');
    expect(compiled.querySelector('.fod-btn-signin')?.textContent).toContain('Sign In');
  });

  it('should hide register and sign in when logged in', async () => {
    fixture.destroy();
    currentUser$.next({
      firstName: 'Jane',
      lastName: 'Doe',
      name: 'Jane Doe',
      email: 'jane@example.com',
      tickets: 12,
      points: 20
    });

    fixture = TestBed.createComponent(Navbar);
    component = fixture.componentInstance;
    await fixture.whenStable();
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.fod-btn-register')).toBeNull();
    expect(compiled.querySelector('.fod-btn-signin')).toBeNull();
    expect(compiled.querySelector('.fod-hamburger-btn')).not.toBeNull();
  });
});
