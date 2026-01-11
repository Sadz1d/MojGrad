import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-reset-password',
  standalone: false,
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss',
})
export class ResetPasswordComponent extends BaseComponent {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private auth = inject(AuthFacadeService);

  token = '';
  successMessage = '';

  form = this.fb.group({
    newPassword: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', Validators.required],
  });

  ngOnInit(): void {
    this.token = this.route.snapshot.queryParamMap.get('token') ?? '';

    if (!this.token) {
      this.errorMessage = 'Nevažeći ili istekao link za reset lozinke.';
    }
  }

  onSubmit(): void {
    if (this.form.invalid || this.isLoading || !this.token) return;

    const { newPassword, confirmPassword } = this.form.value;

    if (newPassword !== confirmPassword) {
      this.errorMessage = 'Lozinke se ne podudaraju.';
      return;
    }

    this.startLoading();
    this.errorMessage = '';

    this.auth.resetPassword(this.token, newPassword!).subscribe({
      next: () => {
        this.stopLoading();
        this.successMessage = 'Lozinka je uspješno promijenjena.';
        setTimeout(() => this.router.navigate(['/login']), 2500);
      },
      error: () => {
        this.stopLoading(
          'Došlo je do greške prilikom promjene lozinke.'
        );
      },
    });
  }
}
