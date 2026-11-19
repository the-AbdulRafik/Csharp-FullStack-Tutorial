import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { HealthService } from './core/services/health.service';
import { AuthService } from './core/services/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private readonly healthService = inject(HealthService);
  private readonly authService = inject(AuthService);
  protected readonly title = signal('ExpenseTracker');
  protected readonly healthStatus = this.healthService.status;
  protected readonly isAuthenticated = this.authService.isAuthenticated;

  constructor() {
    this.healthService.check();
  }

  protected logout(): void {
    this.authService.logout();
  }
}
