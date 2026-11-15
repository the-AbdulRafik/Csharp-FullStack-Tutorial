import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-section-header',
  standalone: false,
  template: `
    <header class="section-header-wrap">
      <div class="title-block">
        <span class="eyebrow" *ngIf="eyebrow">{{ eyebrow | uppercase }}</span>
        <h2 class="section-title">{{ title }}</h2>
        <p class="section-subtitle" *ngIf="subtitle">{{ subtitle }}</p>
      </div>
      <div class="header-actions">
        <ng-content select="[actions]"></ng-content>
      </div>
    </header>
  `,
  styles: [`
    .section-header-wrap {
      display: flex;
      justify-content: space-between;
      align-items: flex-end;
      gap: 1rem;
      margin-bottom: 2rem;
      flex-wrap: wrap;
    }

    .eyebrow {
      display: inline-block;
      font-size: 0.75rem;
      font-weight: 600;
      letter-spacing: 0.08em;
      color: var(--accent-primary);
      margin-bottom: 0.35rem;
    }

    .section-title {
      font-size: 1.85rem;
      font-weight: 700;
      color: var(--text-primary);
      letter-spacing: -0.02em;
      line-height: 1.2;
    }

    .section-subtitle {
      font-size: 0.95rem;
      color: var(--text-secondary);
      margin-top: 0.35rem;
      max-width: 600px;
    }

    .header-actions {
      display: flex;
      align-items: center;
      gap: 0.75rem;
    }
  `]
})
export class SectionHeaderComponent {
  @Input() title = '';
  @Input() subtitle = '';
  @Input() eyebrow = '';
}
