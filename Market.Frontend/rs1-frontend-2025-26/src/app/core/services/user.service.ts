// services/user.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserDropdown } from '../models/problem-report.model';

export interface User {
  id: number;
  name: string;
  description?: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7260/api';

  constructor(private http: HttpClient) {}

  getUsers(): Observable<UserDropdown[]> {
    return this.http.get<UserDropdown[]>(`${this.apiUrl}/users/dropdown`);
  }
}
