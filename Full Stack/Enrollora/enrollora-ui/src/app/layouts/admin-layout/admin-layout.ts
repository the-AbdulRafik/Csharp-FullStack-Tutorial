import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';
import { AsyncPipe } from '@angular/common';
import { TokenService } from '../../core/auth/token.service';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, AsyncPipe],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css'
})
export class AdminLayout {

  private authService = inject(AuthService);
  private router = inject(Router);
  private tokenService = inject(TokenService);

  readonly displayName$ = this.tokenService.name$;

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}