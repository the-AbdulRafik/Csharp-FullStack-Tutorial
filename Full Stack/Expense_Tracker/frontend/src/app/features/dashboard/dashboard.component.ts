import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardService, DashboardSummary } from '../../core/services/dashboard.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  template: `
    <section>
      <div class="section-heading"><div><h1>Dashboard</h1><p>Real-time summary from SQL Server.</p></div></div>
      @if (error()) { <div class="alert-error">{{ error() }}</div> }
      @if (loading()) { <p>Loading dashboard...</p> }
      @else if (summary(); as data) {
        <div class="summary-grid">
          <article><span>Total income</span><strong>{{ data.totalIncome | currency:'INR':'symbol':'1.2-2' }}</strong></article>
          <article><span>Total expenses</span><strong>{{ data.totalExpenses | currency:'INR':'symbol':'1.2-2' }}</strong></article>
          <article><span>Current balance</span><strong>{{ data.currentBalance | currency:'INR':'symbol':'1.2-2' }}</strong></article>
          <article><span>This month</span><strong>{{ data.currentMonthIncome - data.currentMonthExpenses | currency:'INR':'symbol':'1.2-2' }}</strong></article>
        </div>

        <div class="dashboard-grid">
          <section class="page-card">
            <h2>Recent transactions</h2>
            @if (data.recentTransactions.length === 0) { <p>No transactions yet.</p> }
            @else {
              @for (transaction of data.recentTransactions; track transaction.id) {
                <div class="recent-row">
                  <div><strong>{{ transaction.title }}</strong><small>{{ transaction.categoryName }} · {{ transaction.date }}</small></div>
                  <b [class]="transaction.type === 'Income' ? 'income-text' : 'expense-text'">
                    {{ transaction.amount | currency:'INR':'symbol':'1.2-2' }}
                  </b>
                </div>
              }
            }
          </section>

          <section class="page-card">
            <h2>Income vs expenses</h2>
            <div class="bar-row"><span>Income</span><div class="bar-track"><div class="bar income-bar" [style.width.%]="barWidth(data.totalIncome, data.totalIncome + data.totalExpenses)"></div></div></div>
            <div class="bar-row"><span>Expenses</span><div class="bar-track"><div class="bar expense-bar" [style.width.%]="barWidth(data.totalExpenses, data.totalIncome + data.totalExpenses)"></div></div></div>

            <h2>Category expenses</h2>
            @for (item of data.categoryExpenses; track item.categoryId) {
              <div class="category-row"><span>{{ item.categoryName }}</span><strong>{{ item.amount | currency:'INR':'symbol':'1.2-2' }}</strong></div>
            }
          </section>
        </div>
      }
    </section>
  `
})
export class DashboardComponent implements OnInit {
  private readonly service = inject(DashboardService);
  readonly summary = signal<DashboardSummary | null>(null);
  readonly loading = signal(true);
  readonly error = signal('');

  ngOnInit(): void {
    this.service.getSummary().subscribe({
      next: summary => this.summary.set(summary),
      error: () => this.error.set('Could not load dashboard data.'),
      complete: () => this.loading.set(false)
    });
  }

  barWidth(value: number, total: number): number {
    return total > 0 ? Math.max(0, Math.min(100, (value / total) * 100)) : 0;
  }
}
