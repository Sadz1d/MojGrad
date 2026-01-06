// components/problem-report-list/problem-report-list.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { ProblemReportService } from '../../services/problem-report.service';
import { ProblemReportListItem, ProblemReportFilter } from '../../models/problem-report.model';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIcon } from "@angular/material/icon";
import { MatProgressSpinner } from "@angular/material/progress-spinner";

@Component({
  selector: 'app-problem-report-list',
  templateUrl: './problem-report-list.component.html',
  styleUrls: ['./problem-report-list.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatIcon,
    MatProgressSpinner
],
})
export class ProblemReportListComponent implements OnInit {
  reports: ProblemReportListItem[] = [];
  loading = false;
  error = '';
  
  filterForm: FormGroup;
  
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;

  constructor(
    private problemReportService: ProblemReportService,
    private fb: FormBuilder,
    public router: Router
  ) {
    this.filterForm = this.fb.group({
      search: [''],
      userId: [null],
      categoryId: [null],
      statusId: [null]
    });
  }

  ngOnInit(): void {
    this.loadReports();
  }

  loadReports(): void {
    this.loading = true;
    this.error = '';
    
    const filter: ProblemReportFilter = {
      ...this.filterForm.value,
      page: this.currentPage,
      pageSize: this.pageSize
    };

    this.problemReportService.getReports(filter).subscribe({
      next: (response) => {
        this.reports = response.items;
        this.totalItems = response.totalCount;
        this.totalPages = response.totalPages;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška pri učitavanju podataka: ' + err.message;
        this.loading = false;
      }
    });
  }

  applyFilter(): void {
    this.currentPage = 1;
    this.loadReports();
  }

  resetFilter(): void {
    this.filterForm.reset();
    this.currentPage = 1;
    this.loadReports();
  }

  onPageChange(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadReports();
  }

  deleteReport(id: number): void {
    if (confirm('Da li ste sigurni da želite da obrišete ovaj izveštaj?')) {
      this.problemReportService.deleteReport(id).subscribe({
        next: () => {
          this.reports = this.reports.filter(r => r.id !== id);
          if (this.reports.length === 0 && this.currentPage > 1) {
            this.currentPage--;
          }
          this.loadReports();
        },
        error: (err) => {
          alert('Greška pri brisanju: ' + err.message);
        }
      });
    }
  }

  viewReport(id: number): void {
    this.router.navigate(['/problem-reports', id]);
  }

  editReport(id: number): void {
    this.router.navigate(['/problem-reports/edit', id]);
  }
  getPageNumbers(): number[] {
  const pages: number[] = [];
  const maxVisiblePages = 5;
  
  let startPage = Math.max(1, this.currentPage - Math.floor(maxVisiblePages / 2));
  let endPage = Math.min(this.totalPages, startPage + maxVisiblePages - 1);
  
  if (endPage - startPage + 1 < maxVisiblePages) {
    startPage = Math.max(1, endPage - maxVisiblePages + 1);
  }
  
  for (let i = startPage; i <= endPage; i++) {
    pages.push(i);
  }
  
  return pages;
  }
  getStatusClass(statusName: string): string {
    const status = statusName.toLowerCase();
    if (status.includes('nov')) return 'status-new';
    if (status.includes('toku') || status.includes('u toku')) return 'status-progress';
    if (status.includes('rešen') || status.includes('riješen')) return 'status-done';
    return 'status-new';
  }
  exportData(): void {
    console.log('Export functionality would go here');
    // Implementacija exporta u CSV/Excel
  }
}