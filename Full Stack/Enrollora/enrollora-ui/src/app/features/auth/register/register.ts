import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { AuthService } from '../../../core/auth/auth.service';
import { RegisterStudentRequest } from '../../../core/auth/auth.models';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private authService = inject(AuthService);

  private router = inject(Router);
  private formBuilder = inject(FormBuilder);

  registerForm = this.formBuilder.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(120)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(256)]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(72)]],
    department: ['', [Validators.required, Validators.maxLength(100)]],
    age: [17, [Validators.required, Validators.min(17), Validators.max(60)]],
    cgpa: [0, [Validators.required, Validators.min(0), Validators.max(10)]],
  });

  isLoading = false;
  successMessage = '';
  errorMessage = '';

  onRegister(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.successMessage = '';
    this.errorMessage = '';
    const registerRequest = this.registerForm.getRawValue() as RegisterStudentRequest;

    this.authService.register(registerRequest).subscribe({
      next: (response) => {
        this.successMessage =
          'Registration successful! Your account is pending administrator approval. You can log in once your account is approved.';

        this.isLoading = false;

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 4000);
      },
      error: (error) => {
        this.errorMessage =
          error.error?.detail || error.error?.message || 'Registration failed. Please try again.';

        this.isLoading = false;

        console.error('Registration failed:', error);
      },
    });
  }
}
