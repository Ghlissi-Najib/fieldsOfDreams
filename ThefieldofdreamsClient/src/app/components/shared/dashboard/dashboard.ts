import { isPlatformBrowser } from '@angular/common';
import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  inject,
  OnDestroy,
  OnInit,
  PLATFORM_ID,
  signal,
} from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { environment } from '../../../environments/environment';
import { RouterModule } from '@angular/router';
import {
  Chart,
  ChartConfiguration,
  ChartData,
  ChartOptions,
  registerables,
} from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { BaseChartDirective } from 'ng2-charts';
import { filter, Subject, take, takeUntil } from 'rxjs';
import {
  AdminDashboardDto,
  MerchantDashboardDto,
  MissionProgressDto,
  PassengerDashboardDto,
  QRCodePerformanceDto,
} from '../../../models/dashboard';
import { AuthService } from '../../../services/auth.service';
import { DashboardService } from '../../../services/dashboard.service';
import { OrchestrationService } from '../../../services/orchestration.service';

// Register Chart.js defaults + datalabels plugin globally once
Chart.register(...registerables, ChartDataLabels);

// ─── Shared chart theming ─────────────────────────────────────────────────────
const TOOLTIP_DEFAULTS = {
  backgroundColor: '#0e0e1c',
  titleColor: '#eeeef5',
  bodyColor: '#8888aa',
  borderColor: '#1f1f35',
  borderWidth: 1,
};

const DATALABELS_OFF = { display: false } as const;

