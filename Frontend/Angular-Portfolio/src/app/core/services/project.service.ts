import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { Project } from '../../models/portfolio.models';
import { PortfolioDataService } from './portfolio-data.service';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private readonly dataService = inject(PortfolioDataService);

  getAllProjects(): Observable<Project[]> {
    return this.dataService.getProjects();
  }

  getProjectById(id: string): Observable<Project | undefined> {
    return this.dataService.getProjectById(id);
  }

  filterProjects(category: string, query: string): Observable<Project[]> {
    return this.dataService.getProjects().pipe(
      map((projects) =>
        projects.filter((p) => {
          const matchesCategory =
            category === 'All' || p.category.toLowerCase() === category.toLowerCase();
          const q = query.trim().toLowerCase();
          const matchesQuery =
            !q ||
            p.title.toLowerCase().includes(q) ||
            p.technologies.some((t) => t.toLowerCase().includes(q)) ||
            p.summary.toLowerCase().includes(q);
          return matchesCategory && matchesQuery;
        })
      )
    );
  }
}
