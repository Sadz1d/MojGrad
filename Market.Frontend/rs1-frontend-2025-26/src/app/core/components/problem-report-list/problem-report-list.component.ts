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
import { ExportService } from '../../services/export.service';
import { MatDivider } from "@angular/material/divider";
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-problem-report-list',
  templateUrl: './problem-report-list.component.html',
  styleUrls: ['./problem-report-list.component.scss'],
  standalone: true,
  imports: [
    MatMenuModule,
    MatProgressSpinnerModule,
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatIcon,
    MatProgressSpinner,
    MatDivider
],
})
export class ProblemReportListComponent implements OnInit {
  reports: ProblemReportListItem[] = [];
  loading = false;
  error = '';
  exporting = false;
  
  filterForm: FormGroup;
  
  currentPage = 1;
  pageSize = 10;
  totalItems = 0;
  totalPages = 0;

  constructor(
    private problemReportService: ProblemReportService,
    private exportService: ExportService,
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
  /**
   * Exportuje trenutno prikazane podatke u Excel
   */
  exportData(): void {
    if (this.reports.length === 0) {
      alert('Nema podataka za export!');
      return;
    }

    this.exporting = true;

    try {
      // Pripremi podatke za export
      const exportData = this.prepareExportData();
      
      // Exportuj u Excel
      this.exportService.exportToExcel(exportData, {
        fileName: `problem-reports-${this.getCurrentDate()}.xlsx`,
        sheetName: 'Problem Reports',
        dateFormat: 'DD.MM.YYYY'
      });
      
      console.log('Export uspješan!');
      
    } catch (error) {
      console.error('Greška pri exportu:', error);
      alert('Došlo je do greške pri exportu podataka.');
      
    } finally {
      this.exporting = false;
    }
  }

  /**
   * Exportuje sve podatke (sa svim filterima) u Excel
   */
  exportAllData(): void {
    if (!confirm('Da li želite da exportujete SVE podatke (može potrajati)?')) {
      return;
    }

    this.exporting = true;

    // Uzmi sve podatke bez paginacije
    const filter: ProblemReportFilter = {
      ...this.filterForm.value,
      page: 1,
      pageSize: 10000 // Veliki broj da dobijemo sve
    };

    this.problemReportService.getReports(filter).subscribe({
      next: (response) => {
        try {
          const exportData = this.prepareExportData(response.items);
          
          this.exportService.exportToExcel(exportData, {
            fileName: `problem-reports-all-${this.getCurrentDate()}.xlsx`,
            sheetName: 'Svi Problem Reports',
            dateFormat: 'DD.MM.YYYY'
          });
          
          console.log('Export svih podataka uspješan!');
          
        } catch (error) {
          console.error('Greška pri exportu:', error);
          alert('Došlo je do greške pri exportu podataka.');
          
        } finally {
          this.exporting = false;
        }
      },
      error: (err) => {
        this.exporting = false;
        alert('Greška pri dobijanju podataka za export: ' + err.message);
      }
    });
  }

  /**
   * Exportuje podatke u CSV format
   */
  exportToCSV(): void {
    if (this.reports.length === 0) {
      alert('Nema podataka za export!');
      return;
    }

    this.exporting = true;

    try {
      const exportData = this.prepareExportData();
      
      this.exportService.exportToCSV(
        exportData,
        `problem-reports-${this.getCurrentDate()}.csv`,
        ';' // Koristi ; kao delimiter za Excel u našem regionu
      );
      
      console.log('CSV export uspješan!');
      
    } catch (error) {
      console.error('Greška pri CSV exportu:', error);
      alert('Došlo je do greške pri CSV exportu.');
      
    } finally {
      this.exporting = false;
    }
  }

  /**
   * Exportuje advanced report sa više sheetova
   */
  exportAdvancedReport(): void {
    if (this.reports.length === 0) {
      alert('Nema podataka za export!');
      return;
    }

    this.exporting = true;

    try {
      const exportData = [
        {
          sheetName: 'Prijave',
          data: this.prepareExportData(),
          columns: ['ID', 'Naslov', 'Autor', 'Kategorija', 'Status', 'Lokacija', 'Datum', 'Komentari', 'Opis'],
          columnWidths: [10, 40, 20, 15, 15, 25, 15, 12, 50]
        },
        {
          sheetName: 'Statistika',
          data: this.generateStatistics(),
          columns: ['Kategorija', 'Broj Prijava', 'Prosječan Status'],
          columnWidths: [30, 15, 20]
        },
        {
          sheetName: 'Filteri',
          data: [this.getCurrentFilters()],
          columns: ['Pretraga', 'User ID', 'Category ID', 'Status ID', 'Stranica'],
          columnWidths: [30, 15, 15, 15, 15]
        }
      ];

      this.exportService.exportAdvanced(
        exportData,
        `problem-reports-advanced-${this.getCurrentDate()}.xlsx`
      );
      
      console.log('Advanced export uspješan!');
      
    } catch (error) {
      console.error('Greška pri advanced exportu:', error);
      alert('Došlo je do greške pri advanced exportu.');
      
    } finally {
      this.exporting = false;
    }
  }

  // === PRIVATE HELPER METODE ===

  /**
   * Priprema podatke za export
   */
  private prepareExportData(items: ProblemReportListItem[] = this.reports): any[] {
    return items.map(report => ({
      'ID': report.id,
      'Naslov': report.title,
      'Autor': report.authorName,
      'Kategorija': report.categoryName,
      'Status': report.statusName,
      'Lokacija': report.location || 'N/A',
      'Datum': new Date(report.creationDate),
      'Komentari': report.commentsCount,
      'Zadaci': report.tasksCount,
      'Ocjene': report.ratingsCount,
      'Opis': report.shortDescription || '',
      'URL Detalja': `${window.location.origin}/problem-reports/${report.id}`
    }));
  }

  /**
   * Generiše statistiku za advanced report
   */
  private generateStatistics(): any[] {
    const categoryStats = new Map<string, { count: number, statuses: string[] }>();
    
    this.reports.forEach(report => {
      const key = report.categoryName;
      if (!categoryStats.has(key)) {
        categoryStats.set(key, { count: 0, statuses: [] });
      }
      
      const stat = categoryStats.get(key)!;
      stat.count++;
      stat.statuses.push(report.statusName);
    });

    return Array.from(categoryStats.entries()).map(([category, stat]) => ({
      'Kategorija': category,
      'Broj Prijava': stat.count,
      'Prosječan Status': this.calculateAverageStatus(stat.statuses)
    }));
  }

  /**
   * Vraća trenutne filtere
   */
  private getCurrentFilters(): any {
    const formValue = this.filterForm.value;
    return {
      'Pretraga': formValue.search || 'Sve',
      'User ID': formValue.userId || 'Svi',
      'Category ID': formValue.categoryId || 'Sve',
      'Status ID': formValue.statusId || 'Svi',
      'Stranica': this.currentPage,
      'Veličina Stranice': this.pageSize
    };
  }

  /**
   * Računa prosječan status
   */
  private calculateAverageStatus(statuses: string[]): string {
    const statusValues: { [key: string]: number } = {
      'novo': 1,
      'u toku': 2,
      'rešen': 3,
      'riješeno': 3,
      'odložen': 4
    };

    const total = statuses.reduce((sum, status) => {
      const lowerStatus = status.toLowerCase();
      return sum + (statusValues[lowerStatus] || 1);
    }, 0);

    const avg = total / statuses.length;

    if (avg <= 1.5) return 'Novo';
    if (avg <= 2.5) return 'U toku';
    return 'Rešeno/Riješeno';
  }

  /**
   * Vraća trenutni datum za ime fajla
   */
  public getCurrentDate(): string {
    const now = new Date();
    const day = now.getDate().toString().padStart(2, '0');
    const month = (now.getMonth() + 1).toString().padStart(2, '0');
    const year = now.getFullYear();
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');
    
    return `${year}-${month}-${day}_${hours}-${minutes}`;
  }

}