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

  // Custom validator za potvrdu lozinke
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
      Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$/)
    ]],
    confirmPassword: ['', [Validators.required]],
    phoneNumber: [''],
    acceptTerms: [false, [Validators.requiredTrue]]
  }, { validators: this.passwordMatchValidator });

  get passwordErrors() {
    const password = this.form.get('password');
    if (!password || !password.errors) return '';
    
    if (password.errors['required']) return 'Lozinka je obavezna';
    if (password.errors['minlength']) return 'Lozinka mora imati najmanje 6 karaktera';
    if (password.errors['pattern']) return 'Lozinka mora sadržati veliko slovo, malo slovo i cifru';
    return '';
  }

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
      phoneNumber: this.form.value.phoneNumber || undefined
    };

    console.log('Register payload:', payload);

    this.auth.register(payload).subscribe({
      next: (response: any) => {
        this.stopLoading();
        this.successMessage = response.message || 'Uspešno ste registrovani! Sada se možete prijaviti.';
        
        // Preusmjeri na login nakon 3 sekunde
        setTimeout(() => {
          this.router.navigate(['/auth/login']);
        }, 3000);
      },
      error: (err: any) => {
        this.stopLoading(
          err.error?.message || 
          err.error?.errors?.Email?.[0] || 
          err.error?.errors?.Password?.[0] || 
          'Greška pri registraciji. Molimo pokušajte ponovo.'
        );
        console.error('Register error:', err);
      }
    });
  }
}