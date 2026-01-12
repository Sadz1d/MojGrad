// src/app/modules/auth/forgot-password/forgot-password.component.ts
import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-forgot-password',
  standalone: false,
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
})
export class ForgotPasswordComponent extends BaseComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthFacadeService);

  isEmailSent = false;
  successMessage = '';

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
  });

  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();
    this.errorMessage = '';
    this.successMessage = '';

    const email = this.form.value.email ?? '';

    this.auth.forgotPassword(email).subscribe({
      next: (response: any) => {
        this.stopLoading();
        this.isEmailSent = true;
        this.successMessage =
          response?.message ||
          'Ako email postoji u sistemu, poslan je link za reset lozinke.';
      },
      error: (err: any) => {
        this.stopLoading(
          err?.error?.message ||
          'Greška pri slanju emaila. Molimo pokušajte ponovo.'
        );
      },
    });
  }

  resendEmail(): void {
    this.onSubmit();
  }
}
