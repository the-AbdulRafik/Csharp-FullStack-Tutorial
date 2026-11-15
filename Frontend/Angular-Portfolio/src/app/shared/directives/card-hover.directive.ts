import { Directive, ElementRef, HostListener, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appCardHover]',
  standalone: false
})
export class CardHoverDirective {
  @Input() hoverScale = 1.01;
  @Input() hoverBorder = 'var(--border-focus)';

  private originalBorder = '';

  constructor(
    private el: ElementRef,
    private renderer: Renderer2
  ) {
    this.renderer.setStyle(this.el.nativeElement, 'transition', 'transform 0.2s ease, border-color 0.2s ease, box-shadow 0.2s ease');
  }

  @HostListener('mouseenter')
  onMouseEnter(): void {
    this.originalBorder = this.el.nativeElement.style.borderColor;
    this.renderer.setStyle(this.el.nativeElement, 'transform', `translateY(-3px) scale(${this.hoverScale})`);
    this.renderer.setStyle(this.el.nativeElement, 'borderColor', this.hoverBorder);
    this.renderer.setStyle(this.el.nativeElement, 'boxShadow', 'var(--shadow-md)');
  }

  @HostListener('mouseleave')
  onMouseLeave(): void {
    this.renderer.setStyle(this.el.nativeElement, 'transform', 'translateY(0) scale(1)');
    this.renderer.setStyle(this.el.nativeElement, 'borderColor', this.originalBorder || 'var(--border-color)');
    this.renderer.setStyle(this.el.nativeElement, 'boxShadow', 'var(--shadow-sm)');
  }
}
