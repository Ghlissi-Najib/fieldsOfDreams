import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { User } from '../../../models/user';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar implements OnInit, OnDestroy {
  isMenuOpen = false;
  isUserMenuOpen = false;
  currentUser: User | null = null;
  private destroy$ = new Subject<void>();

  constructor(
    private authService: AuthService,
    private router: Router,
  ) {
    this.currentUser = this.authService.getCurrentUser();
  }

  ngOnInit(): void {
    this.authService.currentUser$.pipe(takeUntil(this.destroy$)).subscribe(user => {
      this.currentUser = user;
      if (!user) {
        this.isUserMenuOpen = false;
        this.isMenuOpen = false;
      }
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // ─── Role helpers ────────────────────────────────────────────
  isAdmin(): boolean    { return this.authService.hasRole('admin'); }
  isMerchant(): boolean { return this.authService.hasRole('merchant'); }
  isPartner(): boolean  { return this.authService.hasRole('partner'); }
  isPassenger(): boolean { return this.authService.hasRole('passenger'); }

  canViewAnalytics(): boolean {
    return this.authService.hasPermission('viewAggregateAnalytics')
        || this.authService.hasPermission('viewFullAnalytics');
  }

  // ─── Display helpers ─────────────────────────────────────────
  get userInitials(): string {
    const user = this.currentUser;
    if (!user) return '?';
    if (user.firstName && user.lastName) return (user.firstName[0] + user.lastName[0]).toUpperCase();
    if (user.firstName) return user.firstName[0].toUpperCase();
    if (user.name) return user.name.slice(0, 2).toUpperCase();
    return '?';
  }

  get roleLabel(): string {
    const role = this.currentUser?.role;
    if (!role) return '';
    return role.charAt(0).toUpperCase() + role.slice(1);
  }

  // ─── Menu controls ───────────────────────────────────────────
  toggleMenu(): void    { this.isMenuOpen = !this.isMenuOpen; }
  closeMenu(): void     { this.isMenuOpen = false; }
  toggleUserMenu(): void { this.isUserMenuOpen = !this.isUserMenuOpen; }
  closeUserMenu(): void  { this.isUserMenuOpen = false; }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.fod-user-menu')) {
      this.isUserMenuOpen = false;
    }
  }

  // ─── Actions ────────────────────────────────────────────────
  logout(): void {
    this.authService.logout();
    this.closeUserMenu();
    this.closeMenu();
    this.router.navigate(['/login']);
  }
}
