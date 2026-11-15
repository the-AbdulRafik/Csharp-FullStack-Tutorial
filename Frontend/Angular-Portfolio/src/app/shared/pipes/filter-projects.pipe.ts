import { Pipe, PipeTransform } from '@angular/core';
import { Project } from '../../models/portfolio.models';

@Pipe({
  name: 'filterProjects',
  standalone: false
})
export class FilterProjectsPipe implements PipeTransform {
  transform(projects: Project[] | null, category: string = 'All', query: string = ''): Project[] {
    if (!projects) {
      return [];
    }

    const q = query.trim().toLowerCase();
    const cat = category.toLowerCase();

    return projects.filter((item) => {
      const matchCat = cat === 'all' || item.category.toLowerCase() === cat;
      const matchQuery =
        !q ||
        item.title.toLowerCase().includes(q) ||
        item.technologies.some((t) => t.toLowerCase().includes(q)) ||
        item.summary.toLowerCase().includes(q);
      return matchCat && matchQuery;
    });
  }
}
