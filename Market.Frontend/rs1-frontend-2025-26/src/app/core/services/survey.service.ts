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

  getAll(): Observable<{ total: number; items: SurveyListItem[] }> {
    return this.http.get<{ total: number; items: SurveyListItem[] }>(this.apiUrl);
  }
}
