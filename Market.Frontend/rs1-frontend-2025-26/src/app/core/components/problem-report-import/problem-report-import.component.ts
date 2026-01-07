// src/app/core/components/problem-report-import/problem-report-import.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ImportService, ImportResult } from '../../services/import.service';
import { MatIcon } from "@angular/material/icon";
import { MatProgressSpinner } from "@angular/material/progress-spinner";
import { MatCheckbox } from "@angular/material/checkbox";
import { CommonModule } from '@angular/common';
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { finalize } from 'rxjs/operators';
import * as XLSX from 'xlsx';
@Component({
  selector: 'app-problem-report-import',
  templateUrl: './problem-report-import.component.html',
  styleUrls: ['./problem-report-import.component.scss'],
  standalone: true,
  imports: [CommonModule, MatIcon, MatProgressSpinner, MatCheckbox]
})
export class ProblemReportImportComponent implements OnInit {
  importForm: FormGroup;
  selectedFile: File | null = null;
  validationResult: any = null;
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
      skipFirstRow: [true],
      dryRun: [false]  
    });
  }

  ngOnInit(): void {
    this.importService.progress$.subscribe(progress => {
      this.progress = progress;
    });
  }

  startImport(): void {
    if (!this.selectedFile) {
      this.fileError = 'Molimo odaberite fajl prije importa';
      return;
    }

    this.importing = true;
    this.importError = null;
    this.importResult = null;
    this.progress = 0;
    
    const skipFirstRow = this.importForm.get('skipFirstRow')?.value;
    const dryRun = this.importForm.get('dryRun')?.value;

    // ✅ KORISTITE PRAVU METODU KOJA ŠALJE EXCEL FAJL
    this.importService.importFile(this.selectedFile, skipFirstRow, dryRun)
      .pipe(
        finalize(() => {
          this.importing = false;
        })
      )
      .subscribe({
        next: (result) => {
          this.importResult = result;
          this.importComplete = true;
          
          if (dryRun) {
            // Ako je probni uvoz, pokažite rezultate validacije
            console.log('Dry run rezultati:', result);
          }
          
          if (result.failed > 0) {
            this.importError = `${result.failed} od ${result.totalRecords} zapisa ima greške.`;
          }
        },
        error: (error) => {
          this.importError = error.error?.message || error.message || 'Došlo je do greške pri importu.';
          this.importComplete = false;
          console.error('Import error:', error);
        }
      });
  }

  // Ostale metode ostaju iste...
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
    
    // Validacija tipa fajla
    const validExtensions = ['.xlsx', '.xls', '.csv'];
    const fileExtension = file.name.toLowerCase().slice(file.name.lastIndexOf('.'));
    
    if (!validExtensions.includes(fileExtension)) {
      this.fileError = 'Podržani formati: Excel (.xls, .xlsx) ili CSV (.csv)';
      return;
    }
    
    if (file.size > 10 * 1024 * 1024) {
      this.fileError = 'Fajl je prevelik. Maksimalna veličina je 10MB.';
      return;
    }
    
    this.selectedFile = file;
    this.fileError = null;
    
    // Možete dodati jednostavnu validaciju ovde
    this.simpleFileValidation(file);
  }

  private async simpleFileValidation(file: File): Promise<void> {
  this.loading = true;
  this.fileError = null;
  
  try {
    // Koristite XLSX biblioteku za čitanje Excel fajla
    const data = await this.readExcelFile(file);
    
    if (data.length === 0) {
      this.validationResult = {
        isValid: false,
        errors: ['Fajl je prazan ili nema podataka'],
        totalRows: 0,
        headers: [],
        sampleData: []
      };
      return;
    }
    
    // Prvi red su headers
    const headers = data[0];
    
    // Proverite da li ima obavezne kolone
    const expectedColumns = ['Title', 'Description', 'CategoryId', 'StatusId'];
    const missingColumns = expectedColumns.filter(col => 
      !headers.some((header: string) => 
        header?.toString().toLowerCase().includes(col.toLowerCase())
      )
    );
    
    // Uzmi prvih 5 redova podataka (preskačući header)
    const sampleData = data.slice(1, 6).map((row: any[]) => {
      const obj: any = {};
      headers.forEach((header: string, index: number) => {
        obj[header] = row[index] || '';
      });
      return obj;
    });
    
    this.validationResult = {
      isValid: missingColumns.length === 0,
      errors: missingColumns.length > 0 ? 
        [`Nedostaju obavezne kolone: ${missingColumns.join(', ')}`] : [],
      totalRows: data.length - 1,
      headers: headers,
      sampleData: sampleData
    };
    
    console.log('File validation result:', this.validationResult);
    
  } catch (error) {
    console.error('Error reading Excel file:', error);
    this.fileError = 'Greška pri čitanju fajla: ' + (error as Error).message;
    this.validationResult = {
      isValid: false,
      errors: ['Ne mogu da pročitam fajl'],
      totalRows: 0,
      headers: [],
      sampleData: []
    };
  } finally {
    this.loading = false;
  }
}

// Nova metoda za čitanje Excel fajla
private readExcelFile(file: File): Promise<any[][]> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    
    reader.onload = (e: ProgressEvent<FileReader>) => {
      try {
        const data = e.target?.result;
        if (!data) {
          reject(new Error('Nema podataka u fajlu'));
          return;
        }
        
        // Pročitaj Excel fajl
        const workbook = XLSX.read(data, { type: 'binary' });
        const firstSheetName = workbook.SheetNames[0];
        const worksheet = workbook.Sheets[firstSheetName];
        
        // Konvertuj u JSON sa headerima
        const jsonData = XLSX.utils.sheet_to_json<any[]>(worksheet, { header: 1 });
        
        resolve(jsonData);
      } catch (error) {
        reject(error);
      }
    };
    
    reader.onerror = () => {
      reject(new Error('Greška pri čitanju fajla'));
    };
    
    // Pročitaj kao binary string
    reader.readAsBinaryString(file);
  });
}
// Pomoćne metode za preview
isRequiredColumn(header: string): boolean {
  const requiredColumns = ['title', 'description', 'categoryid', 'statusid'];
  return requiredColumns.includes(header.toLowerCase());
}

formatCellValue(value: any): string {
  if (value === null || value === undefined) return '(prazno)';
  if (typeof value === 'object') return JSON.stringify(value);
  return String(value);
}

isEmptyRow(row: any): boolean {
  if (!row) return true;
  const requiredCols = ['Title', 'Description', 'CategoryId', 'StatusId'];
  return requiredCols.every(col => !row[col] || row[col].toString().trim() === '');
}
  downloadTemplate(): void {
     this.importService.downloadTemplateWithInstructions();
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
    this.importing = false;
    this.loading = false;
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