function scaleDefaults() {
  return {
    x: {
      ticks: { color: '#8888aa', font: { size: 11 } },
      grid: { color: '#1f1f35' },
    },
    y: {
      ticks: { color: '#8888aa', font: { size: 11 } },
      grid: { color: '#1f1f35' },
    },
  };
}

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterModule, BaseChartDirective],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit, OnDestroy {
  private readonly authService = inject(AuthService);
  private readonly dashboardService = inject(DashboardService);
  private readonly orchestrationService = inject(OrchestrationService);
  private readonly platformId = inject(PLATFORM_ID);
  private readonly destroy$ = new Subject<void>();

  readonly currentUser = toSignal(this.authService.currentUser$, {
    initialValue: null,
  });

  readonly userName = computed(
    () =>
      this.currentUser()?.firstName ||
      this.currentUser()?.name ||
      'Captain'
  );

  readonly roleLabel = computed(() => {
    const r = this.currentUser()?.role ?? this.currentUser()?.accountType ?? '';
    return r ? r.charAt(0).toUpperCase() + r.slice(1) : 'Guest';
  });

  readonly isAdmin = computed(() => this.currentUser()?.role === 'admin');
  readonly isMerchant = computed(() => this.currentUser()?.role === 'merchant');
  readonly isPartner = computed(() => this.currentUser()?.role === 'partner');
  readonly isPassenger = computed(() => {
    const role = this.currentUser()?.role;
    if (!role) return this.currentUser()?.accountType !== 'business';
    return role === 'passenger';
  });
  readonly canViewAnalytics = computed(() => this.isAdmin() || this.isPartner());

  readonly googleSheetsMatchesUrl = environment.googleSheetsMatchesUrl;
  readonly telegramBotUrl = environment.telegramBotUrl;

  readonly activeCampaigns = computed(() => this.orchestrationService.getActiveCampaigns());
  readonly enabledFlows = computed(() => this.orchestrationService.state()?.enabledFlows ?? {});

  // ─── Loading / error state ────────────────────────────────────────────────
  loading = signal(true);
  error = signal<string | null>(null);

  // ─── Admin data ───────────────────────────────────────────────────────────
  adminData = signal<AdminDashboardDto | null>(null);

  // ─── Passenger data ───────────────────────────────────────────────────────
  passengerData = signal<PassengerDashboardDto | null>(null);

  // ─── Merchant data ────────────────────────────────────────────────────────
  merchantData = signal<MerchantDashboardDto | null>(null);

  // ─── Convenience computed values ─────────────────────────────────────────
  readonly adminSummary = computed(() => this.adminData()?.summary ?? null);
  readonly passengerSummary = computed(
    () => this.passengerData()?.summary ?? null
  );
  readonly merchantSummary = computed(
    () => this.merchantData()?.summary ?? null
  );
  readonly missionProgress = computed<MissionProgressDto[]>(
    () => this.passengerData()?.missionProgress ?? []
  );
  readonly qrPerformance = computed<QRCodePerformanceDto[]>(
    () => this.merchantData()?.qrCodePerformance ?? []
  );
  readonly topLeaderboard = computed(
    () => this.adminData()?.topLeaderboard ?? []
  );
  readonly matchStats = computed(() => this.adminData()?.matchStats ?? null);

  readonly predictionAccuracyPct = computed(() => {
    const acc =
      this.adminData()?.predictionAccuracy ??
      this.passengerData()?.predictionAccuracy;
    if (!acc) return 0;
    const total = acc.correct + acc.wrong;
    return total === 0 ? 0 : Math.round((acc.correct / total) * 100);
  });

  // ─── Chart.js config objects (rebuilt when data arrives) ─────────────────

  // Doughnut – prediction accuracy
  accuracyChartData = signal<ChartData<'doughnut'>>({
    labels: ['Correct', 'Wrong', 'Pending'],
    datasets: [
      {
        data: [0, 0, 0],
        backgroundColor: ['#00d97e', '#ff4560', '#f0b429'],
        borderWidth: 0,
        hoverOffset: 6,
      },
    ],
  });

  readonly accuracyChartOpts: ChartOptions<'doughnut'> = {
    cutout: '68%',
    maintainAspectRatio: false,
    responsive: true,
    plugins: {
      legend: { display: false },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: DATALABELS_OFF,
    },
  };

  // Doughnut – users by role (admin/partner)
  usersByRoleData = signal<ChartData<'doughnut'>>({
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: ['#00cfff', '#00d97e', '#f0b429', '#a78bfa'],
        borderWidth: 0,
        hoverOffset: 6,
      },
    ],
  });

  readonly usersByRoleOpts: ChartOptions<'doughnut'> = {
    cutout: '60%',
    maintainAspectRatio: false,
    responsive: true,
    plugins: {
      legend: {
        position: 'right',
        labels: { color: '#8888aa', font: { size: 11 }, padding: 14 },
      },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: {
        color: '#eeeef5',
        font: { weight: 'bold', size: 12 },
        formatter: (value: number) => (value > 0 ? value : ''),
      },
    },
  };

  // Bar – weekly activity (admin/partner: predictions + QR scans)
  weeklyActivityData = signal<ChartData<'bar'>>({
    labels: [],
    datasets: [],
  });

  readonly weeklyActivityOpts: ChartOptions<'bar'> = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top',
        labels: { color: '#8888aa', font: { size: 11 } },
      },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: DATALABELS_OFF,
    },
    scales: scaleDefaults(),
  };

  // Bar – weekly predictions (passenger)
  weeklyPredsData = signal<ChartData<'bar'>>({
    labels: [],
    datasets: [],
  });

  readonly weeklyPredsOpts: ChartOptions<'bar'> = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: DATALABELS_OFF,
    },
    scales: scaleDefaults(),
  };

  // Line – points trend (passenger)
  pointsTrendData = signal<ChartData<'line'>>({
    labels: [],
    datasets: [],
  });

  readonly pointsTrendOpts: ChartOptions<'line'> = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: DATALABELS_OFF,
    },
    scales: scaleDefaults(),
    elements: {
      line: { tension: 0.45 },
      point: { radius: 4, hoverRadius: 6 },
    },
  };

  // Doughnut – activity breakdown (passenger)
  activityBreakdownData = signal<ChartData<'doughnut'>>({
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#a78bfa',
          '#00cfff',
          '#f0b429',
          '#00d97e',
          '#ff4560',
        ],
        borderWidth: 0,
        hoverOffset: 6,
      },
    ],
  });

  readonly activityBreakdownOpts: ChartOptions<'doughnut'> = {
    cutout: '55%',
    maintainAspectRatio: false,
    responsive: true,
    plugins: {
      legend: {
        position: 'bottom',
        labels: { color: '#8888aa', font: { size: 11 }, padding: 10 },
      },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: {
        color: '#eeeef5',
        font: { weight: 'bold', size: 11 },
        formatter: (value: number) => (value > 0 ? value : ''),
      },
    },
  };

  // Line – wallet trend (admin/partner)
  walletTrendData = signal<ChartData<'line'>>({
    labels: [],
    datasets: [],
  });

  readonly walletTrendOpts: ChartOptions<'line'> = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: DATALABELS_OFF,
    },
    scales: scaleDefaults(),
    elements: {
      line: { tension: 0.45 },
      point: { radius: 4, hoverRadius: 6 },
    },
  };

  // Bar – scans per day (merchant)
  scansByDayData = signal<ChartData<'bar'>>({
    labels: [],
    datasets: [],
  });

  readonly scansByDayOpts: ChartOptions<'bar'> = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top',
        labels: { color: '#8888aa', font: { size: 11 } },
      },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: DATALABELS_OFF,
    },
    scales: scaleDefaults(),
  };

  // Horizontal bar – QR code performance (merchant)
  qrPerfData = signal<ChartData<'bar'>>({
    labels: [],
    datasets: [],
  });

  readonly qrPerfOpts: ChartOptions<'bar'> = {
    indexAxis: 'y',
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: {
        anchor: 'end',
        align: 'right',
        color: '#8888aa',
        font: { size: 10 },
        formatter: (v: number) => v,
      },
    },
    scales: {
      x: { ticks: { color: '#8888aa' }, grid: { color: '#1f1f35' } },
      y: { ticks: { color: '#eeeef5', font: { size: 11 } }, grid: { color: 'transparent' } },
    },
  };

  // Pie – points given by QR code (merchant)
  pointsByCodeData = signal<ChartData<'pie'>>({
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#00cfff',
          '#00d97e',
          '#f0b429',
          '#a78bfa',
          '#ff4560',
          '#ff6b2b',
          '#34d399',
          '#818cf8',
        ],
        borderWidth: 0,
        hoverOffset: 6,
      },
    ],
  });

  readonly pointsByCodeOpts: ChartOptions<'pie'> = {
    maintainAspectRatio: false,
    responsive: true,
    plugins: {
      legend: {
        position: 'right',
        labels: { color: '#8888aa', font: { size: 11 }, padding: 10 },
      },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: {
        color: '#fff',
        font: { weight: 'bold', size: 11 },
        formatter: (value: number, ctx) => {
          const total = (ctx.dataset.data as number[]).reduce(
            (a, b) => a + b,
            0
          );
          return total === 0 ? '' : `${Math.round((value / total) * 100)}%`;
        },
      },
    },
  };

  // Rewards by type (admin – pie)
  rewardsByTypeData = signal<ChartData<'pie'>>({
    labels: [],
    datasets: [
      {
        data: [],
        backgroundColor: [
          '#00d97e',
          '#00cfff',
          '#f0b429',
          '#a78bfa',
          '#ff4560',
        ],
        borderWidth: 0,
        hoverOffset: 6,
      },
    ],
  });

  readonly rewardsByTypeOpts: ChartOptions<'pie'> = {
    maintainAspectRatio: false,
    responsive: true,
    plugins: {
      legend: {
        position: 'right',
        labels: { color: '#8888aa', font: { size: 11 }, padding: 10 },
      },
      tooltip: TOOLTIP_DEFAULTS,
      datalabels: {
        color: '#fff',
        font: { weight: 'bold', size: 11 },
        formatter: (value: number, ctx) => {
          const total = (ctx.dataset.data as number[]).reduce(
            (a, b) => a + b,
            0
          );
          return total === 0 ? '' : `${Math.round((value / total) * 100)}%`;
        },
      },
    },
  };

  // ─── Lifecycle ────────────────────────────────────────────────────────────
  ngOnInit(): void {
    // Skip API calls during SSR — localStorage (and the JWT) only exist in the browser
    if (!isPlatformBrowser(this.platformId)) return;

    this.authService.currentUser$
      .pipe(
        filter((u) => !!u),
        take(1),
        takeUntil(this.destroy$)
      )
      .subscribe((user) => {
        // Load orchestration state in parallel (non-blocking)
        this.orchestrationService.load().subscribe();

        // Fallback: if role is missing from stored user, read it from the raw JWT
        const role = user!.role ?? this.authService.getRoleFromToken() ?? '';
        if (role === 'admin') this.loadAdmin();
        else if (role === 'partner') this.loadPartner();
        else if (role === 'merchant') this.loadMerchant();
        else this.loadPassenger();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // ─── Data loaders ─────────────────────────────────────────────────────────
  private loadAdmin(): void {
    this.loading.set(true);
    this.dashboardService
      .getAdminDashboard()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.adminData.set(data);
          this.buildAdminCharts(data);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Failed to load admin dashboard.');
          this.loading.set(false);
        },
      });
  }

  private loadPartner(): void {
    this.loading.set(true);
    this.dashboardService
      .getPartnerDashboard()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.adminData.set(data);
          this.buildAdminCharts(data);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Failed to load partner dashboard.');
          this.loading.set(false);
        },
      });
  }

  private loadPassenger(): void {
    this.loading.set(true);
    this.dashboardService
      .getPassengerDashboard()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.passengerData.set(data);
          this.buildPassengerCharts(data);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Failed to load passenger dashboard.');
          this.loading.set(false);
        },
      });
  }

  private loadMerchant(): void {
    this.loading.set(true);
    this.dashboardService
      .getMerchantDashboard()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.merchantData.set(data);
          this.buildMerchantCharts(data);
          this.loading.set(false);
        },
        error: () => {
          this.error.set('Failed to load merchant dashboard.');
          this.loading.set(false);
        },
      });
  }

  // ─── Chart builders ───────────────────────────────────────────────────────
  private buildAdminCharts(data: AdminDashboardDto): void {
    // Prediction accuracy doughnut
    const acc = data.predictionAccuracy;
    this.accuracyChartData.set({
      labels: ['Correct', 'Wrong', 'Pending'],
      datasets: [
        {
          data: [acc.correct, acc.wrong, acc.pending],
          backgroundColor: ['#00d97e', '#ff4560', '#f0b429'],
          borderWidth: 0,
          hoverOffset: 6,
        },
      ],
    });

    // Users by role doughnut
    this.usersByRoleData.set({
      labels: data.usersByRole.labels,
      datasets: [
        {
          data: data.usersByRole.values,
          backgroundColor: ['#00cfff', '#00d97e', '#f0b429', '#a78bfa'],
          borderWidth: 0,
          hoverOffset: 6,
        },
      ],
    });

    // Weekly activity bar
    const colors = ['#00d97e', '#00cfff', '#f0b429', '#a78bfa'];
    this.weeklyActivityData.set({
      labels: data.weeklyActivity.labels,
      datasets: data.weeklyActivity.series.map((s, i) => ({
        label: s.name,
        data: s.values,
        backgroundColor: colors[i % colors.length],
        borderRadius: 6,
      })),
    });

    // Wallet trend line
    this.walletTrendData.set({
      labels: data.walletTrend.labels,
      datasets: [
        {
          label: 'Wallet Balance',
          data: data.walletTrend.values,
          borderColor: '#00cfff',
          backgroundColor: 'rgba(0,207,255,0.10)',
          fill: true,
          tension: 0.45,
          pointBackgroundColor: '#00cfff',
          pointBorderColor: '#07070f',
          pointRadius: 4,
          pointHoverRadius: 7,
        },
      ],
    });

    // Rewards by type pie
    this.rewardsByTypeData.set({
      labels: data.rewardsByType.labels,
      datasets: [
        {
          data: data.rewardsByType.values,
          backgroundColor: [
            '#00d97e',
            '#00cfff',
            '#f0b429',
            '#a78bfa',
            '#ff4560',
          ],
          borderWidth: 0,
          hoverOffset: 6,
        },
      ],
    });
  }

  private buildPassengerCharts(data: PassengerDashboardDto): void {
    // Prediction accuracy doughnut
    const acc = data.predictionAccuracy;
    this.accuracyChartData.set({
      labels: ['Correct', 'Wrong', 'Pending'],
      datasets: [
        {
          data: [acc.correct, acc.wrong, acc.pending],
          backgroundColor: ['#00d97e', '#ff4560', '#f0b429'],
          borderWidth: 0,
          hoverOffset: 6,
        },
      ],
    });

    // Weekly predictions bar
    this.weeklyPredsData.set({
      labels: data.weeklyPredictions.labels,
      datasets: [
        {
          label: 'Predictions',
          data: data.weeklyPredictions.values,
          backgroundColor: '#00d97e',
          borderRadius: 6,
        },
      ],
    });

    // Points trend line
    this.pointsTrendData.set({
      labels: data.pointsTrend.labels,
      datasets: [
        {
          label: 'Points',
          data: data.pointsTrend.values,
          borderColor: '#00d97e',
          backgroundColor: 'rgba(0,217,126,0.10)',
          fill: true,
          tension: 0.45,
          pointBackgroundColor: '#00d97e',
          pointBorderColor: '#07070f',
          pointRadius: 4,
          pointHoverRadius: 7,
        },
      ],
    });

    // Activity breakdown doughnut
    this.activityBreakdownData.set({
      labels: data.activityBreakdown.labels,
      datasets: [
        {
          data: data.activityBreakdown.values,
          backgroundColor: [
            '#a78bfa',
            '#00cfff',
            '#f0b429',
            '#00d97e',
            '#ff4560',
          ],
          borderWidth: 0,
          hoverOffset: 6,
        },
      ],
    });
  }

  private buildMerchantCharts(data: MerchantDashboardDto): void {
    // Scans per day grouped bar
    const colors = ['#00cfff', '#00d97e'];
    this.scansByDayData.set({
      labels: data.scansByDay.labels,
      datasets: data.scansByDay.series.map((s, i) => ({
        label: s.name,
        data: s.values,
        backgroundColor: colors[i % colors.length],
        borderRadius: 6,
      })),
    });

    // QR code performance horizontal bar (top 8)
    const perf = data.qrCodePerformance;
    this.qrPerfData.set({
      labels: perf.map((q) => q.title || q.code),
      datasets: [
        {
          label: 'Scans',
          data: perf.map((q) => q.scans),
          backgroundColor: '#00cfff',
          borderRadius: 4,
        },
      ],
    });

    // Points given by code pie
    const byCode = data.pointsGivenByCode;
    this.pointsByCodeData.set({
      labels: byCode.labels,
      datasets: [
        {
          data: byCode.values,
          backgroundColor: [
            '#00cfff',
            '#00d97e',
            '#f0b429',
            '#a78bfa',
            '#ff4560',
            '#ff6b2b',
            '#34d399',
            '#818cf8',
          ],
          borderWidth: 0,
          hoverOffset: 6,
        },
      ],
    });
  }

  // ─── Helpers ──────────────────────────────────────────────────────────────
  missionPct(m: MissionProgressDto): number {
    return m.required > 0
      ? Math.min(100, Math.round((m.currentProgress / m.required) * 100))
      : 0;
  }

  exportData(): void {
    if (typeof document === 'undefined') return;
    const payload =
      this.adminData() ?? this.passengerData() ?? this.merchantData();
    const blob = new Blob([JSON.stringify(payload, null, 2)], {
      type: 'application/json',
    });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `dashboard-${new Date().toISOString()}.json`;
    a.click();
    URL.revokeObjectURL(url);
  }
}
