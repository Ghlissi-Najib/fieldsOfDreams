import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatchService } from '../../../services/match.service';
import { Match } from '../../../models/match';
import { AuthService } from '../../../services/auth.service';
import { ToastService } from '../../../services/toast.service';
import { PredictionService } from '../../../services/prediction.service';
import { OrchestrationService } from '../../../services/orchestration.service';
import { CreatePredictionRequest } from '../../../models/prediction';

interface PredictionForm {
  matchId: string;
  homeTeamScore: number | null;
  awayTeamScore: number | null;
}

@Component({
  selector: 'app-predictions',
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './predictions.html',
  styleUrl: './predictions.css',
})
export class Predictions implements OnInit {
  private readonly predictionService = inject(PredictionService);
  private readonly orchestrationService = inject(OrchestrationService);

  matches = signal<Match[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  predictionForms = signal<Record<string, PredictionForm>>({});
  submitting = signal<Record<string, boolean>>({});

  readonly isPassenger = computed(() => this.authService.hasRole('passenger'));
  readonly isAdmin = computed(() => this.authService.hasRole('admin'));

  readonly predictionsEnabled = computed(() => this.orchestrationService.isFlowEnabled('predictions'));
  readonly activeCampaigns = computed(() => this.orchestrationService.getActiveCampaigns());

  constructor(
    private matchService: MatchService,
    private authService: AuthService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.orchestrationService.load().subscribe();
    this.load();
  }

  load(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.matchService.getAll().subscribe({
      next: (data) => {
        const scheduledOrLive = data.filter(m => m.status === 'Scheduled' || m.status === 'Live');
        this.matches.set(scheduledOrLive);
        const forms: Record<string, PredictionForm> = {};
        scheduledOrLive.forEach(m => {
          forms[m.id] = { matchId: m.id, homeTeamScore: null, awayTeamScore: null };
        });
        this.predictionForms.set(forms);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load matches. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  getPredictionForm(matchId: string): PredictionForm {
    return this.predictionForms()[matchId] ?? { matchId, homeTeamScore: null, awayTeamScore: null };
  }

  updateScore(matchId: string, field: 'homeTeamScore' | 'awayTeamScore', value: string): void {
    const forms = { ...this.predictionForms() };
    forms[matchId] = { ...forms[matchId], [field]: value === '' ? null : Number(value) };
    this.predictionForms.set(forms);
  }

  submitPrediction(match: Match): void {
    if (!this.isPassenger()) return;

    const form = this.getPredictionForm(match.id);
    if (form.homeTeamScore === null || form.awayTeamScore === null) {
      this.toastService.show('Please enter scores for both teams.', 'warning');
      return;
    }

    this.submitting.set({ ...this.submitting(), [match.id]: true });

    const dto: CreatePredictionRequest = {
      matchId: match.id,
      homeTeamScore: form.homeTeamScore,
      awayTeamScore: form.awayTeamScore,
    };

    this.predictionService.create(dto).subscribe({
      next: () => {
        this.toastService.show(`Prediction submitted for ${match.homeTeam} vs ${match.awayTeam}!`, 'success');
        this.submitting.set({ ...this.submitting(), [match.id]: false });
      },
      error: () => {
        this.toastService.show('Failed to submit prediction. Please try again.', 'error');
        this.submitting.set({ ...this.submitting(), [match.id]: false });
      },
    });
  }

  isSubmitting(matchId: string): boolean {
    return !!this.submitting()[matchId];
  }

  getStatusIcon(status: string): string {
    return status === 'Live' ? '🔴' : '📅';
  }
}

