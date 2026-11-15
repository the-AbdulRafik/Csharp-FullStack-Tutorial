import { describe, it, expect, beforeEach } from 'vitest';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { AppComponent } from './app.component';
import { ThemeService } from './core/services/theme.service';
import { ProjectService } from './core/services/project.service';
import { PortfolioDataService } from './core/services/portfolio-data.service';
import { TruncatePipe } from './shared/pipes/truncate.pipe';
import { provideHttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

describe('AngularPortfolio Smoke and Unit Tests', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [
        provideRouter([]),
        provideHttpClient(),
        ThemeService,
        ProjectService,
        PortfolioDataService
      ]
    }).compileComponents();
  });

  it('should create the AppComponent', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render brand name Abdul Rafik S in the navbar', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.brand-name')?.textContent).toContain('Abdul Rafik S');
  });

  it('ThemeService should toggle between dark and light themes', () => {
    const themeService = TestBed.inject(ThemeService);
    themeService.setTheme('dark');
    expect(themeService.currentTheme()).toBe('dark');
    themeService.toggleTheme();
    expect(themeService.currentTheme()).toBe('light');
  });

  it('TruncatePipe should truncate long text with trail', () => {
    const pipe = new TruncatePipe();
    const input = 'This is a long sentence explaining autonomous agent workflows.';
    const result = pipe.transform(input, 20);
    expect(result).toBe('This is a long sente...');
  });

  it('ProjectService should filter projects by category', async () => {
    const projectService = TestBed.inject(ProjectService);
    const projects = await firstValueFrom(projectService.filterProjects('AI & Agents', ''));
    expect(projects.length).toBeGreaterThan(0);
    expect(projects[0].category).toBe('AI & Agents');
  });
});
