import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SurveyListItem } from '../models/survey.model';

@Injectable({
  providedIn: 'root'
})
export class SurveyService {

  private apiUrl = `${environment.apiUrl}/surveys`;

  constructor(private http: HttpClient) {}

  getAll(filters: {
    search?: string;
    activeOn?: string;
    onlyActive?: boolean;
    fromDate?: string;
    toDate?: string;
  }): Observable<{ total: number; items: SurveyListItem[] }> {

    return this.http.get<{ total: number; items: SurveyListItem[] }>(
      this.apiUrl,
      {
        params: {
          search: filters.search ?? '',
          activeOn: filters.activeOn ?? '',
          onlyActive: filters.onlyActive ?? false,
          fromDate: filters.fromDate ?? '',
          toDate: filters.toDate ?? '',
          page: 1,
          pageSize: 20
        }
      }
    );
  }
  create(payload: {
    question: string;
    startDate: string;
    endDate: string;
  }) {
    return this.http.post(this.apiUrl, payload);
  }
  update(id: number, data: any) {
    return this.http.put(`${this.apiUrl}/${id}`, data);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  getById(id: number) {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

}
