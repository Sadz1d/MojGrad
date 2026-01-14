// services/category.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CategoryDropdown } from '../models/problem-report.model';

export interface Category {
  id: number;
  name: string;
  description?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = 'https://localhost:7260/api';

  constructor(private http: HttpClient) {}

  getCategories(): Observable<CategoryDropdown[]> {
    return this.http.get<CategoryDropdown[]>(`${this.apiUrl}/reports/categories/dropdown`);
  }
}