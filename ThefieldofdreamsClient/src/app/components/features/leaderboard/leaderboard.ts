import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LeaderboardService } from '../../../services/leaderboard.service';
import { LeaderboardEntry } from '../../../models/leaderboard';

@Component({
  selector: 'app-leaderboard',
  imports: [CommonModule, FormsModule],
  templateUrl: './leaderboard.html',
  styleUrl: './leaderboard.css',
})
export class Leaderboard implements OnInit {
  entries = signal<LeaderboardEntry[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  selectedType = signal<string>('Global');
  limit = signal(50);

  readonly types = ['Global', 'Weekly', 'Monthly'];

  constructor(private leaderboardService: LeaderboardService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.leaderboardService.getTop(this.selectedType(), this.limit()).subscribe({
      next: (data) => {
        this.entries.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load leaderboard. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  onTypeChange(type: string): void {
    this.selectedType.set(type);
    this.load();
  }

  getRankIcon(rank: number): string {
    if (rank === 1) return '🥇';
    if (rank === 2) return '🥈';
    if (rank === 3) return '🥉';
    return `#${rank}`;
  }
}

