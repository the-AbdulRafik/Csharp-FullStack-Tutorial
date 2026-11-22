import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StudentLayout } from './student-layout';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';

describe('StudentLayout', () => {
  let component: StudentLayout;
  let fixture: ComponentFixture<StudentLayout>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentLayout],
      providers: [provideHttpClient(), provideRouter([])],
    }).compileComponents();

    fixture = TestBed.createComponent(StudentLayout);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
