// src/app/modules/auth/logout/logout.component.ts
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-logout',
  standalone: false,
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.scss',
})
export class LogoutComponent implements OnInit {
  constructor(
    private auth: AuthFacadeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.performLogout();
  }

  private performLogout(): void {
    this.auth.logout().subscribe({
      next: () => {
        // Already navigated in service
      },
      error: () => {
        // Still navigate to login even on error
        this.router.navigate(['/auth/login']);
      }
    });
  }
}