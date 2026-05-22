import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { forkJoin, of } from 'rxjs';
import { MissionService } from '../../../services/mission.service';
import { Mission, UserMission } from '../../../models/mission';
import { AuthService } from '../../../services/auth.service';
import { ToastService } from '../../../services/toast.service';

export interface EnrichedMission extends Mission {
  userMission: UserMission | null;
}

@Component({
  selector: 'app-missions',
  imports: [CommonModule, FormsModule],
  templateUrl: './missions.html',
  styleUrl: './missions.css',
})
export class Missions implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly toastService = inject(ToastService);

  missions = signal<Mission[]>([]);
  userMissions = signal<UserMission[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  starting = signal<Record<string, boolean>>({});
  typeFilter = signal<'All' | 'Daily' | 'Other'>('All');

  readonly typeOptions: Array<'All' | 'Daily' | 'Other'> = ['All', 'Daily', 'Other'];

  readonly isPassenger = computed(() => this.authService.hasRole('passenger'));

  readonly enrichedMissions = computed<EnrichedMission[]>(() => {
    const ums = this.userMissions();
    return this.missions().map(m => ({
      ...m,
      userMission: ums.find(um => um.missionId === m.id) ?? null,
    }));
  });

  readonly filteredMissions = computed(() => {
    const filter = this.typeFilter();
    const all = this.enrichedMissions();
    if (filter === 'All') return all;
    if (filter === 'Daily') return all.filter(m => m.isDaily);
    return all.filter(m => !m.isDaily);
  });

  constructor(private missionService: MissionService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading.set(true);
    this.error.set(null);

    const user = this.authService.getCurrentUser();
    const userMissions$ = user
      ? this.missionService.getUserMissions(user.id)
      : of([] as UserMission[]);

    forkJoin({ missions: this.missionService.getAll(), userMissions: userMissions$ }).subscribe({
      next: ({ missions, userMissions }) => {
        this.missions.set(missions);
        this.userMissions.set(userMissions);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load missions. Please try again.');
        this.isLoading.set(false);
      },
    });
  }

  onTypeFilter(type: 'All' | 'Daily' | 'Other'): void {
    this.typeFilter.set(type);
  }

  startMission(mission: EnrichedMission): void {
    const user = this.authService.getCurrentUser();
    if (!user) return;

    this.starting.set({ ...this.starting(), [mission.id]: true });

    this.missionService.assignMission(user.id, mission.id).subscribe({
      next: (um) => {
        this.userMissions.update(list => [...list, um]);
        this.starting.set({ ...this.starting(), [mission.id]: false });
        this.toastService.show(`Mission "${mission.title}" started!`, 'success');
      },
      error: () => {
        this.starting.set({ ...this.starting(), [mission.id]: false });
        this.toastService.show('Could not start mission. Try again.', 'error');
      },
    });
  }

  progressPct(m: EnrichedMission): number {
    if (!m.userMission) return 0;
    return Math.min(100, Math.round((m.userMission.currentProgress / m.requiredProgress) * 100));
  }

  getDifficultyClass(difficulty: string): string {
    const map: Record<string, string> = {
      Easy: 'difficulty--easy',
      Medium: 'difficulty--medium',
      Hard: 'difficulty--hard',
      Expert: 'difficulty--expert',
    };
    return map[difficulty] ?? '';
  }
}
