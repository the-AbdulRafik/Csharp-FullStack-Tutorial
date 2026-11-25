import {
  Component,
  OnInit,
  inject,
  signal,
  computed,
  ChangeDetectionStrategy,
  DestroyRef
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { switchMap, catchError, of } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CourseService } from '../course.service';
import { CourseResponse } from '../course.models';
import { SharedModule } from '../../../../shared/shared.module';

@Component({
  selector: 'app-course-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, SharedModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="course-detail-container">
      <div class="nav-back mb-3">
        <a routerLink="/admin/courses" class="btn btn-sm btn-outline-secondary">
          &larr; Back to Courses
        </a>
      </div>

      <div *ngIf="isLoading()" class="alert alert-info py-3">
        Loading course analytics...
      </div>

      <div *ngIf="errorMessage()" class="alert alert-danger py-3">
        {{ errorMessage() }}
      </div>

      <div *ngIf="!isLoading() && !errorMessage() && course() as c">
        <div class="detail-header p-4 bg-white rounded border mb-4">
          <div class="d-flex justify-content-between align-items-start flex-wrap gap-2">
            <div>
              <span class="badge bg-secondary mb-2">Course #{{ c.courseId }}</span>
              <h2 class="mb-1 text-primary">{{ c.courseName }}</h2>
              <p class="text-muted mb-0">Duration: {{ c.duration }}</p>
            </div>
            <app-status-badge [status]="c.isActive ? 'active' : 'cancelled'"></app-status-badge>
          </div>
        </div>

        <div class="row g-3">
          <div class="col-md-4">
            <app-card [elevated]="true">
              <span card-header class="fw-bold">Enrollment Occupancy</span>
              <div class="occupancy-display py-2">
                <h3 class="display-6 fw-bold mb-1">{{ occupancyRate() }}%</h3>
                <p class="text-muted mb-2">{{ c.enrolledCount }} enrolled / {{ c.capacity }} total seats</p>
                <div class="progress" style="height: 10px;">
                  <div
                    class="progress-bar"
                    role="progressbar"
                    [style.width.%]="occupancyRate()"
                    [class.bg-success]="occupancyRate() < 80"
                    [class.bg-warning]="occupancyRate() >= 80 && occupancyRate() < 100"
                    [class.bg-danger]="occupancyRate() >= 100"
                  ></div>
                </div>
              </div>
            </app-card>
          </div>

          <div class="col-md-4">
            <app-card [elevated]="true">
              <span card-header class="fw-bold">Fee Structure</span>
              <div class="fees-display py-2">
                <h3 class="display-6 fw-bold mb-1 text-success">{{ c.fees | inrCurrency }}</h3>
                <p class="text-muted mb-0">Standard tuition fee per enrolled student</p>
              </div>
            </app-card>
          </div>

          <div class="col-md-4">
            <app-card [elevated]="true">
              <span card-header class="fw-bold">Projected Revenue (Signal Computed)</span>
              <div class="revenue-display py-2">
                <h3 class="display-6 fw-bold mb-1 text-primary">{{ projectedRevenue() | inrCurrency }}</h3>
                <p class="text-muted mb-0">Computed dynamically via Angular signals</p>
              </div>
            </app-card>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .course-detail-container {
      padding: 1.5rem;
    }
  `]
})
export class CourseDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private courseService = inject(CourseService);
  private destroyRef = inject(DestroyRef);

  course = signal<CourseResponse | null>(null);
  isLoading = signal(true);
  errorMessage = signal('');

  occupancyRate = computed(() => {
    const c = this.course();
    if (!c || c.capacity <= 0) return 0;
    return Math.min(100, Math.round((c.enrolledCount / c.capacity) * 100));
  });

  projectedRevenue = computed(() => {
    const c = this.course();
    if (!c) return 0;
    return c.enrolledCount * c.fees;
  });

  ngOnInit(): void {
    this.route.paramMap
      .pipe(
        takeUntilDestroyed(this.destroyRef),
        switchMap((params) => {
          this.isLoading.set(true);
          this.errorMessage.set('');
          const id = Number(params.get('id'));
          if (!id || isNaN(id)) {
            this.errorMessage.set('Invalid course identifier.');
            this.isLoading.set(false);
            return of(null);
          }
          return this.courseService.getCourseById(id).pipe(
            catchError((err) => {
              this.errorMessage.set(err.error?.detail || err.error?.message || 'Course was not found.');
              this.isLoading.set(false);
              return of(null);
            })
          );
        })
      )
      .subscribe((course) => {
        if (course) {
          this.course.set(course);
        }
        this.isLoading.set(false);
      });
  }
}
