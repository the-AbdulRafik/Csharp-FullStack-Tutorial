import { Component, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { HealthService } from './core/services/health.service';

@Component({
  selector: 'app-root',
  imports: [RouterLink, RouterLinkActive, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  private readonly healthService = inject(HealthService);
  protected readonly title = signal('ExpenseTracker');
  protected readonly healthStatus = this.healthService.status;

  constructor() {
    this.healthService.check();
  }
}
