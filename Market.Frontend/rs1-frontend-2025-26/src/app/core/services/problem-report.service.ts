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

  // GET lista sa filterima
  getReports(filter: ProblemReportFilter): Observable<PageResult<ProblemReportListItem>> {
    let params = new HttpParams();
    
    // Dodaj filter parametre
    if (filter.search) params = params.set('search', filter.search);
    if (filter.userId) params = params.set('userId', filter.userId.toString());
    if (filter.categoryId) params = params.set('categoryId', filter.categoryId.toString());
    if (filter.statusId) params = params.set('statusId', filter.statusId.toString());
    if (filter.page) params = params.set('page', filter.page.toString());
    if (filter.pageSize) params = params.set('pageSize', filter.pageSize.toString());

    return this.http.get<PageResult<ProblemReportListItem>>(this.baseUrl, { params });
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
}