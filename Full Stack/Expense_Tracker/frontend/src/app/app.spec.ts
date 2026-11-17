import { TestBed } from '@angular/core/testing';
import { DashboardComponent } from './features/dashboard/dashboard.component';

describe('DashboardComponent', () => {
  it('creates the Phase 0 dashboard placeholder', async () => {
    const fixture = await TestBed.configureTestingModule({
      imports: [DashboardComponent]
    }).createComponent(DashboardComponent);

    fixture.detectChanges();

    expect(fixture.nativeElement.textContent).toContain('Dashboard');
  });
});
