import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnChanges,
  SimpleChanges,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormArray,
  FormControl,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { CourseResponse, SaveCourseRequest } from '../course.models';
import { courseNameValidator, positiveNumberValidator } from '../../../../shared/validators/course.validators';

@Component({
  selector: 'app-course-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <form [formGroup]="courseForm" (ngSubmit)="onSubmit()" class="course-form" novalidate>
      <div class="form-group mb-3">
        <label for="courseName" class="form-label">Course Name *</label>
        <input
          type="text"
          id="courseName"
          class="form-control"
          formControlName="courseName"
          placeholder="e.g. Distributed Systems with Kafka"
        />
        <div
          class="text-danger small mt-1"
          *ngIf="courseForm.get('courseName')?.invalid && (courseForm.get('courseName')?.dirty || courseForm.get('courseName')?.touched)"
        >
          Course name must be at least 3 characters.
        </div>
      </div>

      <div class="row g-2 mb-3">
        <div class="col-md-6">
          <label for="duration" class="form-label">Duration *</label>
          <input
            type="text"
            id="duration"
            class="form-control"
            formControlName="duration"
            placeholder="e.g. 8 Weeks"
          />
          <div
            class="text-danger small mt-1"
            *ngIf="courseForm.get('duration')?.invalid && courseForm.get('duration')?.touched"
          >
            Duration is required.
          </div>
        </div>

        <div class="col-md-6">
          <label for="fees" class="form-label">Fees (INR) *</label>
          <input
            type="number"
            id="fees"
            class="form-control"
            formControlName="fees"
            placeholder="e.g. 5000"
          />
          <div
            class="text-danger small mt-1"
            *ngIf="courseForm.get('fees')?.invalid && courseForm.get('fees')?.touched"
          >
            Fees must be a positive number.
          </div>
        </div>
      </div>

      <div class="row g-2 mb-3">
        <div class="col-md-6">
          <label for="capacity" class="form-label">Seat Capacity *</label>
          <input
            type="number"
            id="capacity"
            class="form-control"
            formControlName="capacity"
            placeholder="e.g. 40"
          />
          <div
            class="text-danger small mt-1"
            *ngIf="courseForm.get('capacity')?.invalid && courseForm.get('capacity')?.touched"
          >
            Capacity must be at least 1.
          </div>
        </div>

        <div class="col-md-6 d-flex align-items-center mt-4">
          <div class="form-check">
            <input
              type="checkbox"
              id="isActive"
              class="form-check-input"
              formControlName="isActive"
            />
            <label for="isActive" class="form-check-label">Available for Enrollment</label>
          </div>
        </div>
      </div>

      <!-- FormArray Demonstration: Dynamic Syllabus Modules / Prerequisites -->
      <div class="modules-section mb-3">
        <div class="d-flex justify-content-between align-items-center mb-2">
          <label class="form-label mb-0">Course Prerequisites / Key Modules (FormArray)</label>
          <button
            type="button"
            class="btn btn-sm btn-outline-secondary"
            (click)="addModule()"
          >
            + Add Module
          </button>
        </div>

        <div formArrayName="modules" class="module-list">
          <div
            *ngFor="let mod of modules.controls; let i = index"
            class="input-group input-group-sm mb-2"
          >
            <span class="input-group-text">#{{ i + 1 }}</span>
            <input
              type="text"
              class="form-control"
              [formControlName]="i"
              placeholder="Module topic or prerequisite"
            />
            <button
              type="button"
              class="btn btn-outline-danger"
              (click)="removeModule(i)"
              [disabled]="modules.length <= 1"
            >
              &times;
            </button>
          </div>
        </div>
      </div>

      <div class="form-actions d-flex justify-content-end gap-2 mt-4">
        <button type="button" class="btn btn-secondary" (click)="cancel.emit()">
          Cancel
        </button>
        <button
          type="submit"
          class="btn btn-primary"
          [disabled]="courseForm.invalid"
        >
          {{ isEditMode ? 'Update Course' : 'Create Course' }}
        </button>
      </div>
    </form>
  `,
  styles: [`
    .course-form {
      padding: 0.5rem 0;
    }
    .modules-section {
      background: #f9fafb;
      padding: 0.75rem;
      border-radius: 6px;
      border: 1px solid #e5e7eb;
    }
  `]
})
export class CourseFormComponent implements OnInit, OnChanges {
  private fb = inject(FormBuilder);

  @Input() courseToEdit: CourseResponse | null = null;
  @Output() formSubmit = new EventEmitter<SaveCourseRequest>();
  @Output() cancel = new EventEmitter<void>();

  courseForm!: FormGroup;
  isEditMode = false;

  ngOnInit(): void {
    this.buildForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['courseToEdit']) {
      this.populateForm();
    }
  }

  get modules(): FormArray {
    return this.courseForm.get('modules') as FormArray;
  }

  addModule(initialValue = ''): void {
    this.modules.push(new FormControl(initialValue, Validators.required));
  }

  removeModule(index: number): void {
    if (this.modules.length > 1) {
      this.modules.removeAt(index);
    }
  }

  private buildForm(): void {
    this.courseForm = this.fb.group({
      courseName: ['', [Validators.required, courseNameValidator()]],
      duration: ['', Validators.required],
      fees: [1000, [Validators.required, positiveNumberValidator()]],
      capacity: [30, [Validators.required, Validators.min(1)]],
      isActive: [true],
      modules: this.fb.array([new FormControl('Core Principles', Validators.required)])
    });

    this.populateForm();
  }

  private populateForm(): void {
    if (!this.courseForm) {
      return;
    }

    if (this.courseToEdit) {
      this.isEditMode = true;
      this.courseForm.patchValue({
        courseName: this.courseToEdit.courseName,
        duration: this.courseToEdit.duration,
        fees: this.courseToEdit.fees,
        capacity: this.courseToEdit.capacity,
        isActive: this.courseToEdit.isActive
      });
    } else {
      this.isEditMode = false;
      this.courseForm.reset({
        courseName: '',
        duration: '',
        fees: 1000,
        capacity: 30,
        isActive: true
      });
      this.modules.clear();
      this.modules.push(new FormControl('Core Principles', Validators.required));
    }
  }

  onSubmit(): void {
    if (this.courseForm.invalid) {
      this.courseForm.markAllAsTouched();
      return;
    }

    const val = this.courseForm.value;
    const request: SaveCourseRequest = {
      courseName: val.courseName.trim(),
      duration: val.duration.trim(),
      fees: Number(val.fees),
      capacity: Number(val.capacity),
      isActive: Boolean(val.isActive)
    };

    this.formSubmit.emit(request);
  }
}
