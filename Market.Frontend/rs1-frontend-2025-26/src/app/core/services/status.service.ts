// services/status.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StatusDropdown } from '../models/problem-report.model';

export interface Status {
  id: number;
  name: string;
  description?: string;
}

@Injectable({
  providedIn: 'root'
})
export class StatusService {
  private apiUrl = 'https://localhost:7260/api';

  constructor(private http: HttpClient) {}

  getStatuses(): Observable<StatusDropdown[]> {
    return this.http.get<StatusDropdown[]>(`${this.apiUrl}/reports/statuses/dropdown`);
  }
}