import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class LoginComponent {

  loginForm = new FormGroup({
    email: new FormControl('admin@acts.com', {
      nonNullable: true,
      validators: [Validators.required, Validators.email]
    }),
    password: new FormControl('acts-password', {
      nonNullable: true,
      validators: [Validators.required]
    })
  });

  errorMessage = '';
  isLoading = false;

  constructor(private router: Router) {}

  onSubmit(): void {
    if (this.isLoading) return;

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    setTimeout(() => {
      const email = this.loginForm.controls.email.value;
      const password = this.loginForm.controls.password.value;

      if (email === 'admin@acts.com' && password === 'acts-password') {
        localStorage.setItem('acts-mock-auth', 'true');
        this.isLoading = false;
        this.router.navigate(['/admin']);
      } else {
        this.isLoading = false;
        this.errorMessage = 'Invalid email or password';
      }
    }, 800);
  }
}