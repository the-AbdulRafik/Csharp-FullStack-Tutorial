import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TransactionType } from '../models/category.model';

export interface MonthlyReport {
  month: string;
  income: number;
  expenses: number;
}

export interface CategoryReport {
  categoryId: number;
  categoryName: string;
  amount: number;
  type: TransactionType;
}

export interface ReportSummary {
  totalIncome: number;
  totalExpenses: number;
  balance: number;
  monthly: MonthlyReport[];
  categories: CategoryReport[];
}

@Injectable({ providedIn: 'root' })
export class ReportService {
  private readonly http = inject(HttpClient);

  getSummary(from?: string, to?: string): Observable<ReportSummary> {
    let params = new HttpParams();
    if (from) params = params.set('from', from);
    if (to) params = params.set('to', to);
    return this.http.get<ReportSummary>(
      `${environment.apiBaseUrl}/api/reports/summary`,
      { params });
  }
}
