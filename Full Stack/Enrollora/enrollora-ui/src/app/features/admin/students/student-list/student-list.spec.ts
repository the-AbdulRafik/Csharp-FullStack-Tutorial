import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { StudentList } from './student-list';
import { StudentService } from '../student.service';

describe('StudentList', () => {
  let component: StudentList;
  let fixture: ComponentFixture<StudentList>;

  beforeEach(async () => {
    const studentServiceMock = {
      getStudents: () => of({ items: [], totalRecords: 0, totalPages: 0, pageNumber: 1, pageSize: 5 }),
      getStudentById: () => of(null),
      updateStatus: () => of(null),
    };

    await TestBed.configureTestingModule({
      imports: [StudentList],
      providers: [
        { provide: StudentService, useValue: studentServiceMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(StudentList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

