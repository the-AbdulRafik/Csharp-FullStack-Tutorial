import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportService, ReportSummary } from '../../core/services/report.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  template: `
    <section class="page-card">
      <div class="section-heading"><div><h1>Reports</h1><p>Review totals and breakdowns using real transaction data.</p></div></div>
      @if (error()) { <div class="alert-error">{{ error() }}</div> }

      <div class="filters">
        <label>From<input type="date" [value]="from()" (change)="from.set(eventTarget($event))" /></label>
        <label>To<input type="date" [value]="to()" (change)="to.set(eventTarget($event))" /></label>
        <button type="button" (click)="load()">Apply range</button>
      </div>

      @if (loading()) { <p>Loading report...</p> }
      @else if (report(); as data) {
        <div class="summary-grid reports-summary">
          <article><span>Income</span><strong>{{ data.totalIncome | currency:'INR':'symbol':'1.2-2' }}</strong></article>
          <article><span>Expenses</span><strong>{{ data.totalExpenses | currency:'INR':'symbol':'1.2-2' }}</strong></article>
          <article><span>Balance</span><strong>{{ data.balance | currency:'INR':'symbol':'1.2-2' }}</strong></article>
        </div>

        <div class="dashboard-grid">
          <section>
            <h2>Monthly summary</h2>
            @if (data.monthly.length === 0) { <p>No monthly data.</p> }
            @else {
              @for (item of data.monthly; track item.month) {
                <div class="report-row"><strong>{{ item.month }}</strong><span>Income {{ item.income | currency:'INR':'symbol':'1.0-0' }}</span><span>Expense {{ item.expenses | currency:'INR':'symbol':'1.0-0' }}</span></div>
              }
            }
          </section>
          <section>
            <h2>Category breakdown</h2>
            @if (data.categories.length === 0) { <p>No category data.</p> }
            @else {
              @for (item of data.categories; track item.categoryId) {
                <div class="category-row"><span>{{ item.categoryName }} · {{ item.type }}</span><strong>{{ item.amount | currency:'INR':'symbol':'1.2-2' }}</strong></div>
              }
            }
          </section>
        </div>
      }
    </section>
  `
})
export class ReportsComponent {
  private readonly service = inject(ReportService);
  readonly report = signal<ReportSummary | null>(null);
  readonly loading = signal(true);
  readonly error = signal('');
  readonly from = signal('');
  readonly to = signal('');

  constructor() {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set('');
    this.service.getSummary(this.from() || undefined, this.to() || undefined).subscribe({
      next: report => this.report.set(report),
      error: () => this.error.set('Could not load report data.'),
      complete: () => this.loading.set(false)
    });
  }

  eventTarget(event: Event): string {
    return (event.target as HTMLInputElement).value;
  }
}
