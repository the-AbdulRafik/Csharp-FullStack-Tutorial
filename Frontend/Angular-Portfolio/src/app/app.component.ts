import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ThemeService } from './core/services/theme.service';
import { PortfolioDataService } from './core/services/portfolio-data.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="app-shell">
      <!-- Navbar -->
      <header class="navbar-wrapper">
        <div class="container nav-container">
          <a routerLink="/" class="nav-brand" (click)="closeMobileMenu()">
            <span class="brand-monogram">AR</span>
            <div class="brand-text">
              <span class="brand-name">{{ profile.name }}</span>
              <span class="brand-role">{{ profile.headline }}</span>
            </div>
          </a>

          <!-- Desktop Navigation -->
          <nav class="desktop-nav">
            <a
              routerLink="/"
              routerLinkActive="active"
              [routerLinkActiveOptions]="{ exact: true }"
              class="nav-link"
            >
              Home
            </a>
            <a
              routerLink="/about"
              routerLinkActive="active"
              class="nav-link"
            >
              About
            </a>
            <a
              routerLink="/projects"
              routerLinkActive="active"
              class="nav-link"
            >
              Projects
            </a>
            <a
              routerLink="/contact"
              routerLinkActive="active"
              class="nav-link"
            >
              Contact
            </a>
          </nav>

          <!-- Right actions: Theme Toggle & Mobile Menu Button -->
          <div class="nav-actions">
            <button
              type="button"
              class="theme-toggle-btn"
              (click)="themeService.toggleTheme()"
              [title]="themeService.isDark() ? 'Switch to Light Theme' : 'Switch to Dark Theme'"
              aria-label="Toggle theme"
            >
              <span *ngIf="themeService.isDark()" class="theme-icon">&#9728;&#xFE0E; Light</span>
              <span *ngIf="!themeService.isDark()" class="theme-icon">&#9790;&#xFE0E; Dark</span>
            </button>

            <button
              type="button"
              class="mobile-menu-btn"
              (click)="toggleMobileMenu()"
              [attr.aria-expanded]="mobileMenuOpen"
              aria-label="Toggle navigation menu"
            >
              <span class="bar" [class.open]="mobileMenuOpen"></span>
              <span class="bar" [class.open]="mobileMenuOpen"></span>
              <span class="bar" [class.open]="mobileMenuOpen"></span>
            </button>
          </div>
        </div>

        <!-- Mobile Navigation Drawer -->
        <div class="mobile-drawer" *ngIf="mobileMenuOpen">
          <nav class="mobile-nav-list container">
            <a
              routerLink="/"
              routerLinkActive="active"
              [routerLinkActiveOptions]="{ exact: true }"
              class="mobile-nav-link"
              (click)="closeMobileMenu()"
            >
              Home
            </a>
            <a
              routerLink="/about"
              routerLinkActive="active"
              class="mobile-nav-link"
              (click)="closeMobileMenu()"
            >
              About
            </a>
            <a
              routerLink="/projects"
              routerLinkActive="active"
              class="mobile-nav-link"
              (click)="closeMobileMenu()"
            >
              Projects
            </a>
            <a
              routerLink="/contact"
              routerLinkActive="active"
              class="mobile-nav-link"
              (click)="closeMobileMenu()"
            >
              Contact
            </a>
          </nav>
        </div>
      </header>

      <!-- Main Router Outlet -->
      <main class="main-content">
        <router-outlet></router-outlet>
      </main>

      <!-- Footer -->
      <footer class="footer-wrapper">
        <div class="container footer-container">
          <div class="footer-left">
            <p class="footer-name">{{ profile.name }} &bull; {{ profile.headline }}</p>
            <p class="footer-copy">
              Engineered with Angular &bull; Clean Architecture & Responsive Design
            </p>
          </div>

          <div class="footer-right">
            <a [href]="profile.github" target="_blank" rel="noopener noreferrer" class="footer-link">
              GitHub
            </a>
            <a [href]="profile.linkedin" target="_blank" rel="noopener noreferrer" class="footer-link">
              LinkedIn
            </a>
            <a [href]="'mailto:' + profile.email" class="footer-link">
              Email
            </a>
          </div>
        </div>
      </footer>
    </div>
  `,
  styles: [`
    .app-shell {
      display: flex;
      flex-direction: column;
      min-height: 100vh;
    }

    .navbar-wrapper {
      position: sticky;
      top: 0;
      z-index: 50;
      background-color: var(--bg-primary);
      border-bottom: 1px solid var(--border-color);
      backdrop-filter: blur(8px);
      transition: background-color var(--transition-normal);
    }

    .nav-container {
      display: flex;
      align-items: center;
      justify-content: space-between;
      height: 70px;
    }

    .nav-brand {
      display: flex;
      align-items: center;
      gap: 0.75rem;
    }

    .brand-monogram {
      width: 38px;
      height: 38px;
      border-radius: 8px;
      background-color: var(--accent-tint);
      border: 1px solid rgba(59, 130, 246, 0.3);
      color: var(--accent-primary);
      display: flex;
      align-items: center;
      justify-content: center;
      font-weight: 700;
      font-family: var(--font-mono);
      font-size: 0.9rem;
    }

    .brand-text {
      display: flex;
      flex-direction: column;
    }

    .brand-name {
      font-size: 1rem;
      font-weight: 700;
      color: var(--text-primary);
      line-height: 1.2;
    }

    .brand-role {
      font-size: 0.75rem;
      color: var(--text-muted);
      font-family: var(--font-mono);
    }

    .desktop-nav {
      display: flex;
      gap: 1.75rem;
    }

    .nav-link {
      font-size: 0.9rem;
      font-weight: 500;
      color: var(--text-secondary);
      transition: color var(--transition-fast);
      position: relative;
      padding: 0.4rem 0;
    }

    .nav-link:hover {
      color: var(--text-primary);
    }

    .nav-link.active {
      color: var(--accent-primary);
      font-weight: 600;
    }

    .nav-link.active::after {
      content: '';
      position: absolute;
      bottom: 0;
      left: 0;
      right: 0;
      height: 2px;
      background-color: var(--accent-primary);
      border-radius: 2px;
    }

    .nav-actions {
      display: flex;
      align-items: center;
      gap: 0.75rem;
    }

    .theme-toggle-btn {
      padding: 0.4rem 0.85rem;
      border-radius: 8px;
      border: 1px solid var(--border-color);
      background-color: var(--bg-card);
      color: var(--text-primary);
      font-size: 0.8rem;
      font-family: var(--font-mono);
      transition: all var(--transition-fast);
      display: flex;
      align-items: center;
      gap: 0.35rem;
    }

    .theme-toggle-btn:hover {
      background-color: var(--bg-card-hover);
      border-color: var(--text-muted);
    }

    .mobile-menu-btn {
      display: none;
      flex-direction: column;
      gap: 5px;
      padding: 0.5rem;
    }

    .bar {
      width: 22px;
      height: 2px;
      background-color: var(--text-primary);
      transition: all var(--transition-fast);
    }

    .mobile-drawer {
      background-color: var(--bg-primary);
      border-bottom: 1px solid var(--border-color);
      padding: 1.5rem 0;
    }

    .mobile-nav-list {
      display: flex;
      flex-direction: column;
      gap: 1rem;
    }

    .mobile-nav-link {
      font-size: 1.05rem;
      font-weight: 500;
      color: var(--text-secondary);
      padding: 0.5rem 0;
    }

    .mobile-nav-link.active {
      color: var(--accent-primary);
      font-weight: 700;
    }

    .main-content {
      flex: 1;
    }

    .footer-wrapper {
      border-top: 1px solid var(--border-color);
      background-color: var(--bg-primary);
      padding: 2.5rem 0;
      margin-top: auto;
    }

    .footer-container {
      display: flex;
      justify-content: space-between;
      align-items: center;
      flex-wrap: wrap;
      gap: 1.5rem;
    }

    .footer-name {
      font-size: 0.9rem;
      font-weight: 600;
      color: var(--text-primary);
    }

    .footer-copy {
      font-size: 0.8rem;
      color: var(--text-muted);
      margin-top: 0.25rem;
    }

    .footer-right {
      display: flex;
      gap: 1.5rem;
    }

    .footer-link {
      font-size: 0.85rem;
      color: var(--text-secondary);
      transition: color var(--transition-fast);
    }

    .footer-link:hover {
      color: var(--accent-primary);
    }

    @media (max-width: 768px) {
      .desktop-nav {
        display: none;
      }

      .mobile-menu-btn {
        display: flex;
      }

      .footer-container {
        flex-direction: column;
        align-items: flex-start;
      }
    }
  `]
})
export class AppComponent {
  readonly themeService = inject(ThemeService);
  private readonly dataService = inject(PortfolioDataService);
  readonly profile = this.dataService.profile;

  mobileMenuOpen = false;

  toggleMobileMenu(): void {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  closeMobileMenu(): void {
    this.mobileMenuOpen = false;
  }
}
