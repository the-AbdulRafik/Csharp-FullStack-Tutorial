import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable, combineLatest, map } from 'rxjs';
import { CourseResponse } from '../../features/admin/courses/course.models';
import { CourseService } from '../../features/admin/courses/course.service';

export interface EnrollmentState {
  courses: CourseResponse[];
  searchTerm: string;
  selectedCategory: string;
  lastUpdated: Date;
}

@Injectable({
  providedIn: 'root'
})
export class EnrollmentStateService {
  private courseService = inject(CourseService);

  private readonly _state$ = new BehaviorSubject<EnrollmentState>({
    courses: [],
    searchTerm: '',
    selectedCategory: 'all',
    lastUpdated: new Date()
  });

  readonly state$: Observable<EnrollmentState> = this._state$.asObservable();

  // Stream of search term
  private readonly _searchQuery$ = new BehaviorSubject<string>('');
  readonly searchQuery$ = this._searchQuery$.asObservable();

  // Stream of category filter
  private readonly _availabilityFilter$ = new BehaviorSubject<string>('all');
  readonly availabilityFilter$ = this._availabilityFilter$.asObservable();

  // Reactive derived stream demonstrating combineLatest and map
  readonly filteredCourses$: Observable<CourseResponse[]> = combineLatest([
    this.state$.pipe(map((s) => s.courses)),
    this.searchQuery$,
    this.availabilityFilter$
  ]).pipe(
    map(([courses, query, availability]) => {
      const q = query.trim().toLowerCase();
      return courses.filter((c) => {
        const matchesQuery =
          !q ||
          c.courseName.toLowerCase().includes(q) ||
          c.duration.toLowerCase().includes(q);

        const matchesAvail =
          availability === 'all' ||
          (availability === 'available' && c.availableSeats > 0) ||
          (availability === 'full' && c.availableSeats === 0);

        return matchesQuery && matchesAvail;
      });
    })
  );

  // Derived metrics demonstrating map
  readonly totalAvailableSeats$: Observable<number> = this.state$.pipe(
    map((s) => s.courses.reduce((acc, c) => acc + (c.availableSeats || 0), 0))
  );

  setCourses(courses: CourseResponse[]): void {
    this._state$.next({
      ...this._state$.value,
      courses,
      lastUpdated: new Date()
    });
  }

  setSearchQuery(query: string): void {
    this._searchQuery$.next(query);
    this._state$.next({
      ...this._state$.value,
      searchTerm: query,
      lastUpdated: new Date()
    });
  }

  setAvailabilityFilter(filter: string): void {
    this._availabilityFilter$.next(filter);
    this._state$.next({
      ...this._state$.value,
      selectedCategory: filter,
      lastUpdated: new Date()
    });
  }

  refreshCourses(): Observable<CourseResponse[]> {
    return this.courseService.getCourses(1, 50).pipe(
      map((res) => {
        this.setCourses(res.items);
        return res.items;
      })
    );
  }
}
