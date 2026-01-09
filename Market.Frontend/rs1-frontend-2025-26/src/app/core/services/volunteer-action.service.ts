import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
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

  private readonly baseUrl =
    `${environment.apiUrl}/volunteering/actions`;

  private http = inject(HttpClient);

  // =============================
  // GET – LISTA (samo READ za USER)
  // =============================
  getActions(
    page?: number,
    pageSize?: number
  ): Observable<PageResult<VolunteerActionListItem>> {

    let params = new HttpParams();

    if (page) params = params.set('page', page.toString());
    if (pageSize) params = params.set('pageSize', pageSize.toString());

    console.log('VolunteerAction API URL:', this.baseUrl);
    console.log('Params:', params.toString());

    return this.http.get<PageResult<VolunteerActionListItem>>(
      this.baseUrl,
      { params }
    );
  }

  // =============================
  // GET by ID (detalji)
  // =============================
  getAction(id: number): Observable<VolunteerActionListItem> {
    return this.http.get<VolunteerActionListItem>(
      `${this.baseUrl}/${id}`
    );
  }

  // =============================
  // POST – CREATE (ADMIN ONLY)
  // =============================
  createAction(
    command: CreateVolunteerActionCommand
  ): Observable<number> {
    return this.http
      .post<{ id: number }>(this.baseUrl, command)
      .pipe(
        map(response => response.id)
      );
  }

  // =============================
  // PUT – UPDATE (ADMIN ONLY)
  // =============================
  updateAction(
    id: number,
    command: UpdateVolunteerActionCommand
  ): Observable<void> {
    return this.http.put<void>(
      `${this.baseUrl}/${id}`,
      command
    );
  }
  // =============================
  // POST – PRIJAVA NA AKCIJU (USER)
  // =============================
  joinAction(actionId: number): Observable<void> {
    return this.http.post<void>(
      `${this.baseUrl}/${actionId}/join`,
      {}
    );
  }

  // =============================
  // DELETE (ADMIN ONLY)
  // =============================
  deleteAction(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/${id}`
    );
  }
}
// =============================
// POST – PRIJAVA NA AKCIJU (USER)
// =============================
// =============================
// POST – PRIJAVA NA AKCIJU (USER)
// =============================
// =============================
// POST – PRIJAVA NA AKCIJU (USER)
// =============================



