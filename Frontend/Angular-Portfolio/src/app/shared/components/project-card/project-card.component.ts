import {
  Component,
  Input,
  Output,
  EventEmitter,
  OnInit,
  OnChanges,
  OnDestroy,
  SimpleChanges
} from '@angular/core';
import { Project } from '../../../models/portfolio.models';

@Component({
  selector: 'app-project-card',
  standalone: false,
  template: `
    <article
      class="project-card-container"
      appCardHover
      [ngClass]="{ 'featured-border': project.featured }"
      (click)="onCardClick()"
    >
      <div class="card-top-row">
        <span class="project-year">{{ project.year }}</span>
        <span class="badge category-badge">{{ project.category | uppercase }}</span>
      </div>

      <h3 class="project-title">{{ project.title }}</h3>

      <p class="project-summary">
        {{ project.summary | truncate: 120 }}
      </p>

      <div class="tech-stack-wrap">
        <span
          class="badge tech-badge"
          *ngFor="let tech of project.technologies | slice: 0: 4"
        >
          {{ tech }}
        </span>
        <span
          class="badge more-badge"
          *ngIf="project.technologies.length > 4"
        >
          +{{ project.technologies.length - 4 }}
        </span>
      </div>

      <div class="card-footer-row" *ngIf="showDetailsButton">
        <button
          type="button"
          class="btn btn-outline details-btn"
          (click)="onDetailsClick($event)"
        >
          View Details &rarr;
        </button>

        <a
          *ngIf="project.githubUrl"
          [href]="project.githubUrl"
          target="_blank"
          rel="noopener noreferrer"
          class="github-link"
          (click)="$event.stopPropagation()"
        >
          GitHub
        </a>
      </div>
    </article>
  `,
  styles: [`
    .project-card-container {
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      border-radius: 12px;
      padding: 1.5rem;
      display: flex;
      flex-direction: column;
      height: 100%;
      cursor: pointer;
    }

    .featured-border {
      border-left: 3px solid var(--accent-primary);
    }

    .card-top-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 0.85rem;
    }

    .project-year {
      font-size: 0.8rem;
      font-family: var(--font-mono);
      color: var(--text-muted);
    }

    .category-badge {
      font-size: 0.7rem;
    }

    .project-title {
      font-size: 1.2rem;
      font-weight: 700;
      color: var(--text-primary);
      margin-bottom: 0.65rem;
      line-height: 1.3;
    }

    .project-summary {
      font-size: 0.9rem;
      color: var(--text-secondary);
      line-height: 1.5;
      margin-bottom: 1.25rem;
      flex-grow: 1;
    }

    .tech-stack-wrap {
      display: flex;
      flex-wrap: wrap;
      gap: 0.4rem;
      margin-bottom: 1.25rem;
    }

    .tech-badge {
      background-color: var(--bg-secondary);
      color: var(--text-secondary);
      border-color: var(--border-color);
    }

    .more-badge {
      background-color: transparent;
      color: var(--text-muted);
      border-color: transparent;
    }

    .card-footer-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      padding-top: 1rem;
      border-top: 1px solid var(--border-color);
      margin-top: auto;
    }

    .details-btn {
      padding: 0.4rem 0.85rem;
      font-size: 0.8rem;
    }

    .github-link {
      font-size: 0.8rem;
      color: var(--text-muted);
      text-decoration: underline;
      transition: color var(--transition-fast);
    }

    .github-link:hover {
      color: var(--text-primary);
    }
  `]
})
export class ProjectCardComponent implements OnInit, OnChanges, OnDestroy {
  @Input({ required: true }) project!: Project;
  @Input() showDetailsButton = true;

  @Output() selectProject = new EventEmitter<Project>();
  @Output() viewDetails = new EventEmitter<string>();

  ngOnInit(): void {
    // Lifecycle demonstration: track initialization
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Lifecycle demonstration: react to input property changes
    if (changes['project'] && !changes['project'].firstChange) {
      // Input reference updated
    }
  }

  ngOnDestroy(): void {
    // Lifecycle demonstration: clean up listeners/timers
  }

  onCardClick(): void {
    this.selectProject.emit(this.project);
  }

  onDetailsClick(event: MouseEvent): void {
    event.stopPropagation();
    this.viewDetails.emit(this.project.id);
  }
}
