import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { ProjectService } from '../../core/services/project.service';
import { Project } from '../../models/portfolio.models';
import { switchMap } from 'rxjs';

@Component({
  selector: 'app-project-detail',
  standalone: true,
  imports: [SharedModule],
  template: `
    <div class="project-detail-page container" *ngIf="project">
      <nav class="back-nav">
        <a routerLink="/projects" class="back-link">
          &larr; Back to all projects
        </a>
      </nav>

      <article class="detail-card">
        <div class="detail-header">
          <div class="meta-row">
            <span class="badge category-badge">{{ project.category | uppercase }}</span>
            <span class="project-year">{{ project.year }}</span>
          </div>

          <h1 class="project-title">{{ project.title }}</h1>
          <p class="project-lead">{{ project.summary }}</p>
        </div>

        <div class="detail-body">
          <section class="section-block">
            <h3 class="block-title">Key Engineering Highlights</h3>
            <ul class="highlights-list">
              <li *ngFor="let point of project.highlights">
                {{ point }}
              </li>
            </ul>
          </section>

          <section class="section-block">
            <h3 class="block-title">Technology Stack & Tooling</h3>
            <div class="tech-pills">
              <span class="badge tech-badge" *ngFor="let tech of project.technologies">
                {{ tech }}
              </span>
            </div>
          </section>

          <section class="section-block actions-block" *ngIf="project.githubUrl">
            <h3 class="block-title">Source Repository</h3>
            <a
              [href]="project.githubUrl"
              target="_blank"
              rel="noopener noreferrer"
              class="btn btn-outline"
            >
              Inspect on GitHub &rarr;
            </a>
          </section>
        </div>
      </article>
    </div>
  `,
  styles: [`
    .project-detail-page {
      padding-top: 2.5rem;
      padding-bottom: 5rem;
      max-width: 860px;
    }

    .back-nav {
      margin-bottom: 2rem;
    }

    .back-link {
      font-size: 0.9rem;
      color: var(--text-muted);
      transition: color var(--transition-fast);
      display: inline-flex;
      align-items: center;
      gap: 0.5rem;
    }

    .back-link:hover {
      color: var(--accent-primary);
    }

    .detail-card {
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      border-radius: 12px;
      overflow: hidden;
      padding: 2.5rem;
    }

    .meta-row {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 1rem;
    }

    .project-year {
      font-size: 0.85rem;
      font-family: var(--font-mono);
      color: var(--text-muted);
    }

    .project-title {
      font-size: 2.2rem;
      font-weight: 800;
      color: var(--text-primary);
      line-height: 1.2;
      margin-bottom: 1rem;
    }

    .project-lead {
      font-size: 1.1rem;
      color: var(--text-secondary);
      line-height: 1.6;
      padding-bottom: 2rem;
      border-bottom: 1px solid var(--border-color);
    }

    .detail-body {
      padding-top: 2rem;
      display: flex;
      flex-direction: column;
      gap: 2.5rem;
    }

    .block-title {
      font-size: 1.15rem;
      font-weight: 700;
      color: var(--text-primary);
      margin-bottom: 1rem;
    }

    .highlights-list {
      list-style-type: disc;
      padding-left: 1.25rem;
      display: flex;
      flex-direction: column;
      gap: 0.85rem;
      color: var(--text-secondary);
      line-height: 1.6;
    }

    .tech-pills {
      display: flex;
      flex-wrap: wrap;
      gap: 0.5rem;
    }

    .tech-badge {
      font-size: 0.85rem;
      padding: 0.35rem 0.75rem;
    }

    .actions-block {
      padding-top: 1.5rem;
      border-top: 1px solid var(--border-color);
    }
  `]
})
export class ProjectDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly projectService = inject(ProjectService);

  project: Project | undefined;

  ngOnInit(): void {
    // Demonstrating route parameter extraction with ActivatedRoute paramMap
    this.route.paramMap
      .pipe(
        switchMap((params) => {
          const id = params.get('id') || '';
          return this.projectService.getProjectById(id);
        })
      )
      .subscribe((proj) => {
        if (!proj) {
          this.router.navigate(['/projects']);
        } else {
          this.project = proj;
        }
      });
  }
}
