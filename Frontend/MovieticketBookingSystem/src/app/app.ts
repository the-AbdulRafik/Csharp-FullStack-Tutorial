import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from './auth.service';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly authService = inject(AuthService);
  protected readonly isLoginMode = signal(true);
  protected readonly loading = signal(false);
  protected readonly message = signal<{ kind: 'success' | 'error'; text: string } | null>(null);
  private readonly formBuilder = new FormBuilder();

  protected readonly form = this.formBuilder.group({
    fullName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(150)]],
    password: ['', [
      Validators.required,
      Validators.minLength(8),
      Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).+$/),
    ]],
  });

  protected setMode(mode: 'login' | 'register'): void {
    this.isLoginMode.set(mode === 'login');
    this.message.set(null);
    this.form.reset();
  }

  protected hasError(controlName: string): boolean {
    const control = this.form.get(controlName);
    return Boolean(control?.invalid && (control.dirty || control.touched));
  }

  protected submit(): void {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      this.message.set({ kind: 'error', text: 'Please correct the highlighted fields.' });
      return;
    }

    this.loading.set(true);
    this.message.set(null);
    const value = this.form.getRawValue();
    const request = this.isLoginMode()
      ? this.authService.login({ email: value.email ?? '', password: value.password ?? '' })
      : this.authService.register({
          fullName: value.fullName ?? '',
          email: value.email ?? '',
          password: value.password ?? '',
        });

    request.subscribe({
      next: (response) => {
        this.loading.set(false);
        this.message.set({ kind: 'success', text: response.message });
        if (!this.isLoginMode()) {
          this.setMode('login');
          this.message.set({ kind: 'success', text: 'Registration successful. Sign in to continue.' });
        }
      },
      error: (error: Error) => {
        this.loading.set(false);
        this.message.set({ kind: 'error', text: error.message });
      },
    });
  }

  protected logout(): void {
    this.authService.logout();
    this.message.set({ kind: 'success', text: 'You have been signed out.' });
  }
}
