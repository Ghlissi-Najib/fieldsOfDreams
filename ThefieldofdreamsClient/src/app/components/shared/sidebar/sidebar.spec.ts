import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Component } from '@angular/core';
import { provideRouter } from '@angular/router';
import { By } from '@angular/platform-browser';
import { BehaviorSubject } from 'rxjs';
import { vi } from 'vitest';

import { Sidebar } from './sidebar';
import { AuthService } from '../../../services/auth.service';

@Component({ template: '' })
class DummyLoginComponent {}

describe('Sidebar', () => {
  let component: Sidebar;
  let fixture: ComponentFixture<Sidebar>;
  const currentUser$ = new BehaviorSubject<any>({
    id: '1',
    name: 'Jane Doe',
    firstName: 'Jane',
    lastName: 'Doe',
    email: 'jane@demo.com',
    accountType: 'player',
    tickets: 4,
    points: 12,
    verified: true,
    createdAt: new Date(),
    updatedAt: new Date()
  });
  const authServiceMock = {
    currentUser$,
    logout: vi.fn(),
    hasAnyRole: vi.fn().mockReturnValue(true)
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Sidebar],
      providers: [
        provideRouter([{ path: 'login', component: DummyLoginComponent }]),
        {
          provide: AuthService,
          useValue: authServiceMock
        }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Sidebar);
    component = fixture.componentInstance;
    fixture.detectChanges();
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show user actions when logged in', () => {
    const labels = fixture.debugElement
      .queryAll(By.css('.right-panel__link span'))
      .map((el) => el.nativeElement.textContent.trim());

    expect(labels).toContain('Profile');
    expect(labels).toContain('Settings');
    expect(labels).toContain('Logout');
  });

  it('should call logout action', () => {
    const logoutButton = fixture.debugElement.query(By.css('.right-panel__link--logout'));
    logoutButton.nativeElement.click();

    expect(authServiceMock.logout).toHaveBeenCalled();
  });

  it('should apply hidden class when visibility input is false', () => {
    fixture.destroy();
    fixture = TestBed.createComponent(Sidebar);
    component = fixture.componentInstance;
    component.isVisible = false;
    fixture.detectChanges();

    const panel = fixture.debugElement.query(By.css('.right-panel'));
    expect(panel.nativeElement.classList).toContain('right-panel--hidden');
  });
});
