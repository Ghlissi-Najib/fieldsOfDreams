import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { Subscription } from 'rxjs';
import { User } from '../../../models/user';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css'
})
export class Sidebar implements OnInit, OnDestroy {
  @Input() isVisible = true;
  isLoggedIn = false;
  user: User | null = null;
  private userSubscription: Subscription | null = null;
  isExpanded = true;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.userSubscription = this.authService.currentUser$.subscribe(user => {
      this.user = user;
      this.isLoggedIn = !!user;
    });
  }

  ngOnDestroy(): void {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }

  togglePanel(): void {
    this.isExpanded = !this.isExpanded;
  }

  closePanel(): void {
    // Optional: close panel on mobile after navigation
    if (window.innerWidth <= 991) {
      this.isVisible = false;
    }
  }

  getInitials(): string {
    if (!this.user) return '?';
    
    if (this.user.firstName && this.user.lastName) {
      return (this.user.firstName[0] + this.user.lastName[0]).toUpperCase();
    }
    
    if (this.user.name) {
      const parts = this.user.name.split(' ');
      if (parts.length >= 2) {
        return (parts[0][0] + parts[1][0]).toUpperCase();
      }
      return this.user.name[0].toUpperCase();
    }
    
    return this.user.email[0].toUpperCase();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  canAccessAccountManagement(): boolean {
    return this.authService.hasAnyRole(['admin', 'merchant', 'partner']);
  }
}
