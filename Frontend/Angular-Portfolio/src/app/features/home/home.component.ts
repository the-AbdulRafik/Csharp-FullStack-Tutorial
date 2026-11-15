import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { PortfolioDataService } from '../../core/services/portfolio-data.service';
import { Project, PortfolioStats } from '../../models/portfolio.models';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [SharedModule],
  template: `
    <section class="hero-section container">
      <div class="hero-content">
        <div class="hero-badge-wrap">
          <span class="status-indicator"></span>
          <span class="hero-tag">{{ profile.headline | uppercase }}</span>
        </div>

        <h1 class="hero-name">{{ profile.name }}</h1>

        <p class="hero-summary">
          {{ profile.summary }}
        </p>

        <div class="hero-actions">
          <a routerLink="/projects" class="btn btn-primary">
            Explore Projects &rarr;
          </a>
          <a routerLink="/contact" class="btn btn-outline">
            Get In Touch
          </a>
          <a routerLink="/about" class="btn btn-outline">
            Full Background
          </a>
        </div>
      </div>

      <!-- Live Demonstration Terminal / System Status Card -->
      <div class="terminal-card" appCardHover>
        <div class="terminal-header">
          <div class="window-dots">
            <span class="dot red"></span>
            <span class="dot yellow"></span>
            <span class="dot green"></span>
          </div>
          <span class="terminal-title">system-profile.sh</span>
        </div>

        <div class="terminal-body">
          <p class="term-line"><span class="prompt">$</span> whoami</p>
          <p class="term-output">Abdul Rafik S [AI Backend Engineer]</p>

          <p class="term-line"><span class="prompt">$</span> cat focus.txt</p>
          <p class="term-output">LangGraph &bull; Multi-Agent Systems &bull; Kafka &bull; Temporal</p>

          <p class="term-line"><span class="prompt">$</span> check-stats --http-interceptor</p>
          <div class="term-stats" *ngIf="stats$ | async as stats; else loadingStats">
            <div class="stat-pill">
              <span class="stat-label">System Health</span>
              <span class="stat-val text-green">{{ stats.systemHealth }}</span>
            </div>
            <div class="stat-pill">
              <span class="stat-label">Active Projects</span>
              <span class="stat-val">{{ stats.activeProjectsCount }}</span>
            </div>
            <div class="stat-pill">
              <span class="stat-label">Hackathon Wins</span>
              <span class="stat-val">{{ stats.hackathonWins }}</span>
            </div>
            <div class="stat-pill">
              <span class="stat-label">Verified Skills</span>
              <span class="stat-val">{{ stats.skillsCount }}+</span>
            </div>
          </div>
          <ng-template #loadingStats>
            <p class="term-output text-muted">Querying local asset via HttpClient & Interceptor...</p>
          </ng-template>
        </div>
      </div>
    </section>

    <!-- Key Skills Highlight -->
    <section class="skills-highlight-section container">
      <app-section-header
        eyebrow="Core Competencies"
        title="Engineering Foundations"
        subtitle="Primary languages, agentic frameworks, and backend technologies from practical builds."
      ></app-section-header>

      <div class="highlights-grid">
        <div class="skill-tile" *ngFor="let skill of coreHighlights">
          <span class="tile-category">{{ skill.category }}</span>
          <h4 class="tile-title">{{ skill.name }}</h4>
          <p class="tile-desc">{{ skill.desc }}</p>
        </div>
      </div>
    </section>

    <!-- Featured Projects Section -->
    <section class="featured-projects-section container">
      <app-section-header
        eyebrow="Portfolio Highlights"
        title="Featured Work"
        subtitle="Recent production-grade systems, AI agent runtimes, and distributed workflows."
      >
        <a routerLink="/projects" class="btn btn-outline" actions>
          All Projects ({{ (featuredProjects$ | async)?.length || 3 }}) &rarr;
        </a>
      </app-section-header>

      <div class="grid-cols-3">
        <app-project-card
          *ngFor="let project of featuredProjects$ | async"
          [project]="project"
          (selectProject)="onProjectSelected($event)"
          (viewDetails)="onViewDetails($event)"
        ></app-project-card>
      </div>
    </section>
  `,
  styles: [`
    .hero-section {
      padding-top: 4rem;
      padding-bottom: 4rem;
      display: grid;
      grid-template-columns: 1.2fr 0.9fr;
      gap: 3rem;
      align-items: center;
    }

    .hero-badge-wrap {
      display: inline-flex;
      align-items: center;
      gap: 0.5rem;
      padding: 0.3rem 0.75rem;
      border-radius: 9999px;
      background-color: var(--accent-tint);
      border: 1px solid rgba(59, 130, 246, 0.25);
      margin-bottom: 1.25rem;
    }

    .status-indicator {
      width: 8px;
      height: 8px;
      border-radius: 50%;
      background-color: var(--accent-secondary);
      box-shadow: 0 0 8px var(--accent-secondary);
    }

    .hero-tag {
      font-size: 0.75rem;
      font-family: var(--font-mono);
      font-weight: 600;
      color: var(--accent-primary);
      letter-spacing: 0.05em;
    }

    .hero-name {
      font-size: 2.85rem;
      font-weight: 800;
      line-height: 1.15;
      letter-spacing: -0.03em;
      margin-bottom: 1.25rem;
      color: var(--text-primary);
    }

    .hero-summary {
      font-size: 1.05rem;
      color: var(--text-secondary);
      line-height: 1.6;
      margin-bottom: 2rem;
      max-width: 580px;
    }

    .hero-actions {
      display: flex;
      flex-wrap: wrap;
      gap: 0.85rem;
    }

    .terminal-card {
      background-color: var(--bg-secondary);
      border: 1px solid var(--border-color);
      border-radius: 12px;
      overflow: hidden;
      font-family: var(--font-mono);
      font-size: 0.85rem;
    }

    .terminal-header {
      background-color: var(--bg-card);
      padding: 0.65rem 1rem;
      display: flex;
      align-items: center;
      border-bottom: 1px solid var(--border-color);
      position: relative;
    }

    .window-dots {
      display: flex;
      gap: 6px;
    }

    .dot {
      width: 10px;
      height: 10px;
      border-radius: 50%;
    }

    .dot.red { background-color: #ef4444; }
    .dot.yellow { background-color: #f59e0b; }
    .dot.green { background-color: #10b981; }

    .terminal-title {
      position: absolute;
      left: 50%;
      transform: translateX(-50%);
      font-size: 0.75rem;
      color: var(--text-muted);
    }

    .terminal-body {
      padding: 1.25rem;
      display: flex;
      flex-direction: column;
      gap: 0.75rem;
    }

    .term-line {
      color: var(--text-primary);
    }

    .prompt {
      color: var(--accent-secondary);
      font-weight: 700;
      margin-right: 0.4rem;
    }

    .term-output {
      color: var(--text-secondary);
      margin-left: 1rem;
      font-size: 0.8rem;
    }

    .text-green {
      color: var(--accent-secondary);
    }

    .text-muted {
      color: var(--text-muted);
    }

    .term-stats {
      display: grid;
      grid-template-columns: repeat(2, 1fr);
      gap: 0.6rem;
      margin-top: 0.5rem;
      margin-left: 1rem;
    }

    .stat-pill {
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      padding: 0.5rem;
      border-radius: 6px;
      display: flex;
      flex-direction: column;
      gap: 0.2rem;
    }

    .stat-label {
      font-size: 0.7rem;
      color: var(--text-muted);
    }

    .stat-val {
      font-size: 0.95rem;
      font-weight: 700;
      color: var(--text-primary);
    }

    .skills-highlight-section, .featured-projects-section {
      padding-top: 2rem;
      padding-bottom: 4rem;
    }

    .highlights-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
      gap: 1.25rem;
    }

    .skill-tile {
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      border-radius: 10px;
      padding: 1.25rem;
      transition: border-color var(--transition-fast);
    }

    .skill-tile:hover {
      border-color: var(--border-focus);
    }

    .tile-category {
      font-size: 0.7rem;
      font-family: var(--font-mono);
      color: var(--accent-primary);
      text-transform: uppercase;
      letter-spacing: 0.05em;
    }

    .tile-title {
      font-size: 1.1rem;
      font-weight: 700;
      color: var(--text-primary);
      margin-top: 0.35rem;
      margin-bottom: 0.4rem;
    }

    .tile-desc {
      font-size: 0.85rem;
      color: var(--text-secondary);
      line-height: 1.4;
    }

    @media (max-width: 850px) {
      .hero-section {
        grid-template-columns: 1fr;
        padding-top: 2.5rem;
      }
    }
  `]
})
export class HomeComponent implements OnInit {
  private readonly dataService = inject(PortfolioDataService);
  private readonly router = inject(Router);

