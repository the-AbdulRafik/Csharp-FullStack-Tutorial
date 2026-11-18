import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Category, TransactionType } from '../../core/models/category.model';
import { Transaction } from '../../core/models/transaction.model';
import { CategoryService } from '../../core/services/category.service';
import { TransactionService } from '../../core/services/transaction.service';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <section class="page-card">
      <div class="section-heading"><div><h1>Transactions</h1><p>Record and review income or expenses.</p></div></div>
      @if (error()) { <div class="alert-error">{{ error() }}</div> }

      <form [formGroup]="form" (ngSubmit)="save()" class="transaction-form">
        <label>Title<input formControlName="title" maxlength="100" /></label>
        <label>Amount<input type="number" min="0.01" step="0.01" formControlName="amount" /></label>
        <label>Type
          <select formControlName="type" (change)="refreshCategoryOptions()">
            <option value="Income">Income</option>
            <option value="Expense">Expense</option>
          </select>
        </label>
        <label>Date<input type="date" formControlName="date" /></label>
        <label>Category
          <select formControlName="categoryId">
            <option [ngValue]="null">Select category</option>
            @for (category of matchingCategories(); track category.id) {
              <option [ngValue]="category.id">{{ category.name }}</option>
            }
          </select>
        </label>
        <label class="wide">Description<textarea maxlength="500" formControlName="description"></textarea></label>
        <div class="form-actions">
          <button type="submit" [disabled]="form.invalid || loading()">{{ editingId() ? 'Update' : 'Add transaction' }}</button>
          @if (editingId()) { <button type="button" class="secondary" (click)="cancelEdit()">Cancel</button> }
        </div>
      </form>

      <div class="filters">
        <label>Type
          <select [value]="filterType()" (change)="setFilterType($event)">
            <option value="">All</option>
            <option value="Income">Income</option>
            <option value="Expense">Expense</option>
          </select>
        </label>
        <label>Category
          <select [value]="filterCategory()" (change)="setFilterCategory($event)">
            <option value="">All</option>
            @for (category of categories(); track category.id) {
              <option [value]="category.id">{{ category.name }}</option>
            }
          </select>
        </label>
        <label>From<input type="date" [value]="filterFrom()" (change)="filterFrom.set(eventTarget($event))" /></label>
        <label>To<input type="date" [value]="filterTo()" (change)="filterTo.set(eventTarget($event))" /></label>
        <button type="button" class="secondary" (click)="load()">Apply</button>
      </div>

      @if (loading()) { <p>Loading transactions...</p> }
      @else if (transactions().length === 0) { <p>No transactions found.</p> }
      @else {
        <div class="table-wrap">
          <table>
            <thead><tr><th>Date</th><th>Title</th><th>Category</th><th>Type</th><th class="amount">Amount</th><th>Actions</th></tr></thead>
            <tbody>
              @for (transaction of transactions(); track transaction.id) {
                <tr>
                  <td>{{ transaction.date }}</td>
                  <td>{{ transaction.title }}</td>
                  <td>{{ transaction.category.name }}</td>
                  <td><span [class]="transaction.type === 'Income' ? 'badge income' : 'badge expense'">{{ transaction.type }}</span></td>
                  <td class="amount">{{ transaction.amount | currency:'INR':'symbol':'1.2-2' }}</td>
                  <td><button type="button" class="link-button" (click)="edit(transaction)">Edit</button>
                    <button type="button" class="danger" (click)="remove(transaction)">Delete</button></td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      }
    </section>
  `
})
export class TransactionsComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly categoriesApi = inject(CategoryService);
  private readonly transactionsApi = inject(TransactionService);

  readonly transactions = signal<Transaction[]>([]);
  readonly categories = signal<Category[]>([]);
  readonly matchingCategories = signal<Category[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');
  readonly editingId = signal<number | null>(null);
  readonly filterType = signal<TransactionType | ''>('');
  readonly filterCategory = signal('');
  readonly filterFrom = signal('');
  readonly filterTo = signal('');

  readonly form = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    amount: [0, [Validators.required, Validators.min(0.01)]],
    type: this.fb.nonNullable.control<TransactionType>('Expense'),
    date: [new Date().toISOString().slice(0, 10), Validators.required],
    description: [''],
    categoryId: [null as number | null, Validators.required]
  });

  ngOnInit(): void {
    this.categoriesApi.getAll().subscribe({
      next: categories => {
        this.categories.set(categories);
        this.refreshCategoryOptions();
      },
      error: () => this.error.set('Could not load categories.')
    });
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set('');
    this.transactionsApi.getAll({
      type: this.filterType() || undefined,
      categoryId: this.filterCategory() ? Number(this.filterCategory()) : undefined,
      from: this.filterFrom() || undefined,
      to: this.filterTo() || undefined
    }).subscribe({
      next: transactions => this.transactions.set(transactions),
      error: () => this.error.set('Could not load transactions.'),
      complete: () => this.loading.set(false)
    });
  }

  refreshCategoryOptions(): void {
    const type = this.form.controls.type.value;
    const matching = this.categories().filter(category => category.type === type);
    this.matchingCategories.set(matching);
  }

  save(): void {
    if (this.form.invalid) return;
    const value = this.form.getRawValue();
    const request = { ...value, categoryId: value.categoryId! };
    const operation = this.editingId()
      ? this.transactionsApi.update(this.editingId()!, request)
      : this.transactionsApi.create(request);

    operation.subscribe({
      next: () => {
        this.cancelEdit();
        this.load();
      },
      error: error => this.error.set(error.error?.detail ?? 'Could not save the transaction.')
    });
  }

  edit(transaction: Transaction): void {
    this.editingId.set(transaction.id);
    this.form.reset({
      title: transaction.title,
      amount: transaction.amount,
      type: transaction.type,
      date: transaction.date,
      description: transaction.description ?? '',
      categoryId: transaction.categoryId
    });
    this.refreshCategoryOptions();
  }

  cancelEdit(): void {
    this.editingId.set(null);
    this.form.reset({
      title: '',
      amount: 0,
      type: 'Expense',
      date: new Date().toISOString().slice(0, 10),
      description: '',
      categoryId: null
    });
    this.refreshCategoryOptions();
  }

  remove(transaction: Transaction): void {
    if (!confirm(`Delete "${transaction.title}"?`)) return;
    this.transactionsApi.delete(transaction.id).subscribe({
      next: () => this.load(),
      error: error => this.error.set(error.error?.detail ?? 'Could not delete the transaction.')
    });
  }

  setFilterType(event: Event): void {
    this.filterType.set((event.target as HTMLSelectElement).value as TransactionType | '');
  }

  setFilterCategory(event: Event): void {
    this.filterCategory.set((event.target as HTMLSelectElement).value);
  }

  eventTarget(event: Event): string {
    return (event.target as HTMLInputElement).value;
  }
}
