import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CampaignService } from '../../../services/campaign.service';
import { Campaign, CampaignStatus } from '../../../models/campaign';

@Component({
  selector: 'app-campaigns',
  imports: [CommonModule, FormsModule],
  templateUrl: './campaigns.html',
  styleUrl: './campaigns.css',
})
export class Campaigns implements OnInit {
  campaigns = signal<Campaign[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  statusFilter = signal<CampaignStatus | 'All'>('All');

  readonly statusOptions: Array<CampaignStatus | 'All'> = ['All', 'Draft', 'Scheduled', 'Active', 'Paused', 'Completed', 'Cancelled'];

  readonly filteredCampaigns = computed(() => {
    const filter = this.statusFilter();
    if (filter === 'All') return this.campaigns();
    return this.campaigns().filter(c => c.status === filter);
  });

  constructor(private campaignService: CampaignService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.campaignService.getAll().subscribe({
      next: (data) => {
        this.campaigns.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load campaigns. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  onStatusFilter(status: CampaignStatus | 'All'): void {
    this.statusFilter.set(status);
  }

  getStatusClass(status: CampaignStatus): string {
    const map: Record<CampaignStatus, string> = {
      Draft: 'status--draft',
      Scheduled: 'status--scheduled',
      Active: 'status--active',
      Paused: 'status--paused',
      Completed: 'status--completed',
      Cancelled: 'status--cancelled',
    };
    return map[status] ?? '';
  }
}
