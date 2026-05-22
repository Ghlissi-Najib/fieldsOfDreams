import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RewardService } from '../../../services/reward.service';
import { OrchestrationService } from '../../../services/orchestration.service';
import { Reward, RewardType, ClaimRewardRequest } from '../../../models/reward';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-rewards',
  imports: [CommonModule, FormsModule],
  templateUrl: './rewards.html',
  styleUrl: './rewards.css',
})
export class Rewards implements OnInit {
  private readonly orchestrationService = inject(OrchestrationService);

  rewards = signal<Reward[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  claimingId = signal<string | null>(null);
  successMessage = signal<string | null>(null);
  typeFilter = signal<RewardType | 'All'>('All');

  readonly typeOptions: Array<RewardType | 'All'> = ['All', 'DiscountCoupon', 'FreeDrink', 'Merchandise', 'TicketUpgrade', 'VIPAccess', 'DigitalBadge', 'Cashback'];

  readonly isPassenger = computed(() => this.authService.hasRole('passenger'));

  readonly rewardFlowEnabled = computed(() => this.orchestrationService.isFlowEnabled('rewardClaim'));

  readonly activeCampaigns = computed(() => this.orchestrationService.getActiveCampaigns());

  readonly filteredRewards = computed(() => {
    const filter = this.typeFilter();
    const all = filter === 'All' ? this.rewards() : this.rewards().filter(r => r.type === filter);
    // If orchestration has loaded a visibility list, respect it
    const ids = this.orchestrationService.state()?.visibleRewardIds;
    if (!ids || ids.length === 0) return all;
    return all.filter(r => ids.includes(r.id));
  });

  constructor(
    private rewardService: RewardService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.orchestrationService.load().subscribe();
    this.load();
  }

  load(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.rewardService.getAll().subscribe({
      next: (data) => {
        this.rewards.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load rewards. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  onTypeFilter(type: RewardType | 'All'): void {
    this.typeFilter.set(type);
  }

  claimReward(reward: Reward): void {
    const user = this.authService.getCurrentUser();
    if (!user) return;

    this.claimingId.set(reward.id);
    this.error.set(null);
    this.successMessage.set(null);

    const dto: ClaimRewardRequest = { userId: user.id, rewardId: reward.id };
    this.rewardService.claimReward(dto).subscribe({
      next: () => {
        this.successMessage.set(`Successfully claimed: ${reward.name}`);
        this.claimingId.set(null);
        this.load();
      },
      error: () => {
        this.error.set('Failed to claim reward. Check your points balance.');
        this.claimingId.set(null);
      }
    });
  }

  isAvailable(reward: Reward): boolean {
    return reward.quantity > reward.claimedCount && (!reward.expiryDate || new Date(reward.expiryDate) > new Date());
  }
}
