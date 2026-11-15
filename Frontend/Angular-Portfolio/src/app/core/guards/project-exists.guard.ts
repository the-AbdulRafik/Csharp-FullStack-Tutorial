import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { ProjectService } from '../services/project.service';
import { map } from 'rxjs';

export const projectExistsGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const projectService = inject(ProjectService);
  const projectId = route.paramMap.get('id');

  if (!projectId) {
    return router.createUrlTree(['/projects']);
  }

  return projectService.getProjectById(projectId).pipe(
    map((project) => {
      if (project) {
        return true;
      }
      return router.createUrlTree(['/projects']);
    })
  );
};
