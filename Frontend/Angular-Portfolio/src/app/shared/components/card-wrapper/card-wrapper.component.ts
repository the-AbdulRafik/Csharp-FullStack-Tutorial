import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-card-wrapper',
  standalone: false,
  template: `
    <div
      class="custom-card"
      [ngClass]="{
        'card-elevated': variant === 'elevated',
        'card-bordered': variant === 'default'
      }"
      [ngStyle]="customStyles"
    >
      <div class="card-header-slot" *ngIf="hasHeaderSlot">
        <ng-content select="[card-header]"></ng-content>
      </div>

      <div class="card-body-slot">
        <ng-content></ng-content>
      </div>

      <div class="card-footer-slot">
        <ng-content select="[card-footer]"></ng-content>
      </div>
    </div>
  `,
  styles: [`
    .custom-card {
      background-color: var(--bg-card);
      border-radius: 12px;
      padding: 1.5rem;
      border: 1px solid var(--border-color);
      transition: all var(--transition-normal);
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    .card-elevated {
      box-shadow: var(--shadow-md);
    }

    .card-header-slot:not(:empty) {
      padding-bottom: 0.75rem;
      border-bottom: 1px solid var(--border-color);
    }

    .card-footer-slot:not(:empty) {
      margin-top: auto;
      padding-top: 0.75rem;
      border-top: 1px solid var(--border-color);
    }
  `]
})
export class CardWrapperComponent {
  @Input() variant: 'default' | 'elevated' = 'default';
  @Input() hasHeaderSlot = true;
  @Input() customStyles: Record<string, string> = {};
}
