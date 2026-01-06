// src/app/modules/auth/register/register.component.ts
import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent extends BaseComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthFacadeService);
  private router = inject(Router);

  hidePassword = true;
  hideConfirmPassword = true;
  successMessage = '';

  // Custom validator for password confirmation
  passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    
    if (password && confirmPassword && password !== confirmPassword) {
      control.get('confirmPassword')?.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    }
    return null;
  }

  form = this.fb.group({
    firstName: ['', [Validators.required, Validators.maxLength(50)]],
    lastName: ['', [Validators.required, Validators.maxLength(50)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [
      Validators.required,
      Validators.minLength(6),
      Validators.pattern(/[A-Z]/),
      Validators.pattern(/[a-z]/),
      Validators.pattern(/[0-9]/)
    ]],
    confirmPassword: ['', [Validators.required]],
    phoneNumber: [''],
    acceptTerms: [false, [Validators.requiredTrue]]
  }, { validators: this.passwordMatchValidator });

  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();
    this.errorMessage = '';
    this.successMessage = '';

    const payload = {
      firstName: this.form.value.firstName ?? '',
      lastName: this.form.value.lastName ?? '',
      email: this.form.value.email ?? '',
      password: this.form.value.password ?? '',
      confirmPassword: this.form.value.confirmPassword ?? '',
      phoneNumber: this.form.value.phoneNumber ?? ''
    };

    this.auth.register(payload).subscribe({
      next: (response: any) => {
        this.stopLoading();
        this.successMessage = response.message;
        
        // Auto login after registration (optional)
        setTimeout(() => {
          this.router.navigate(['/auth/login']);
        }, 3000);
      },
      error: (err: any) => {
        this.stopLoading(err.error?.message || 'Greška pri registraciji. Molimo pokušajte ponovo.');
      }
    });
  }
}