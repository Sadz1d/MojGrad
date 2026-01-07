// src/app/core/services/import.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import * as XLSX from 'xlsx';
import { environment } from '../../../environments/environment';
import { filter } from 'rxjs/operators';
export interface ImportResult {
  totalRecords: number;
  successful: number;
  failed: number;
  errors: string[];
  importedIds: number[];
}

export interface FileValidationResult {
  isValid: boolean;
  errors: string[];
  totalRows: number;
  headers: string[];
  sampleData: any[];
}

@Injectable({
  providedIn: 'root'
})
export class ImportService {
  private baseUrl = `${environment.apiUrl}/reports/problem-reports`;
  
  private progressSubject = new Subject<number>();
  public progress$ = this.progressSubject.asObservable();

  constructor(private http: HttpClient) {}

importFile(file: File, skipFirstRow: boolean = true, dryRun: boolean = false): Observable<ImportResult> {
    const formData = new FormData();
    formData.append('File', file, file.name);
    formData.append('SkipFirstRow', skipFirstRow.toString());
    formData.append('DryRun', dryRun.toString());

    return this.http.post<ImportResult>(`${this.baseUrl}/import`, formData, {
      reportProgress: true,
      observe: 'events'
    }).pipe(
      map((event: HttpEvent<any>) => {
        if (event.type === HttpEventType.UploadProgress && event.total) {
          const progress = Math.round(100 * event.loaded / event.total);
          this.progressSubject.next(progress);
          return null;
        } else if (event.type === HttpEventType.Response) {
          this.progressSubject.next(100);
          return event.body;
        }
        return null;
      }),
      filter((result): result is ImportResult => result !== null),
      catchError(error => {
        this.progressSubject.next(0);
        throw this.handleImportError(error);
      })
    );
  }

  validateFile(file: File): Promise<FileValidationResult> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      
      reader.onload = (e: any) => {
        try {
          const data = new Uint8Array(e.target.result);
          const workbook = XLSX.read(data, { type: 'array' });
          
          const firstSheetName = workbook.SheetNames[0];
          const worksheet = workbook.Sheets[firstSheetName];
          const jsonData = XLSX.utils.sheet_to_json(worksheet, { header: 1 });
          
          if (jsonData.length < 2) {
            resolve({
              isValid: false,
              errors: ['Fajl nema podataka'],
              totalRows: 0,
              headers: [],
              sampleData: []
            });
            return;
          }
          
          const headers = jsonData[0] as string[];
          const expectedHeaders = ['title', 'description', 'categoryId', 'statusId'];
          const headerErrors = this.validateHeaders(headers, expectedHeaders);
          
          const sampleData = jsonData.slice(1, 6).map((row: any) => {
            const obj: any = {};
            headers.forEach((header, index) => {
              obj[header] = row[index];
            });
            return obj;
          });
          
          resolve({
            isValid: headerErrors.length === 0,
            errors: headerErrors,
            totalRows: jsonData.length - 1,
            headers,
            sampleData
          });
          
        } catch (error) {
          reject(error);
        }
      };
      
      reader.onerror = () => {
        reject(new Error('Greška pri čitanju fajla'));
      };
      
      reader.readAsArrayBuffer(file);
    });
  }

  parseFile(file: File): Promise<any[]> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      
      reader.onload = (e: any) => {
        try {
          const data = new Uint8Array(e.target.result);
          const workbook = XLSX.read(data, { type: 'array' });
          const firstSheetName = workbook.SheetNames[0];
          const worksheet = workbook.Sheets[firstSheetName];
          const jsonData = XLSX.utils.sheet_to_json(worksheet);
          resolve(jsonData);
          
        } catch (error) {
          reject(error);
        }
      };
      
      reader.onerror = () => {
        reject(new Error('Greška pri parsiranju fajla'));
      };
      
      reader.readAsArrayBuffer(file);
    });
  }

  importProblemReports(data: any[]): Observable<ImportResult> {
    const formData = new FormData();
    const jsonData = JSON.stringify(data);
    const blob = new Blob([jsonData], { type: 'application/json' });
    
    formData.append('file', blob, 'import-data.json');
    
    return this.http.post<ImportResult>(`${this.baseUrl}/import`, formData, {
      reportProgress: true,
      observe: 'events'
    }).pipe(
      map(event => this.getEventMessage(event)),
      catchError(error => {
        throw this.handleImportError(error);
      })
    );
  }

  importWithProgress(file: File): Observable<ImportResult> {
    return new Observable(observer => {
      this.parseFile(file).then(data => {
        const total = data.length;
        let processed = 0;
        
        const interval = setInterval(() => {
          processed += Math.ceil(total / 10);
          if (processed >= total) {
            processed = total;
            clearInterval(interval);
          }
          
          const progress = Math.round((processed / total) * 100);
          this.progressSubject.next(progress);
        }, 300);
        
        this.importProblemReports(data).subscribe({
          next: (result) => {
            clearInterval(interval);
            this.progressSubject.next(100);
            observer.next(result);
            observer.complete();
          },
          error: (error) => {
            clearInterval(interval);
            observer.error(error);
          }
        });
        
      }).catch(error => {
        observer.error(error);
      });
    });
  }
