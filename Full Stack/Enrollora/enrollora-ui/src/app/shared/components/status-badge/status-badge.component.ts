import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnChanges,
  OnDestroy,
  SimpleChanges,
  ChangeDetectionStrategy
} from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <span
      class="badge"
      [ngClass]="badgeClass"
      (click)="onBadgeClick()"
      [title]="clickable ? 'Click to filter by this status' : ''"
      [style.cursor]="clickable ? 'pointer' : 'default'"
    >
      <span class="status-dot"></span>
      {{ label }}
    </span>
  `,
  styles: [`
    .badge {
      display: inline-flex;
      align-items: center;
      gap: 0.35rem;
      padding: 0.25rem 0.65rem;
      border-radius: 9999px;
      font-size: 0.75rem;
      font-weight: 600;
      text-transform: capitalize;
    }
    .status-dot {
      width: 6px;
      height: 6px;
      border-radius: 50%;
      background-color: currentColor;
    }
    .badge-approved, .badge-active {
      background-color: #ecfdf5;
      color: #065f46;
      border: 1px solid #a7f3d0;
    }
    .badge-pending {
      background-color: #fffbeb;
      color: #92400e;
      border: 1px solid #fde68a;
    }
    .badge-rejected, .badge-cancelled {
      background-color: #fef2f2;
      color: #991b1b;
      border: 1px solid #fecaca;
    }
    .badge-default {
      background-color: #f3f4f6;
      color: #374151;
      border: 1px solid #e5e7eb;
    }
  `]
})
export class StatusBadgeComponent implements OnInit, OnChanges, OnDestroy {
  @Input({ required: true }) status: string | number | null = null;
  @Input() clickable = false;
  @Output() badgeClick = new EventEmitter<string | number>();

  label = '';
  badgeClass = 'badge-default';

  ngOnInit(): void {
    this.computeBadge();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['status']) {
      this.computeBadge();
    }
  }

  ngOnDestroy(): void {
    // Lifecycle cleanup demonstration
  }

  onBadgeClick(): void {
    if (this.clickable && this.status !== null) {
      this.badgeClick.emit(this.status);
    }
  }

  private computeBadge(): void {
    const raw = String(this.status ?? '').toLowerCase();
    if (raw === '0' || raw === 'pending') {
      this.label = 'Pending';
      this.badgeClass = 'badge-pending';
    } else if (raw === '1' || raw === 'approved' || raw === 'active') {
      this.label = raw === '1' ? 'Approved' : 'Active';
      this.badgeClass = 'badge-approved';
    } else if (raw === '2' || raw === 'rejected' || raw === 'cancelled') {
      this.label = raw === '2' ? 'Rejected' : 'Cancelled';
      this.badgeClass = 'badge-rejected';
    } else {
      this.label = raw || 'Unknown';
      this.badgeClass = 'badge-default';
    }
  }
}
