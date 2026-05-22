import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WalletService } from '../../../services/wallet.service';
import { Wallet, Transaction } from '../../../models/wallet';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-wallet',
  imports: [CommonModule, FormsModule],
  templateUrl: './wallet.html',
  styleUrl: './wallet.css',
})
export class WalletComponent implements OnInit {
  wallet = signal<Wallet | null>(null);
  transactions = signal<Transaction[]>([]);
  isLoading = signal(true);
  error = signal<string | null>(null);

  constructor(
    private walletService: WalletService,
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

    this.walletService.getByUserId(user.id).subscribe({
      next: (wallet) => {
        this.wallet.set(wallet);
        this.walletService.getTransactions(wallet.id).subscribe({
          next: (txns) => {
            this.transactions.set(txns);
            this.isLoading.set(false);
          },
          error: () => {
            this.isLoading.set(false);
          }
        });
      },
      error: (err) => {
        // 404 = wallet not yet created for this user (normal for admin/partner)
        if (err?.status === 404) {
          this.wallet.set(null);
          this.isLoading.set(false);
        } else {
          this.error.set('Failed to load wallet. Please try again.');
          this.isLoading.set(false);
        }
      }
    });
  }

  getTransactionClass(type: string): string {
    const credits = ['Credit', 'Reward', 'Refund'];
    return credits.includes(type) ? 'txn--credit' : 'txn--debit';
  }

  getTransactionSign(type: string): string {
    const credits = ['Credit', 'Reward', 'Refund'];
    return credits.includes(type) ? '+' : '-';
  }
}
