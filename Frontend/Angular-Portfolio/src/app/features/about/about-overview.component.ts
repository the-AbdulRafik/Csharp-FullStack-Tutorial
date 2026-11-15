import { Component, OnInit, inject } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { PortfolioDataService } from '../../core/services/portfolio-data.service';
import { SkillGroup, ExperienceItem, EducationItem } from '../../models/portfolio.models';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-about-overview',
  standalone: true,
  imports: [SharedModule],
  template: `
    <div class="overview-grid">
      <!-- Professional Summary & Work Experience -->
      <section class="experience-block">
        <h3 class="sub-heading">Internship Experience</h3>

        <div class="timeline">
          <app-card-wrapper
            *ngFor="let exp of experience$ | async"
            variant="elevated"
            appCardHover
          >
            <div card-header class="exp-header">
              <div>
                <h4 class="exp-role">{{ exp.role }}</h4>
                <p class="exp-company">{{ exp.company }} &bull; {{ exp.location }}</p>
              </div>
              <span class="badge exp-period">{{ exp.period }}</span>
            </div>

            <ul class="highlights-list">
              <li *ngFor="let point of exp.highlights">{{ point }}</li>
            </ul>
          </app-card-wrapper>
        </div>

        <h3 class="sub-heading mt-6">Education</h3>
        <div class="education-wrap">
          <app-card-wrapper
            *ngFor="let edu of education$ | async"
            variant="default"
          >
            <div card-header class="edu-header">
              <div>
                <h4 class="edu-degree">{{ edu.degree }}</h4>
                <p class="edu-school">{{ edu.institution }}, {{ edu.university }}</p>
                <p class="edu-loc">{{ edu.location }}</p>
              </div>
              <div class="edu-meta">
                <span class="badge score-badge">{{ edu.score }}</span>
                <span class="period-text">{{ edu.period }}</span>
              </div>
            </div>
          </app-card-wrapper>
        </div>
      </section>

      <!-- Technical Skills Grouped -->
      <section class="skills-block">
        <h3 class="sub-heading">Categorized Technical Skills</h3>

        <div class="skills-categories">
          <div class="skill-category-card" *ngFor="let group of skillGroups$ | async">
            <h4 class="category-name">{{ group.category }}</h4>
            <div class="skill-tags">
              <span class="skill-tag" *ngFor="let skill of group.skills">
                {{ skill }}
              </span>
            </div>
          </div>
        </div>
      </section>
    </div>
  `,
  styles: [`
    .overview-grid {
      display: grid;
      grid-template-columns: 1.15fr 0.85fr;
      gap: 2.5rem;
    }

    .sub-heading {
      font-size: 1.3rem;
      font-weight: 700;
      color: var(--text-primary);
      margin-bottom: 1.25rem;
      letter-spacing: -0.01em;
    }

    .mt-6 {
      margin-top: 2.5rem;
    }

    .timeline {
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
    }

    .exp-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      gap: 1rem;
    }

    .exp-role {
      font-size: 1.15rem;
      font-weight: 700;
      color: var(--text-primary);
    }

    .exp-company {
      font-size: 0.85rem;
      color: var(--text-secondary);
      margin-top: 0.2rem;
    }

    .exp-period {
      white-space: nowrap;
    }

    .highlights-list {
      list-style-type: disc;
      padding-left: 1.25rem;
      display: flex;
      flex-direction: column;
      gap: 0.65rem;
      color: var(--text-secondary);
      font-size: 0.9rem;
      line-height: 1.5;
    }

    .edu-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      gap: 1rem;
    }

    .edu-degree {
      font-size: 1.05rem;
      font-weight: 700;
      color: var(--text-primary);
    }

    .edu-school {
      font-size: 0.85rem;
      color: var(--text-secondary);
      margin-top: 0.2rem;
    }

    .edu-loc {
      font-size: 0.8rem;
      color: var(--text-muted);
    }

    .edu-meta {
      display: flex;
      flex-direction: column;
      align-items: flex-end;
      gap: 0.35rem;
    }

    .score-badge {
      background-color: var(--accent-tint);
      color: var(--accent-primary);
      border-color: rgba(59, 130, 246, 0.3);
    }

    .period-text {
      font-size: 0.8rem;
      color: var(--text-muted);
      font-family: var(--font-mono);
    }

    .skills-categories {
      display: flex;
      flex-direction: column;
      gap: 1.25rem;
    }

    .skill-category-card {
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      border-radius: 10px;
      padding: 1.1rem;
    }

    .category-name {
      font-size: 0.85rem;
      font-family: var(--font-mono);
      font-weight: 600;
      text-transform: uppercase;
      letter-spacing: 0.05em;
      color: var(--accent-primary);
      margin-bottom: 0.75rem;
    }

    .skill-tags {
      display: flex;
      flex-wrap: wrap;
      gap: 0.4rem;
    }

    .skill-tag {
      font-size: 0.8rem;
      background-color: var(--bg-secondary);
      color: var(--text-secondary);
      border: 1px solid var(--border-color);
      padding: 0.25rem 0.55rem;
      border-radius: 6px;
    }

    @media (max-width: 850px) {
      .overview-grid {
        grid-template-columns: 1fr;
      }
    }
  `]
})
export class AboutOverviewComponent implements OnInit {
  private readonly dataService = inject(PortfolioDataService);

  experience$!: Observable<ExperienceItem[]>;
  education$!: Observable<EducationItem[]>;
  skillGroups$!: Observable<SkillGroup[]>;

  ngOnInit(): void {
    this.experience$ = this.dataService.getExperience();
    this.education$ = this.dataService.getEducation();
    this.skillGroups$ = this.dataService.getSkillGroups();
  }
}