// Dodajte ovu metodu u servis
downloadTemplateWithInstructions(): void {
  // Prvo, možda dohvatite postojeće kategorije i statuse iz baze
  // Ovo je primer - možete adaptirati za vaš API
  this.http.get<any[]>('https://localhost:7260/api/categories').subscribe({
    next: (categories) => {
      this.http.get<any[]>('https://localhost:7260/api/statuses').subscribe({
        next: (statuses) => {
          this.generateTemplateWithData(categories, statuses);
        },
        error: () => {
          // Fallback ako ne može da dobije podatke
          this.generateTemplateWithData([], []);
        }
      });
    },
    error: () => {
      this.generateTemplateWithData([], []);
    }
  });
}

private generateTemplateWithData(categories: any[], statuses: any[]): void {
  // Kreiraj sheet sa primjerima
  const exampleData = [
    {
      Title: 'Nepokošena trava u parku',
      Description: 'Trava u centralnom parku nije pokošena nekoliko sedmica',
      Location: 'Park Zrinjevac',
      CategoryId: categories.length > 0 ? categories[0].id : 1,
      StatusId: statuses.length > 0 ? statuses[0].id : 1,
      UserId: 1
    }
  ];

  const headers = ['Title', 'Description', 'Location', 'CategoryId', 'StatusId', 'UserId'];
  
  const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(exampleData, { header: headers });
  
  // Kreiraj sheet sa listama validnih ID-jeva
  const validIdsData = [
    ['VALIDNI ID-JEVI IZ BAZE'],
    [''],
    ['KATEGORIJE:'],
    ['ID', 'Naziv', 'Opis']
  ];
  
  categories.forEach(cat => {
    validIdsData.push([cat.id, cat.name, cat.description || '']);
  });
  
  validIdsData.push([''], ['STATUSI:'], ['ID', 'Naziv', 'Opis']);
  
  statuses.forEach(status => {
    validIdsData.push([status.id, status.name, status.description || '']);
  });
  
  const wsValidIds = XLSX.utils.aoa_to_sheet(validIdsData);
  
  const wb: XLSX.WorkBook = XLSX.utils.book_new();
  XLSX.utils.book_append_sheet(wb, ws, 'Template');
  XLSX.utils.book_append_sheet(wb, wsValidIds, 'Validni ID-jev');
  
  const excelBuffer: any = XLSX.write(wb, { bookType: 'xlsx', type: 'array' });
  
  const data: Blob = new Blob([excelBuffer], {
    type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
  });
  
  const url = window.URL.createObjectURL(data);
  const link = document.createElement('a');
  link.href = url;
  link.download = `problem-reports-template-with-valid-ids.xlsx`;
  link.click();
  window.URL.revokeObjectURL(url);
}
  downloadTemplate(): void {
    const templateData = [
      {
        title: 'Nepokošena trava u parku',
        description: 'Trava u parku nije pokošena nekoliko sedmica',
        categoryId: 1,
        statusId: 1,
        location: 'Park Zrinjevac',
        userId: 1
      },
      {
        title: 'Polomljena rasvjeta',
        description: 'Oštećena rasvjeta na ulazu',
        categoryId: 2,
        statusId: 2,
        location: 'Splitska ulica',
        userId: 1
      }
    ];
    
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(templateData);
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Template');
    
    const excelBuffer: any = XLSX.write(wb, {
      bookType: 'xlsx',
      type: 'array'
    });
    
    const data: Blob = new Blob([excelBuffer], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });
    
    const url = window.URL.createObjectURL(data);
    const link = document.createElement('a');
    link.href = url;
    link.download = 'problem-reports-template.xlsx';
    link.click();
    window.URL.revokeObjectURL(url);
  }

  private validateHeaders(actual: string[], expected: string[]): string[] {
    const errors: string[] = [];
    const actualLower = actual.map(h => h.toLowerCase().trim());
    
    expected.forEach(expectedHeader => {
      if (!actualLower.includes(expectedHeader.toLowerCase())) {
        errors.push(`Nedostaje kolona: ${expectedHeader}`);
      }
    });
    
    return errors;
  }

  private getEventMessage(event: HttpEvent<any>): any {
    switch (event.type) {
      case HttpEventType.UploadProgress:
        const percentDone = Math.round(100 * event.loaded / (event.total || 1));
        this.progressSubject.next(percentDone);
        return null;
        
      case HttpEventType.Response:
        return event.body;
        
      default:
        return null;
    }
  }

  private handleImportError(error: any): Error {
    if (error.status === 400) {
      return new Error('Nevalidni podaci u fajlu: ' + (error.error?.message || ''));
    }
    if (error.status === 413) {
      return new Error('Fajl je prevelik. Maksimalna veličina je 10MB.');
    }
    return new Error('Greška pri importu: ' + error.message);
  }
}