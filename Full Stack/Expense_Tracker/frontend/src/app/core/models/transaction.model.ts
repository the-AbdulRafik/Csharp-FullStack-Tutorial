import { TransactionType } from './category.model';

export interface TransactionCategory {
  id: number;
  name: string;
  type: TransactionType;
}

export interface Transaction {
  id: number;
  title: string;
  amount: number;
  type: TransactionType;
  date: string;
  description?: string;
  categoryId: number;
  category: TransactionCategory;
  createdAt: string;
  updatedAt?: string;
}

export interface TransactionRequest {
  title: string;
  amount: number;
  type: TransactionType;
  date: string;
  description?: string;
  categoryId: number;
}
