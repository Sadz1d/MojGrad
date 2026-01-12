import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ProblemReportService {

  private readonly baseUrl = '/api/reports/problem-reports';

  constructor(private http: HttpClient) {}

  getPaged(page: number, pageSize: number): Observable<any> {
    const params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get(`${this.baseUrl}/paged`, { params });
  }
}
