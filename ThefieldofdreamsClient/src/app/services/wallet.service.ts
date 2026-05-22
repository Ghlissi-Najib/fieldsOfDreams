import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';
import { Wallet, Transaction, CreateTransactionRequest } from '../models/wallet';

@Injectable({ providedIn: 'root' })
export class WalletService {
  private readonly apiUrl = `${environment.apiUrl}/wallets`;

  constructor(private http: HttpClient) {}

  getByUserId(userId: string): Observable<Wallet> {
    return this.http.get<Wallet>(`${this.apiUrl}/user/${userId}`);
  }

  getTransactions(walletId: string): Observable<Transaction[]> {
    return this.http.get<Transaction[]>(`${environment.apiUrl}/transactions/wallet/${walletId}`);
  }

  createTransaction(dto: CreateTransactionRequest): Observable<Transaction> {
    return this.http.post<Transaction>(`${environment.apiUrl}/transactions`, dto);
  }
}
