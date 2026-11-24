import { Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../../shared/shared.module';
import { CourseFormComponent } from '../course-form/course-form.component';

import { CourseService } from '../course.service';
import { CourseResponse, SaveCourseRequest } from '../course.models';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, SharedModule, CourseFormComponent],
  templateUrl: './course-list.html',
  styleUrl: './course-list.css',
})
export class CourseListComponent implements OnInit {
  private courseService = inject(CourseService);

  // Demonstration of @ViewChild accessing a child component
  @ViewChild(CourseFormComponent) courseFormComponent?: CourseFormComponent;

  // Signals
  courses = signal<CourseResponse[]>([]);

  isLoading = signal(false);
  errorMessage = signal('');

  pageNumber = signal(1);
  pageSize = signal(10);

  totalRecords = signal(0);
  totalPages = signal(0);

  // Modal & Form State
  isModalOpen = signal(false);
  selectedCourseForEdit = signal<CourseResponse | null>(null);

  ngOnInit(): void {
    this.loadCourses();
  }

  openCreateModal(): void {
    this.selectedCourseForEdit.set(null);
    this.isModalOpen.set(true);
  }

  closeModal(): void {
    this.isModalOpen.set(false);
    this.selectedCourseForEdit.set(null);
  }

  handleFormSubmit(request: SaveCourseRequest): void {
    this.isLoading.set(true);
    this.courseService.createCourse(request).subscribe({
      next: () => {
        this.closeModal();
        this.loadCourses();
      },
      error: (err) => {
        this.errorMessage.set(err.error?.detail || err.error?.message || 'Failed to create course.');
        this.isLoading.set(false);
      }
    });
  }

  // Load courses from backend
  loadCourses(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.courseService.getCourses(this.pageNumber(), this.pageSize()).subscribe({
      next: (response) => {
        this.courses.set(response.items);

        this.totalRecords.set(response.totalRecords);
        this.totalPages.set(response.totalPages);

        this.isLoading.set(false);
      },

      error: (error) => {
        console.error('Error loading courses:', error);

        this.errorMessage.set('Unable to load courses.');

        this.isLoading.set(false);
      },
    });
  }

  // Navigate to next page
  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update((page) => page + 1);

      this.loadCourses();
    }
  }

  // Navigate to previous page
  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.update((page) => page - 1);

      this.loadCourses();
    }
  }
}
