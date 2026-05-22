import { Component, computed, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { QRCodeService } from '../../../services/qr-code.service';
import { RewardService } from '../../../services/reward.service';
import { OrchestrationService } from '../../../services/orchestration.service';
import { QRScan } from '../../../models/qr-code';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-qr-scanner',
  imports: [CommonModule, FormsModule],
  templateUrl: './qr-scanner.html',
  styleUrl: './qr-scanner.css',
})
export class QRScanner implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly qrCodeService = inject(QRCodeService);
  private readonly rewardService = inject(RewardService);
  private readonly orchestrationService = inject(OrchestrationService);

  // Role guards
  readonly isMerchant = computed(() => this.authService.hasRole('merchant'));
  readonly isAdmin    = computed(() => this.authService.hasRole('admin'));
  readonly isPassenger = computed(() => this.authService.hasRole('passenger'));
  readonly canValidatePerks = computed(() => this.isMerchant() || this.isAdmin());

  // Orchestration
  readonly qrScanEnabled        = computed(() => this.orchestrationService.isFlowEnabled('qrScanning'));
  readonly perkValidationEnabled = computed(() => this.orchestrationService.isFlowEnabled('perkValidation'));
  readonly activeCampaigns      = computed(() => this.orchestrationService.getActiveCampaigns());

  // Sandbox mode: passenger, no live campaign
  readonly isSandboxMode = computed(() => this.isPassenger() && !this.qrScanEnabled());

  // Passenger — live scan
  qrCodeId  = signal('');
  lastScan  = signal<QRScan | null>(null);
  isScanning = signal(false);

  // Passenger — sandbox
  sandboxCode   = signal('TEST2026');
  sandboxResult = signal<{ success: boolean; message: string } | null>(null);

  // Merchant/Admin — validate promo code
  promoCode      = signal('');
  isValidating   = signal(false);
  validateResult = signal<{ success: boolean; message: string } | null>(null);

  error   = signal<string | null>(null);
  success = signal<string | null>(null);

  ngOnInit(): void {
    this.orchestrationService.load().subscribe();
  }

  // ── Live scan ────────────────────────────────────────────────────────────
  onScanQR(): void {
    const id = this.qrCodeId().trim();
    if (!id) return;

    this.isScanning.set(true);
    this.error.set(null);
    this.success.set(null);

    this.qrCodeService.scan({ qrCodeId: id }).subscribe({
      next: (scan) => {
        this.lastScan.set(scan);
        this.success.set(`QR code scanned! Points awarded: ${scan.pointsAwarded}`);
        this.qrCodeId.set('');
        this.isScanning.set(false);
      },
      error: () => {
        this.error.set('Failed to scan QR code. Please check the ID and try again.');
        this.isScanning.set(false);
      },
    });
  }

  // ── Sandbox demo scan (no API, no points) ────────────────────────────────
  onSandboxScan(): void {
    const code = this.sandboxCode().trim().toUpperCase();
    if (!code) return;

    if (code === 'TEST2026') {
      this.sandboxResult.set({
        success: true,
        message: 'Demo scan successful! In a live campaign this QR would award you 50 pts — no points in sandbox mode.'
      });
    } else {
      this.sandboxResult.set({
        success: false,
        message: `Unknown demo code "${this.sandboxCode().trim()}". Try the suggested code: TEST2026`
      });
    }
  }

  clearSandbox(): void {
    this.sandboxCode.set('TEST2026');
    this.sandboxResult.set(null);
  }

  // ── Merchant perk validation ──────────────────────────────────────────────
  onValidatePerk(): void {
    const code = this.promoCode().trim();
    if (!code) return;

    this.isValidating.set(true);
    this.error.set(null);
    this.validateResult.set(null);

    this.rewardService.validatePromoCode(code).subscribe({
      next: (result) => {
        this.validateResult.set(result);
        this.promoCode.set('');
        this.isValidating.set(false);
      },
      error: () => {
        this.error.set('Failed to validate promo code. Check the code and try again.');
        this.isValidating.set(false);
      },
    });
  }
}
