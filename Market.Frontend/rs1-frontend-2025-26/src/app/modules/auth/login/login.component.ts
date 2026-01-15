// src/app/modules/auth/login/login.component.ts
import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent extends BaseComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthFacadeService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  hidePassword = true;

  form = this.fb.group({
    email: ['admin@mojgrad.local', [Validators.required, Validators.email]],
    password: ['Admin123!', [Validators.required]],
    rememberMe: [false],
  });

  ngOnInit(): void {
    if (this.auth.isLoggedIn()) {
      this.router.navigate(['/admin/products']);
    }
  }

  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();

    const payload = {
      email: this.form.value.email ?? '',
      password: this.form.value.password ?? ''
      // ❌ fingerprint ne šaljemo ovdje, AuthFacadeService će ga dodati automatski
    };

    this.auth.login(payload).subscribe({
      next: (res) => {
        console.log('LOGIN RESPONSE:', res);

        this.stopLoading();
        const returnUrl =
          this.route.snapshot.queryParams['returnUrl'] || '/admin/products';
        this.router.navigate([returnUrl]);
      },
      error: (err) => {
        this.stopLoading(err.error?.message || 'Neispravni kredencijali.');
        console.error('Login error:', err);
      },
    });

  }
}
