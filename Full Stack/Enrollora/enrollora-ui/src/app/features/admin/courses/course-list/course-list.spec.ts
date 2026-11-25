import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { CourseListComponent } from './course-list';
import { CourseService } from '../course.service';

describe('CourseList', () => {
  let component: CourseListComponent;
  let fixture: ComponentFixture<CourseListComponent>;

  beforeEach(async () => {
    const courseServiceMock = {
      getCourses: () => of({ items: [], totalRecords: 0, totalPages: 0, pageNumber: 1, pageSize: 10 }),
      createCourse: () => of(null),
      updateCourse: () => of(null),
      deleteCourse: () => of(null),
    };

    await TestBed.configureTestingModule({
      imports: [CourseListComponent],
      providers: [
        provideRouter([]),
        { provide: CourseService, useValue: courseServiceMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(CourseListComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

