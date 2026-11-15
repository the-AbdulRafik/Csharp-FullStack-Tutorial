import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { ProjectService } from '../../core/services/project.service';
import { Project } from '../../models/portfolio.models';
import { QuickFilterComponent } from '../../shared/components/quick-filter/quick-filter.component';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [SharedModule],
  template: `
    <div class="projects-page container">
      <app-section-header
        eyebrow="Portfolio Architecture"
        title="Engineering Projects"
        subtitle="End-to-end AI workflows, agent security evaluation, and microservices."
      >
        <button
          type="button"
          class="btn btn-outline reset-btn"
          (click)="onResetFilterClicked()"
          actions
        >
          Reset Filters
        </button>
      </app-section-header>

      <!-- Child Filter Component accessed via @ViewChild -->
      <app-quick-filter
        #quickFilterRef
        [categories]="['AI & Agents', 'Agent Runtime', 'Full-Stack']"
        (filterChange)="onFilterChanged($event)"
      ></app-quick-filter>

      <!-- Projects Grid using modern control flow @if / @for -->
      @if (projects$ | async; as projects) {
        @if (projects.length > 0) {
          <div class="grid-cols-3">
            @for (project of projects; track project.id) {
              <app-project-card
                [project]="project"
                (selectProject)="onProjectSelected($event)"
                (viewDetails)="onViewDetails($event)"
              ></app-project-card>
            }
          </div>
        } @else {
          <div class="empty-state">
            <p class="empty-title">No projects match the current filter criteria.</p>
            <button type="button" class="btn btn-primary" (click)="onResetFilterClicked()">
              Clear Search
            </button>
          </div>
        }
      }
    </div>
  `,
  styles: [`
    .projects-page {
      padding-top: 3rem;
      padding-bottom: 5rem;
    }

    .reset-btn {
      font-size: 0.85rem;
      padding: 0.45rem 0.9rem;
    }

    .empty-state {
      padding: 4rem 1rem;
      text-align: center;
      background-color: var(--bg-card);
      border: 1px dashed var(--border-color);
      border-radius: 12px;
      display: flex;
      flex-direction: column;
      align-items: center;
      gap: 1.25rem;
    }

    .empty-title {
      font-size: 1.1rem;
      color: var(--text-secondary);
    }
  `]
})
export class ProjectsComponent implements OnInit {
  private readonly projectService = inject(ProjectService);
  private readonly router = inject(Router);

  // Demonstration of @ViewChild accessing a child component
  @ViewChild('quickFilterRef') quickFilter!: QuickFilterComponent;

  projects$!: Observable<Project[]>;
  currentCategory = 'All';
  currentQuery = '';

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.projects$ = this.projectService.filterProjects(
      this.currentCategory,
      this.currentQuery
    );
  }

  onFilterChanged(event: { category: string; query: string }): void {
    this.currentCategory = event.category;
    this.currentQuery = event.query;
    this.loadProjects();
  }

  onProjectSelected(project: Project): void {
    this.router.navigate(['/projects', project.id]);
  }

  onViewDetails(projectId: string): void {
    this.router.navigate(['/projects', projectId]);
  }

  // Demonstration of calling child component method via @ViewChild
  onResetFilterClicked(): void {
    if (this.quickFilter) {
      this.quickFilter.resetFilters();
    } else {
      this.currentCategory = 'All';
      this.currentQuery = '';
      this.loadProjects();
    }
  }
}
