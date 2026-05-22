import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { QRCodeService } from '../../../services/qr-code.service';
import { QRCode } from '../../../models/qr-code';

@Component({
  selector: 'app-qr-codes',
  imports: [CommonModule, FormsModule],
  templateUrl: './qr-codes.html',
  styleUrl: './qr-codes.css',
})
export class QRCodes implements OnInit {
  qrCodes = signal<QRCode[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);

  constructor(private qrCodeService: QRCodeService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.qrCodeService.getAll().subscribe({
      next: (data) => {
        this.qrCodes.set(data);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Failed to load QR codes. Please try again.');
        this.isLoading.set(false);
      }
    });
  }

  delete(id: string): void {
    this.qrCodeService.delete(id).subscribe({
      next: () => this.qrCodes.update(codes => codes.filter(c => c.id !== id)),
      error: () => this.error.set('Failed to delete QR code.')
    });
  }
}
