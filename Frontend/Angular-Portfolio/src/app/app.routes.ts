import { Routes } from '@angular/router';
import { projectExistsGuard } from './core/guards/project-exists.guard';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadComponent: () =>
      import('./features/home/home.component').then((m) => m.HomeComponent),
    title: 'Abdul Rafik S | AI Backend Engineer'
  },
  {
    path: 'about',
    loadComponent: () =>
      import('./features/about/about.component').then((m) => m.AboutComponent),
    title: 'About | Abdul Rafik S',
    children: [
      {
        path: '',
        pathMatch: 'full',
        loadComponent: () =>
          import('./features/about/about-overview.component').then(
            (m) => m.AboutOverviewComponent
          )
      },
      {
        path: 'achievements',
        loadComponent: () =>
          import('./features/about/about-achievements.component').then(
            (m) => m.AboutAchievementsComponent
          ),
        title: 'Achievements | Abdul Rafik S'
      }
    ]
  },
  {
    path: 'projects',
    loadComponent: () =>
      import('./features/projects/projects.component').then(
        (m) => m.ProjectsComponent
      ),
    title: 'Projects | Abdul Rafik S'
  },
  {
    path: 'projects/:id',
    canActivate: [projectExistsGuard],
    loadComponent: () =>
      import('./features/projects/project-detail.component').then(
        (m) => m.ProjectDetailComponent
      ),
    title: 'Project Details | Abdul Rafik S'
  },
  {
    path: 'contact',
    loadComponent: () =>
      import('./features/contact/contact.component').then(
        (m) => m.ContactComponent
      ),
    title: 'Contact | Abdul Rafik S'
  },
  {
    path: '404',
    loadComponent: () =>
      import('./features/not-found/not-found.component').then(
        (m) => m.NotFoundComponent
      ),
    title: '404 Not Found | Abdul Rafik S'
  },
  {
    path: '**',
    redirectTo: '404'
  }
];
