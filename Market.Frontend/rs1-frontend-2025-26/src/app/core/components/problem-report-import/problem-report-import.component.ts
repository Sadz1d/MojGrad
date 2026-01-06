// src/app/core/components/problem-report-import/problem-report-import.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ImportService, ImportResult, FileValidationResult } from '../../services/import.service';
import { MatIcon } from "@angular/material/icon";
import { MatProgressSpinner } from "@angular/material/progress-spinner";
import { MatCheckbox } from "@angular/material/checkbox";
import { CommonModule } from '@angular/common';
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-problem-report-import',
  templateUrl: './problem-report-import.component.html',
  styleUrls: ['./problem-report-import.component.scss'],
  standalone: true,
  imports: [CommonModule,MatIcon, MatProgressSpinner, MatCheckbox]
})
export class ProblemReportImportComponent implements OnInit {
  importForm: FormGroup;
  selectedFile: File | null = null;
  validationResult: FileValidationResult | null = null;
  importResult: ImportResult | null = null;
  
  loading = false;
  importing = false;
  progress = 0;
  
  isDragging = false;
  showPreview = false;
  importComplete = false;
  
  fileError: string | null = null;
  importError: string | null = null;

  constructor(
    private fb: FormBuilder,
    private importService: ImportService,
    private router: Router,
    private http: HttpClient
  ) {
    this.importForm = this.fb.group({
      file: [null, Validators.required],
      skipFirstRow: [true],
      dryRun: [false]
    });
  }
  private performRealImport(): void {
    if (!this.selectedFile) return;
    
    const formData = new FormData();
    formData.append('file', this.selectedFile);
    formData.append('skipFirstRow', this.importForm.get('skipFirstRow')?.value.toString());
    formData.append('dryRun', this.importForm.get('dryRun')?.value.toString());
    
    this.http.post<ImportResult>(
      `${environment.apiUrl}/api/reports/problem-reports/import`,
      formData,
      {
        reportProgress: true,
        observe: 'events'
      }
    ).subscribe({
      next: (event: HttpEvent<any>) => {
        if (event.type === HttpEventType.UploadProgress && event.total) {
          const progress = Math.round(100 * event.loaded / event.total);
          this.progress = progress;
        } else if (event.type === HttpEventType.Response) {
          this.importResult = event.body;
          this.importing = false;
          this.importComplete = true;
        }
      },
      error: (error) => {
        this.importError = error.error?.message || error.message;
        this.importing = false;
        this.importComplete = false;
      }
    });}
  ngOnInit(): void {
    this.importService.progress$.subscribe(progress => {
      this.progress = progress;
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.handleFile(file);
    }
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
    
    if (event.dataTransfer?.files.length) {
      const file = event.dataTransfer.files[0];
      this.handleFile(file);
    }
  }

  private handleFile(file: File): void {
    this.resetState();
    
    const validTypes = [
      'application/vnd.ms-excel',
      'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      'text/csv',
      '.xls',
      '.xlsx',
      '.csv'
    ];
    
    const isValidType = validTypes.some(type => 
      file.type.includes(type) || file.name.toLowerCase().endsWith(type)
    );
    
    if (!isValidType) {
      this.fileError = 'Podržani formati: Excel (.xls, .xlsx) ili CSV (.csv)';
      return;
    }
    
    if (file.size > 10 * 1024 * 1024) {
      this.fileError = 'Fajl je prevelik. Maksimalna veličina je 10MB.';
      return;
    }
    
    this.selectedFile = file;
    this.validateFile();
  }

  validateFile(): void {
    if (!this.selectedFile) {
      this.fileError = 'Nije odabran fajl';
      return;
    }
    
    this.loading = true;
    this.fileError = null;
    
    this.importService.validateFile(this.selectedFile).then(
      (result) => {
        this.validationResult = result;
        this.loading = false;
        
        if (!result.isValid) {
          this.fileError = result.errors.join(', ');
        }
      },
      (error) => {
        this.fileError = 'Greška pri validaciji fajla: ' + error.message;
        this.validationResult = null;
        this.loading = false;
      }
    );
  }

  startImport(): void {
    if (!this.selectedFile || !this.validationResult?.isValid) {
      this.fileError = 'Molimo odaberite validan fajl prije importa';
      return;
    }

    this.importing = true;
    this.importError = null;
    this.importResult = null;
    
    const dryRun = this.importForm.get('dryRun')?.value;
    
    if (dryRun) {
      this.performDryRun();
    } else {
      this.performImport();
    }
  }

  private performImport(): void {
    if (!this.selectedFile) return;
    
    this.importService.importWithProgress(this.selectedFile).subscribe({
      next: (result) => {
        this.importResult = result;
        this.importing = false;
        this.importComplete = true;
        
        if (result.failed > 0) {
          this.importError = `${result.failed} od ${result.totalRecords} zapisa nije importovano.`;
        }
      },
      error: (error) => {
        this.importError = error.message;
        this.importing = false;
        this.importComplete = false;
      }
    });
  }

  private performDryRun(): void {
    if (!this.selectedFile) return;
    
    this.importService.parseFile(this.selectedFile).then(data => {
      setTimeout(() => {
        this.importResult = {
          totalRecords: data.length,
          successful: Math.max(0, data.length - 2),
          failed: Math.min(2, data.length),
          errors: ['Red 5: Nevalidan categoryId', 'Red 8: Title je predug'],
          importedIds: []
        };
        
        this.importing = false;
        this.importComplete = true;
        
        if (this.importResult.failed > 0) {
          this.importError = `DRY RUN: ${this.importResult.failed} zapisa bi failovalo.`;
        }
      }, 1500);
    }).catch(error => {
      this.importError = error.message;
      this.importing = false;
      this.importComplete = false;
    });
  }

  downloadTemplate(): void {
    this.importService.downloadTemplate();
  }

  goToList(): void {
    this.router.navigate(['/problem-reports']);
  }

  importAnother(): void {
    this.resetState();
  }

  public resetState(): void {
    this.selectedFile = null;
    this.validationResult = null;
    this.importResult = null;
    this.fileError = null;
    this.importError = null;
    this.importComplete = false;
    this.showPreview = false;
    this.progress = 0;
    this.importForm.patchValue({ file: null });
  }

  getFileIcon(): string {
    if (!this.selectedFile) return 'insert_drive_file';
    
    const name = this.selectedFile.name.toLowerCase();
    if (name.endsWith('.xlsx') || name.endsWith('.xls')) return 'table_chart';
    if (name.endsWith('.csv')) return 'grid_on';
    return 'description';
  }

 public getFileSize(): string {
    if (!this.selectedFile) return '';
    
    const size = this.selectedFile.size;
    if (size < 1024) return size + ' B';
    if (size < 1024 * 1024) return (size / 1024).toFixed(1) + ' KB';
    return (size / (1024 * 1024)).toFixed(1) + ' MB';
  }
}