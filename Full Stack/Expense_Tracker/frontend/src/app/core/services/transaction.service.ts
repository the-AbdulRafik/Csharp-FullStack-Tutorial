import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { TransactionType } from '../models/category.model';
import { Transaction, TransactionRequest } from '../models/transaction.model';

@Injectable({ providedIn: 'root' })
export class TransactionService {
  private readonly http = inject(HttpClient);
  private readonly url = `${environment.apiBaseUrl}/api/transactions`;

  getAll(filters: {
    type?: TransactionType;
    categoryId?: number;
    from?: string;
    to?: string;
  } = {}): Observable<Transaction[]> {
    let params = new HttpParams();
    if (filters.type) params = params.set('type', filters.type);
    if (filters.categoryId) params = params.set('categoryId', filters.categoryId);
    if (filters.from) params = params.set('from', filters.from);
    if (filters.to) params = params.set('to', filters.to);
    return this.http.get<Transaction[]>(this.url, { params });
  }

  create(request: TransactionRequest): Observable<Transaction> {
    return this.http.post<Transaction>(this.url, request);
  }

  update(id: number, request: TransactionRequest): Observable<Transaction> {
    return this.http.put<Transaction>(`${this.url}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}`);
  }
}
