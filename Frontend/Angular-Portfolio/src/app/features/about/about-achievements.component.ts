import { Component, OnInit, inject } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';
import { PortfolioDataService } from '../../core/services/portfolio-data.service';
import { AchievementItem } from '../../models/portfolio.models';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-about-achievements',
  standalone: true,
  imports: [SharedModule],
  template: `
    <div class="achievements-wrap">
      <h3 class="sub-heading">Hackathon & Competitive Engineering Honors</h3>
      <p class="section-intro">
        Awards won in state-level and 24-hour rapid development hackathons solving real-world challenges.
      </p>

      <div class="achievements-grid">
        <app-card-wrapper
          *ngFor="let item of achievements$ | async"
          variant="elevated"
          appCardHover
        >
          <div card-header class="achieve-header">
            <span class="badge award-badge">{{ item.award }}</span>
            <span class="achieve-date">{{ item.date }}</span>
          </div>

          <h4 class="competition-title">{{ item.competition }}</h4>
          <p class="details-text">
            <strong>Project:</strong> {{ item.details }}
          </p>
        </app-card-wrapper>
      </div>
    </div>
  `,
  styles: [`
    .achievements-wrap {
      max-width: 900px;
    }

    .sub-heading {
      font-size: 1.3rem;
      font-weight: 700;
      color: var(--text-primary);
      margin-bottom: 0.5rem;
    }

    .section-intro {
      font-size: 0.95rem;
      color: var(--text-secondary);
      margin-bottom: 2rem;
    }

    .achievements-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
      gap: 1.5rem;
    }

    .achieve-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .award-badge {
      background-color: var(--accent-tint);
      color: var(--accent-primary);
      border-color: rgba(59, 130, 246, 0.3);
    }

    .achieve-date {
      font-size: 0.8rem;
      font-family: var(--font-mono);
      color: var(--text-muted);
    }

    .competition-title {
      font-size: 1.1rem;
      font-weight: 700;
      color: var(--text-primary);
      margin-bottom: 0.5rem;
    }

    .details-text {
      font-size: 0.9rem;
      color: var(--text-secondary);
    }
  `]
})
export class AboutAchievementsComponent implements OnInit {
  private readonly dataService = inject(PortfolioDataService);
  achievements$!: Observable<AchievementItem[]>;

  ngOnInit(): void {
    this.achievements$ = this.dataService.getAchievements();
  }
}
