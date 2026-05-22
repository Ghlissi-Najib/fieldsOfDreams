import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RewardService } from '../../../services/reward.service';
import { UserReward } from '../../../models/reward';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-my-rewards',
  imports: [CommonModule, FormsModule],
  templateUrl: './my-rewards.html',
  styleUrl: './my-rewards.css',
})
export class MyRewards implements OnInit {
  userRewards = signal<UserReward[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  showUsed = signal(false);

  readonly displayedRewards = computed(() => {
    const rewards = this.userRewards();
    return this.showUsed() ? rewards : rewards.filter(r => !r.isUsed && !r.isExpired);
  });

  constructor(
    private rewardService: RewardService,
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
    this.rewardService.getUserRewards(user.id).subscribe({
      next: (data) => {
        this.userRewards.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load your rewards. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  toggleShowUsed(): void {
    this.showUsed.update(v => !v);
  }

  getStatusLabel(reward: UserReward): string {
    if (reward.isExpired) return 'Expired';
    if (reward.isUsed) return 'Used';
    return 'Active';
  }

  getStatusClass(reward: UserReward): string {
    if (reward.isExpired) return 'status--expired';
    if (reward.isUsed) return 'status--used';
    return 'status--active';
  }
}
