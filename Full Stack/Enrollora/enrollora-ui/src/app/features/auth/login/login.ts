import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/auth/auth.service';
import { LoginRequest } from '../../../core/auth/auth.models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  
  private authService = inject(AuthService);
  private router = inject(Router);
  private formBuilder = inject(FormBuilder);

  loginForm = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  isLoading = false;
  errorMessage = '';

  onLogin(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    const loginRequest = this.loginForm.getRawValue() as LoginRequest;

    this.authService.login(loginRequest).subscribe({
      next: (response) => {
        this.isLoading = false;
        if(response.role === 'Administrator') {
          this.router.navigate(['/admin/dashboard']);
        } else if(response.role === 'Student') {
          this.router.navigate(['/student/dashboard']);
        } 
      },

      error: (error) => {
        console.error('Login failed:', error);

        this.errorMessage =
          error.error?.detail || error.error?.message || 'Something went wrong. Please try again.';

        this.isLoading = false;
      },
    });
  }
}
