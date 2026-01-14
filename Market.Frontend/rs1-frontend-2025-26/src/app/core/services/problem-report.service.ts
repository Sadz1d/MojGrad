// services/problem-report.service.ts
import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ProblemReportListItem,
  ProblemReportDetail,
  CreateProblemReportCommand,
  UpdateProblemReportCommand,
  PageResult,
  ProblemReportFilter
} from '../models/problem-report.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ProblemReportService {
  private readonly baseUrl = `${environment.apiUrl}/reports/problem-reports`;
  private http = inject(HttpClient);

  // GET lista sa filterima I SORTIRANJEM
  getReports(filter: ProblemReportFilter): Observable<PageResult<ProblemReportListItem>> {
    let params = new HttpParams();
    
    // Dodaj filter parametre
    if (filter.search) params = params.set('search', filter.search);
    if (filter.userId) params = params.set('userId', filter.userId.toString());
    if (filter.categoryId) params = params.set('categoryId', filter.categoryId.toString());
    if (filter.statusId) params = params.set('statusId', filter.statusId.toString());
    
    // Dodaj paginaciju parametre
    if (filter.page) params = params.set('page', filter.page.toString());
    if (filter.pageSize) params = params.set('pageSize', filter.pageSize.toString());
    
    // Dodaj SORTIRANJE parametre
    if (filter.sortBy) params = params.set('sortBy', filter.sortBy);
    if (filter.sortDirection) params = params.set('sortDirection', filter.sortDirection);

    console.log('API Request:', {
    url: this.baseUrl + '/paged', // DODAJTE OVO!
    params: params.toString()
  });

    return this.http.get<PageResult<ProblemReportListItem>>(
    `${this.baseUrl}/paged`, // ILI this.baseUrl + '/paged'
    { params }
  ).pipe(
    map(response => {
      console.log('📥 API Response:', response);
      return response;
    })
    );
  }

  // GET po ID-u
  getReport(id: number): Observable<ProblemReportDetail> {
    return this.http.get<ProblemReportDetail>(`${this.baseUrl}/${id}`);
  }

  // POST - kreiranje
  createReport(command: CreateProblemReportCommand): Observable<number> {
    return this.http.post<{ id: number }>(this.baseUrl, command)
      .pipe(map(response => response.id));
  }

  // PUT - update
  updateReport(id: number, command: UpdateProblemReportCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, command);
  }

  // DELETE
  deleteReport(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  // Get validne kolone za sortiranje (opciono, za validaciju)
  getSortableColumns(): string[] {
    return [
      'id',
      'title', 
      'authorName',
      'categoryName',
      'statusName',
      'location',
      'creationDate',
      'commentsCount',
      'tasksCount',
      'ratingsCount'
    ];
  }

  // Validate sort direction (opciono)
  isValidSortDirection(direction: string): boolean {
    const validDirections = ['asc', 'desc', ''];
    return validDirections.includes(direction.toLowerCase());
  }

  // Get default sort (opciono)
  getDefaultSort(): { sortBy: string, sortDirection: string } {
    return {
      sortBy: 'id',
      sortDirection: 'desc'
    };
  }
  // Ako backend ne podržava sortiranje
sortReportsLocally(
  reports: ProblemReportListItem[], 
  sortBy: string, 
  sortDirection: string
): ProblemReportListItem[] {
  if (!sortBy || sortDirection === '') {
    return reports;
  }

  return [...reports].sort((a, b) => {
    const aValue = this.getSortValue(a, sortBy);
    const bValue = this.getSortValue(b, sortBy);
    
    if (sortDirection === 'asc') {
      return this.compareValues(aValue, bValue);
    } else {
      return this.compareValues(bValue, aValue);
    }
  });
}

private getSortValue(item: ProblemReportListItem, sortBy: string): any {
  switch (sortBy) {
    case 'id': return item.id;
    case 'title': return item.title?.toLowerCase() || '';
    case 'authorName': return item.authorName?.toLowerCase() || '';
    case 'categoryName': return item.categoryName?.toLowerCase() || '';
    case 'statusName': return item.statusName?.toLowerCase() || '';
    case 'location': return item.location?.toLowerCase() || '';
    case 'creationDate': return new Date(item.creationDate).getTime();
    case 'commentsCount': return item.commentsCount || 0;
    case 'tasksCount': return item.tasksCount || 0;
    case 'ratingsCount': return item.ratingsCount || 0;
    default: return '';
  }
}

private compareValues(a: any, b: any): number {
  if (a === b) return 0;
  return a > b ? 1 : -1;
}
}