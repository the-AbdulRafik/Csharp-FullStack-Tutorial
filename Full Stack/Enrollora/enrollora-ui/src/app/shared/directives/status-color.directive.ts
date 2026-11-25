import { Directive, ElementRef, Input, OnChanges, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appStatusColor]',
  standalone: true
})
export class StatusColorDirective implements OnChanges {
  @Input('appStatusColor') status: string | number | null = null;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) {}

  ngOnChanges(): void {
    this.applyBorderColor();
  }

  private applyBorderColor(): void {
    const raw = String(this.status ?? '').toLowerCase();
    let border = '#e5e7eb';

    if (raw === '0' || raw === 'pending') {
      border = '#f59e0b';
    } else if (raw === '1' || raw === 'approved' || raw === 'active') {
      border = '#10b981';
    } else if (raw === '2' || raw === 'rejected' || raw === 'cancelled') {
      border = '#ef4444';
    }

    this.renderer.setStyle(this.el.nativeElement, 'borderLeftWidth', '4px');
    this.renderer.setStyle(this.el.nativeElement, 'borderLeftStyle', 'solid');
    this.renderer.setStyle(this.el.nativeElement, 'borderLeftColor', border);
  }
}
