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

// ENUM za sortiranje
enum SortDirection {
  ASC = 'asc',
  DESC = 'desc',
  NONE = ''
}

interface SortState {
  column: string;
  direction: SortDirection;
}

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

  // Sortiranje
  sortState: SortState = {
    column: 'id',
    direction: SortDirection.DESC
  };

  // Dostupne kolone za sortiranje
  sortableColumns = [
    { key: 'id', label: 'ID' },
    { key: 'title', label: 'Naslov' },
    { key: 'authorName', label: 'Autor' },
    { key: 'categoryName', label: 'Kategorija' },
    { key: 'statusName', label: 'Status' },
    { key: 'location', label: 'Lokacija' },
    { key: 'creationDate', label: 'Datum' },
    { key: 'commentsCount', label: 'Komentari' }
  ];

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
      statusId: [null],
      sortBy: ['id'],
      sortDirection: ['desc']
    });
  }

  ngOnInit(): void {
    this.loadReports();
  }
originalReports: ProblemReportListItem[] = [];
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
      // Sačuvaj originalne podatke
      this.originalReports = [...response.items];
      
      // Postavi trenutne podatke
      this.reports = this.originalReports;
      
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

  // === SORTIRANJE ===

  /**
   * Sortira po koloni
   */
  sortBy(column: string): void {
  console.log(`Sorting by ${column}, current direction: ${this.sortState.direction}`);
  
  // Ako već sortiramo po ovoj koloni, promijeni direction
  if (this.sortState.column === column) {
    switch (this.sortState.direction) {
      case SortDirection.ASC:
        this.sortState.direction = SortDirection.DESC;
        break;
      case SortDirection.DESC:
        this.sortState.direction = SortDirection.NONE;
        break;
      case SortDirection.NONE:
        this.sortState.direction = SortDirection.ASC;
        break;
    }
  } else {
    // Nova kolona, kreni sa DESC
    this.sortState.column = column;
    this.sortState.direction = SortDirection.DESC;
  }
  
  console.log(`New direction: ${this.sortState.direction}`);
  
  // Primijeni sortiranje bez reloada
  this.applySorting();
}
private applySorting(): void {
  if (this.sortState.direction === SortDirection.NONE) {
    // Vrati na originalni redoslijed BEZ reloada
    this.reports = [...this.originalReports];
    console.log('Reset to original order');
  } else {
    // Sortiraj trenutne podatke
    const sorted = this.problemReportService.sortReportsLocally(
      [...this.reports], // Napravi kopiju
      this.sortState.column,
      this.sortState.direction
    );
    this.reports = sorted;
    console.log('Applied sorting');
  }
}
private sortLocal(): void {
  if (this.sortState.direction === SortDirection.NONE) {
    // Ako je NONE, vrati se na originalni redoslijed
    // Ponovo učitaj podatke bez sortiranja
    this.currentPage = 1;
    this.loadReports();
    return;
  }
  // Sortiraj lokalno postojeće podatke
  this.reports = this.problemReportService.sortReportsLocally(
    this.reports,
    this.sortState.column,
    this.sortState.direction
  );}
  /**
   * Vraća sledeći direction za sortiranje
   */
  private getNextDirection(currentDirection: SortDirection): SortDirection {
    switch (currentDirection) {
      case SortDirection.ASC:
        return SortDirection.DESC;
      case SortDirection.DESC:
        return SortDirection.NONE;
      case SortDirection.NONE:
        return SortDirection.ASC;
      default:
        return SortDirection.DESC;
    }
  }

  /**
   * Vraća ikonicu za sortiranje
   */
  getSortIcon(column: string): string {
    if (this.sortState.column !== column) {
      return 'sort'; // Neutralna ikonica
    }

    switch (this.sortState.direction) {
      case SortDirection.ASC:
        return 'arrow_upward';
      case SortDirection.DESC:
        return 'arrow_downward';
      case SortDirection.NONE:
        return 'sort';
      default:
        return 'sort';
    }
  }

  /**
   * Vraća tooltip za sortiranje
   */
  getSortTooltip(column: string): string {
    if (this.sortState.column !== column) {
      return `Sortiraj po ${this.getColumnLabel(column)}`;
    }

    switch (this.sortState.direction) {
      case SortDirection.ASC:
        return `Sortirano A-Z po ${this.getColumnLabel(column)}`;
      case SortDirection.DESC:
        return `Sortirano Z-A po ${this.getColumnLabel(column)}`;
      case SortDirection.NONE:
        return `Ukloni sortiranje po ${this.getColumnLabel(column)}`;
      default:
        return `Sortiraj po ${this.getColumnLabel(column)}`;
    }
  }

  /**
   * Vraća label za kolonu
   */
  public getColumnLabel(column: string): string {
    const col = this.sortableColumns.find(c => c.key === column);
    return col ? col.label : column;
  }

  /**
   * Resetuje sortiranje
   */
  resetSort(): void {
    this.sortState = {
      column: 'id',
      direction: SortDirection.DESC
    };
    this.currentPage = 1;
    this.loadReports();
  }

  // === FILTERI ===

  applyFilter(): void {
    this.currentPage = 1;
    this.loadReports();
  }

  resetFilter(): void {
    this.filterForm.reset();
    this.resetSort(); // Resetuj i sortiranje
  }

  // === PAGINACIJA ===

  onPageChange(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadReports();
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

  // === AKCIJE ===

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

  getStatusClass(statusName: string): string {
    const status = statusName.toLowerCase();
    if (status.includes('nov')) return 'status-new';
    if (status.includes('toku') || status.includes('u toku')) return 'status-progress';
    if (status.includes('rešen') || status.includes('riješen')) return 'status-done';
    return 'status-new';
  }

  // === EXPORT ===

  exportData(): void {
    if (this.reports.length === 0) {
      alert('Nema podataka za export!');
      return;
    }

    this.exporting = true;

    try {
      const exportData = this.prepareExportData();
      
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

  exportAllData(): void {
    if (!confirm('Da li želite da exportujete SVE podatke (može potrajati)?')) {
      return;
    }

    this.exporting = true;

    const filter: ProblemReportFilter = {
      ...this.filterForm.value,
      page: 1,
      pageSize: 10000
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
        ';'
      );
      
      console.log('CSV export uspješan!');
      
    } catch (error) {
      console.error('Greška pri CSV exportu:', error);
      alert('Došlo je do greške pri CSV exportu.');
      
    } finally {
      this.exporting = false;
    }
  }

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
          columns: ['Pretraga', 'User ID', 'Category ID', 'Status ID', 'Stranica', 'Sortiraj po', 'Smjer'],
          columnWidths: [20, 10, 10, 10, 10, 15, 10]
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

  private getCurrentFilters(): any {
    const formValue = this.filterForm.value;
    return {
      'Pretraga': formValue.search || 'Sve',
      'User ID': formValue.userId || 'Svi',
      'Category ID': formValue.categoryId || 'Sve',
      'Status ID': formValue.statusId || 'Svi',
      'Stranica': this.currentPage,
      'Veličina Stranice': this.pageSize,
      'Sortiraj po': this.sortState.column,
      'Smjer': this.sortState.direction.toUpperCase()
    };
  }

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