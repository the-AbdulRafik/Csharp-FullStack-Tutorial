import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-quick-filter',
  standalone: false,
  template: `
    <div class="filter-controls-wrap">
      <div class="search-input-box">
        <input
          type="text"
          class="filter-input"
          placeholder="Filter by keyword or technology..."
          [(ngModel)]="query"
          (ngModelChange)="onQueryChange()"
        />
        <button
          type="button"
          class="clear-btn"
          *ngIf="query"
          (click)="clearQuery()"
          aria-label="Clear filter"
        >
          &times;
        </button>
      </div>

      <div class="category-pills">
        <button
          type="button"
          class="pill-btn"
          [class.active]="selectedCategory === 'All'"
          (click)="selectCategory('All')"
        >
          All
        </button>

        <button
          type="button"
          class="pill-btn"
          *ngFor="let cat of categories"
          [class.active]="selectedCategory === cat"
          (click)="selectCategory(cat)"
        >
          {{ cat }}
        </button>
      </div>
    </div>
  `,
  styles: [`
    .filter-controls-wrap {
      display: flex;
      flex-direction: column;
      gap: 1rem;
      margin-bottom: 2rem;
    }

    .search-input-box {
      position: relative;
      width: 100%;
      max-width: 480px;
    }

    .filter-input {
      width: 100%;
      padding: 0.65rem 2.2rem 0.65rem 0.9rem;
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      color: var(--text-primary);
      border-radius: 8px;
      font-size: 0.9rem;
      font-family: inherit;
      outline: none;
      transition: border-color var(--transition-fast);
    }

    .filter-input:focus {
      border-color: var(--border-focus);
    }

    .clear-btn {
      position: absolute;
      right: 0.6rem;
      top: 50%;
      transform: translateY(-50%);
      font-size: 1.25rem;
      color: var(--text-muted);
      line-height: 1;
    }

    .clear-btn:hover {
      color: var(--text-primary);
    }

    .category-pills {
      display: flex;
      flex-wrap: wrap;
      gap: 0.5rem;
    }

    .pill-btn {
      padding: 0.35rem 0.85rem;
      border-radius: 9999px;
      font-size: 0.8rem;
      font-weight: 500;
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      color: var(--text-secondary);
      transition: all var(--transition-fast);
    }

    .pill-btn:hover {
      border-color: var(--text-muted);
      color: var(--text-primary);
    }

    .pill-btn.active {
      background-color: var(--accent-primary);
      border-color: var(--accent-primary);
      color: #ffffff;
    }
  `]
})
export class QuickFilterComponent {
  @Input() categories: string[] = ['AI & Agents', 'Agent Runtime', 'Full-Stack'];
  @Output() filterChange = new EventEmitter<{ category: string; query: string }>();

  selectedCategory = 'All';
  query = '';

  selectCategory(cat: string): void {
    this.selectedCategory = cat;
    this.emitChange();
  }

  onQueryChange(): void {
    this.emitChange();
  }

  clearQuery(): void {
    this.query = '';
    this.emitChange();
  }

  // Method accessed via @ViewChild in parent component
  resetFilters(): void {
    this.selectedCategory = 'All';
    this.query = '';
    this.emitChange();
  }

  private emitChange(): void {
    this.filterChange.emit({
      category: this.selectedCategory,
      query: this.query
    });
  }
}
