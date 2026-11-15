import { Component } from '@angular/core';
import { SharedModule } from '../../shared/shared.module';

@Component({
  selector: 'app-not-found',
  standalone: true,
  imports: [SharedModule],
  template: `
    <div class="not-found-page container">
      <div class="error-box">
        <span class="error-code">404</span>
        <h1 class="error-title">Route Not Found</h1>
        <p class="error-desc">
          The requested path does not exist in this portfolio application.
        </p>
        <div class="actions">
          <a routerLink="/" class="btn btn-primary">Return to Home</a>
          <a routerLink="/projects" class="btn btn-outline">Explore Projects</a>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .not-found-page {
      padding-top: 6rem;
      padding-bottom: 6rem;
      display: flex;
      justify-content: center;
      align-items: center;
    }

    .error-box {
      text-align: center;
      max-width: 500px;
      padding: 3rem;
      background-color: var(--bg-card);
      border: 1px solid var(--border-color);
      border-radius: 12px;
    }

    .error-code {
      font-size: 4rem;
      font-family: var(--font-mono);
      font-weight: 800;
      color: var(--accent-primary);
      line-height: 1;
    }

    .error-title {
      font-size: 1.75rem;
      font-weight: 700;
      color: var(--text-primary);
      margin-top: 1rem;
      margin-bottom: 0.75rem;
    }

    .error-desc {
      font-size: 0.95rem;
      color: var(--text-secondary);
      margin-bottom: 2rem;
    }

    .actions {
      display: flex;
      justify-content: center;
      gap: 1rem;
    }
  `]
})
export class NotFoundComponent {}