  readonly profile = this.dataService.profile;
  featuredProjects$!: Observable<Project[]>;
  stats$!: Observable<PortfolioStats>;

  readonly coreHighlights = [
    {
      category: 'Agent Orchestration',
      name: 'LangGraph & LangChain',
      desc: 'Multi-turn autonomous workflows, state graph DAGs, and adaptive Critic-Finalizer loops.'
    },
    {
      category: 'Backend Architecture',
      name: 'FastAPI & Flask',
      desc: 'High-throughput microservices, REST APIs, and asynchronous background pipelines.'
    },
    {
      category: 'Distributed Systems',
      name: 'Kafka & Temporal',
      desc: 'Event stream mapping and reliable workflow migration for core microservices.'
    },
    {
      category: 'Runtime & LLMs',
      name: 'Gemini, Groq & Ollama',
      desc: 'Multimodal vision parsing, prompt decoupling, and local LLM orchestration.'
    }
  ];

  ngOnInit(): void {
    this.featuredProjects$ = this.dataService.getFeaturedProjects();
    this.stats$ = this.dataService.fetchLiveStats();
  }

  onProjectSelected(project: Project): void {
    this.router.navigate(['/projects', project.id]);
  }

  onViewDetails(projectId: string): void {
    this.router.navigate(['/projects', projectId]);
  }
}
