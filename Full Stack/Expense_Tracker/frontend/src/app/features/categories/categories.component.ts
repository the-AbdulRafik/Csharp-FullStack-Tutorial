import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Category } from '../../core/models/category.model';
import { CategoryService } from '../../core/services/category.service';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <section class="page-card">
      <div class="section-heading">
        <div><h1>Categories</h1><p>Organize income and expense entries.</p></div>
      </div>

      @if (error()) { <div class="alert-error">{{ error() }}</div> }

      <form [formGroup]="form" (ngSubmit)="save()" class="category-form">
        <label>Name
          <input formControlName="name" maxlength="50" placeholder="e.g. Salary" />
        </label>
        <label>Type
          <select formControlName="type">
            <option value="Income">Income</option>
            <option value="Expense">Expense</option>
          </select>
        </label>
        <button type="submit" [disabled]="form.invalid || loading()">
          {{ editingId() ? 'Update category' : 'Add category' }}
        </button>
        @if (editingId()) {
          <button type="button" class="secondary" (click)="cancelEdit()">Cancel</button>
        }
      </form>

      @if (loading()) {
        <p>Loading categories...</p>
      } @else if (categories().length === 0) {
        <p>No categories yet.</p>
      } @else {
        <div class="table-wrap">
          <table>
            <thead><tr><th>Name</th><th>Type</th><th>Actions</th></tr></thead>
            <tbody>
              @for (category of categories(); track category.id) {
                <tr>
                  <td>{{ category.name }}</td>
                  <td><span [class]="category.type === 'Income' ? 'badge income' : 'badge expense'">{{ category.type }}</span></td>
                  <td>
                    <button type="button" class="link-button" (click)="edit(category)">Edit</button>
                    <button type="button" class="danger" (click)="remove(category)">Delete</button>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>
      }
    </section>
  `
})
export class CategoriesComponent implements OnInit {
  private readonly service = inject(CategoryService);
  private readonly fb = inject(FormBuilder);

  readonly categories = signal<Category[]>([]);
  readonly loading = signal(true);
  readonly error = signal('');
  readonly editingId = signal<number | null>(null);

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(50)]],
    type: this.fb.nonNullable.control<'Income' | 'Expense'>('Expense')
  });

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.error.set('');
    this.service.getAll().subscribe({
      next: categories => this.categories.set(categories),
      error: () => this.error.set('Could not load categories.'),
      complete: () => this.loading.set(false)
    });
  }

  save(): void {
    if (this.form.invalid) return;
    this.error.set('');
    const request = this.form.getRawValue();
    const operation = this.editingId()
      ? this.service.update(this.editingId()!, request)
      : this.service.create(request);

    operation.subscribe({
      next: () => {
        this.cancelEdit();
        this.load();
      },
      error: error => this.error.set(error.error?.detail ?? 'Could not save the category.')
    });
  }

  edit(category: Category): void {
    this.editingId.set(category.id);
    this.form.reset({ name: category.name, type: category.type });
  }

  cancelEdit(): void {
    this.editingId.set(null);
    this.form.reset({ name: '', type: 'Expense' });
  }

  remove(category: Category): void {
    if (!confirm(`Delete "${category.name}"?`)) return;
    this.service.delete(category.id).subscribe({
      next: () => this.load(),
      error: error => this.error.set(error.error?.detail ?? 'Could not delete the category.')
    });
  }
}
