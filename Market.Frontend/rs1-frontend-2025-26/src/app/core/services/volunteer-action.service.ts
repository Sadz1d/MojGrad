import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import {
  VolunteerActionListItem,
  CreateVolunteerActionCommand,
  UpdateVolunteerActionCommand,
  PageResult
} from '../models/volunteer-action.model';

@Injectable({
  providedIn: 'root'
})
export class VolunteerActionService {

  private readonly baseUrl = `${environment.apiUrl}/volunteering/actions`;

  private http = inject(HttpClient);

  getActions(page?: number, pageSize?: number): Observable<PageResult<VolunteerActionListItem>> {
    let params = new HttpParams();

    if (page) params = params.set('page', page.toString());
    if (pageSize) params = params.set('pageSize', pageSize.toString());

    return this.http.get<PageResult<VolunteerActionListItem>>(this.baseUrl, { params });
  }

  getAction(id: number): Observable<VolunteerActionListItem> {
    return this.http.get<VolunteerActionListItem>(`${this.baseUrl}/${id}`);
  }

  createAction(command: CreateVolunteerActionCommand): Observable<number> {
    return this.http
      .post<{ id: number }>(this.baseUrl, command)
      .pipe(map(response => response.id));
  }

  updateAction(id: number, command: UpdateVolunteerActionCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, command);
  }

  deleteAction(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  disableAction(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/disable`, {});
  }

  enableAction(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/enable`, {});
  }

  joinAction(actionId: number): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/${actionId}/join`,
      {}
    );
  }
}
