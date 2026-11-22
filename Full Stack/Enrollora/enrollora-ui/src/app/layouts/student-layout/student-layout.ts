import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';
import { Router } from '@angular/router';
import { AsyncPipe } from '@angular/common';
import { TokenService } from '../../core/auth/token.service';

@Component({
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, AsyncPipe],
  selector: 'app-student-layout',
  styleUrl: './student-layout.css',
  templateUrl: './student-layout.html',
})
export class StudentLayout {
  private authService = inject(AuthService);
  private router = inject(Router);
  private tokenService = inject(TokenService);

  readonly displayName$ = this.tokenService.name$;

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
