// src/app/modules/public/public-layout/public-layout.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { ProblemReportService } from '../../../core/services/problem-report.service';
import { ProblemReportListItem } from '../../../core/models/problem-report.model';

@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.scss',
})
export class PublicLayoutComponent implements OnInit {
  private auth = inject(AuthFacadeService);
  private problemReportService = inject(ProblemReportService);

  currentYear: string = '2025';
  isLoggedIn$ = this.auth.isAuthenticated$;
  newestReports: ProblemReportListItem[] = [];

  readonly apiBase = 'https://localhost:7260';

  ngOnInit(): void {
    this.problemReportService.getReports({
      page: 1,
      pageSize: 4,
      sortBy: 'creationdate',
      sortDirection: 'desc'
    }).subscribe({
      next: (result) => {
        this.newestReports = result.items ?? [];
      },
      error: () => {
        this.newestReports = [];
      }
    });
  }

  getImageUrl(imagePath?: string): string | null {
    if (!imagePath) return null;
    return `${this.apiBase}${imagePath}`;
  }

  getStatusClass(status: string): string {
    const s = status?.toLowerCase();
    if (s === 'novo' || s === 'new') return 'status-new';
    if (s === 'u toku' || s === 'in progress') return 'status-progress';
    if (s === 'riješeno' || s === 'resolved' || s === 'done') return 'status-done';
    return 'status-new';
  }

  getStatusLabel(status: string): string {
    const s = status?.toLowerCase();
    if (s === 'novo' || s === 'new') return 'NOVO';
    if (s === 'u toku' || s === 'in progress') return 'U TOKU';
    if (s === 'riješeno' || s === 'resolved' || s === 'done') return 'RIJEŠENO';
    return status?.toUpperCase() ?? 'NOVO';
  }

  logout(): void {
    this.auth.logout().subscribe();
  }

  get currentUser() {
    return this.auth.getCurrentUserValue();
  }

  get isAdmin(): boolean {
    return this.auth.isAdmin();
  }
}
