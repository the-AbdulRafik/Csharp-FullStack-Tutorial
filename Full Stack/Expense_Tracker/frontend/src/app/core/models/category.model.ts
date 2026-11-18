export type TransactionType = 'Income' | 'Expense';

export interface Category {
  id: number;
  name: string;
  type: TransactionType;
  createdAt: string;
}

export interface CategoryRequest {
  name: string;
  type: TransactionType;
}
