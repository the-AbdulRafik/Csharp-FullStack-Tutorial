import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TransactionType } from '../models/category.model';

export interface RecentTransaction {
  id: number;
  title: string;
  amount: number;
  type: TransactionType;
  date: string;
  categoryName: string;
}

export interface CategoryExpense {
  categoryId: number;
  categoryName: string;
  amount: number;
}

export interface DashboardSummary {
  totalIncome: number;
  totalExpenses: number;
  currentBalance: number;
  currentMonthIncome: number;
  currentMonthExpenses: number;
  recentTransactions: RecentTransaction[];
  categoryExpenses: CategoryExpense[];
}

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private readonly http = inject(HttpClient);

  getSummary(): Observable<DashboardSummary> {
    return this.http.get<DashboardSummary>(
      `${environment.apiBaseUrl}/api/dashboard/summary`);
  }
}
