import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatchService } from '../../../services/match.service';
import { Match, MatchStatus } from '../../../models/match';
import { AuthService } from '../../../services/auth.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-matchs',
  imports: [CommonModule, FormsModule],
  templateUrl: './matchs.html',
  styleUrl: './matchs.css',
})
export class Matchs implements OnInit {
  private readonly authService = inject(AuthService);

  matches = signal<Match[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);
  statusFilter = signal<MatchStatus | 'All'>('All');
  deletingId = signal<string | null>(null);

  readonly isAdmin = computed(() => this.authService.hasRole('admin'));
  readonly googleSheetsUrl = environment.googleSheetsMatchesUrl;

  readonly statusOptions: Array<MatchStatus | 'All'> = ['All', 'Scheduled', 'Live', 'Completed', 'Cancelled'];

  // Maps the clean country name → ISO 2-letter code for flagcdn.com
  private readonly flagMap: Record<string, string> = {
    'Mexico': 'mx', 'South Africa': 'za', 'Korea Republic': 'kr', 'Czechia': 'cz',
    'Canada': 'ca', 'Bosnia-Herzegovina': 'ba', 'United States': 'us', 'Paraguay': 'py',
    'Australia': 'au', 'Türkiye': 'tr', 'Qatar': 'qa', 'Switzerland': 'ch',
    'Brazil': 'br', 'Morocco': 'ma', 'Haiti': 'ht', 'Scotland': 'gb-sct',
    'Germany': 'de', 'Curaçao': 'cw', 'Netherlands': 'nl', 'Japan': 'jp',
    "Côte d'Ivoire": 'ci', 'Ecuador': 'ec', 'Sweden': 'se', 'Tunisia': 'tn',
    'Spain': 'es', 'Cabo Verde': 'cv', 'Belgium': 'be', 'Egypt': 'eg',
    'Saudi Arabia': 'sa', 'Uruguay': 'uy', 'IR Iran': 'ir', 'New Zealand': 'nz',
    'Austria': 'at', 'Jordan': 'jo', 'France': 'fr', 'Senegal': 'sn',
    'Iraq': 'iq', 'Norway': 'no', 'Argentina': 'ar', 'Algeria': 'dz',
    'Portugal': 'pt', 'Congo DR': 'cd', 'England': 'gb-eng', 'Croatia': 'hr',
    'Ghana': 'gh', 'Panama': 'pa', 'Uzbekistan': 'uz', 'Colombia': 'co'
  };

  // Strip leading emoji flag (Regional Indicator pairs or subdivision sequences)
  getTeamName(raw: string | undefined): string {
    if (!raw) return 'TBD';
    return raw.replace(/^[\u{1F1E0}-\u{1F1FF}]{2}\s*/u, '')
              .replace(/^\uD83C[\uDFF4]󠁧\uDB40[\uDC62-\uDC73]\uDB40[\uDCA7-\uDCF5]\uDB40[\uDCE6-\uDCF4]󠁿\s*/u, '')
              .trim();
  }

  getFlagUrl(raw: string | undefined): string {
    const name = this.getTeamName(raw);
    const code = this.flagMap[name];
    return code ? `https://flagcdn.com/w40/${code}.png` : '';
  }

  readonly filteredMatches = computed(() => {
    const filter = this.statusFilter();
    if (filter === 'All') return this.matches();
    return this.matches().filter(m => m.status === filter);
  });

  constructor(private matchService: MatchService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.matchService.getAll().subscribe({
      next: (data) => {
        this.matches.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load matches. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  onStatusFilter(status: MatchStatus | 'All'): void {
    this.statusFilter.set(status);
  }

  getStatusClass(status: MatchStatus): string {
    const map: Record<MatchStatus, string> = {
      Scheduled: 'status--scheduled',
      Live: 'status--live',
      Completed: 'status--completed',
      Cancelled: 'status--cancelled'
    };
    return map[status] ?? '';
  }

  getStatusIcon(status: MatchStatus): string {
    const map: Record<MatchStatus, string> = {
      Scheduled: '📅',
      Live: '🔴',
      Completed: '✅',
      Cancelled: '❌'
    };
    return map[status] ?? '';
  }

  deleteMatch(id: string): void {
    if (!confirm('Delete this match? This action cannot be undone.')) return;
    this.deletingId.set(id);
    this.matchService.delete(id).subscribe({
      next: () => {
        this.matches.update(list => list.filter(m => m.id !== id));
        this.deletingId.set(null);
      },
      error: () => {
        this.error.set('Failed to delete match. Please try again.');
        this.deletingId.set(null);
      }
    });
  }
}

