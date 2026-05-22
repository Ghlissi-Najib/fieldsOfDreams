import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReferralService } from '../../../services/referral.service';
import { Referral } from '../../../models/referral';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-referrals',
  imports: [CommonModule, FormsModule],
  templateUrl: './referrals.html',
  styleUrl: './referrals.css',
})
export class Referrals implements OnInit {
  referrals = signal<Referral[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  referredEmail = signal('');
  isSending = signal(false);
  successMessage = signal<string | null>(null);

  constructor(
    private referralService: ReferralService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    const user = this.authService.getCurrentUser();
    if (!user) return;

    this.isLoading.set(true);
    this.error.set(null);
    this.referralService.getByReferrer(user.id).subscribe({
      next: (data) => {
        this.referrals.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load referrals. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  get completedCount(): number {
    return this.referrals().filter(r => r.isCompleted).length;
  }

  get totalPoints(): number {
    return this.referrals().filter(r => r.isCompleted).reduce((sum, r) => sum + r.pointsAwarded, 0);
  }
}
