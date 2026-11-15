import { Component } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [SharedModule],
  template: `
    <div class="about-page container">
      <app-section-header
        eyebrow="Profile & Background"
        title="About Abdul Rafik S"
        subtitle="Background in AI & Data Science, microservices migration, and multi-turn agent systems."
      ></app-section-header>

      <!-- Sub-route tabs demonstrating nested routing -->
      <nav class="sub-nav-tabs">
        <a
          routerLink="/about"
          routerLinkActive="active-tab"
          [routerLinkActiveOptions]="{ exact: true }"
          class="tab-link"
        >
          Overview & Experience
        </a>
        <a
          routerLink="/about/achievements"
          routerLinkActive="active-tab"
          class="tab-link"
        >
          Hackathons & Honors
        </a>
      </nav>

      <div class="sub-route-container">
        <router-outlet></router-outlet>
      </div>
    </div>
  `,
  styles: [`
    .about-page {
      padding-top: 3rem;
      padding-bottom: 5rem;
    }

    .sub-nav-tabs {
      display: flex;
      gap: 1rem;
      border-bottom: 1px solid var(--border-color);
      margin-bottom: 2.5rem;
    }

    .tab-link {
      padding: 0.65rem 1rem;
      font-size: 0.95rem;
      font-weight: 500;
      color: var(--text-secondary);
      border-bottom: 2px solid transparent;
      transition: all var(--transition-fast);
      margin-bottom: -1px;
    }

    .tab-link:hover {
      color: var(--text-primary);
    }

    .tab-link.active-tab {
      color: var(--accent-primary);
      border-bottom-color: var(--accent-primary);
    }

    .sub-route-container {
      margin-top: 1rem;
    }
  `]
})
export class AboutComponent {}
