import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-card',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="app-card" [class.elevated]="elevated">
      <div class="card-header" *ngIf="hasHeader">
        <ng-content select="[card-header]"></ng-content>
      </div>
      <div class="card-body">
        <ng-content></ng-content>
      </div>
      <div class="card-footer">
        <ng-content select="[card-footer]"></ng-content>
      </div>
    </div>
  `,
  styles: [`
    .app-card {
      background: #ffffff;
      border: 1px solid #e5e7eb;
      border-radius: 8px;
      padding: 1.25rem;
      margin-bottom: 1rem;
      transition: box-shadow 0.2s ease;
    }
    .app-card.elevated {
      box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -2px rgba(0, 0, 0, 0.05);
    }
    .card-header:not(:empty) {
      padding-bottom: 0.75rem;
      border-bottom: 1px solid #f3f4f6;
      margin-bottom: 0.75rem;
    }
    .card-footer:not(:empty) {
      padding-top: 0.75rem;
      border-top: 1px solid #f3f4f6;
      margin-top: 0.75rem;
    }
  `]
})
export class AppCardComponent {
  @Input() elevated = false;
  @Input() hasHeader = true;
}
