export type TransactionType = 'Credit' | 'Debit' | 'Reward' | 'Purchase' | 'Refund';
export type TransactionStatus = 'Pending' | 'Completed' | 'Failed' | 'Cancelled';

export interface Wallet {
  id: string;
  userId: string;
  balance: number;
  points: number;
  stripeCustomerId?: string;
}

export interface Transaction {
  id: string;
  walletId: string;
  amount: number;
  type: TransactionType;
  description?: string;
  referenceId?: string;
  status: TransactionStatus;
  createdAt: string;
}

export interface CreateTransactionRequest {
  walletId: string;
  amount: number;
  type: TransactionType;
  description?: string;
  referenceId?: string;
}
