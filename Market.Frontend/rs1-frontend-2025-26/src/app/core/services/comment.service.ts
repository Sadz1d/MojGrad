// services/comment.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  CommentListItem,
  CommentDetail,
  CreateCommentCommand,
  UpdateCommentCommand,
  CommentFilter,
  PageResult
} from '../models/comment.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private apiUrl = `${environment.apiUrl}/reports/comments`;

  constructor(private http: HttpClient) {}

  getComments(filter: CommentFilter = {}): Observable<PageResult<CommentListItem>> {
    let params = new HttpParams();

    if (filter.reportId) params = params.set('reportId', filter.reportId.toString());
    if (filter.userId) params = params.set('userId', filter.userId.toString());
    if (filter.search) params = params.set('search', filter.search);
    if (filter.page) params = params.set('page', filter.page.toString());
    if (filter.pageSize) params = params.set('pageSize', filter.pageSize.toString());
    if (filter.sortBy) params = params.set('sortBy', filter.sortBy);
    if (filter.sortDirection) params = params.set('sortDirection', filter.sortDirection);

    return this.http.get<PageResult<CommentListItem>>(this.apiUrl, { params });
  }

  getComment(id: number): Observable<CommentDetail> {
    return this.http.get<CommentDetail>(`${this.apiUrl}/${id}`);
  }

  createComment(command: CreateCommentCommand): Observable<number> {
    return this.http.post<number>(this.apiUrl, command);
  }

  updateComment(id: number, command: UpdateCommentCommand): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, command);
  }

  deleteComment(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  // Helper metode za lokalno sortiranje (ako je potrebno)
  sortCommentsLocally(
    comments: CommentListItem[],
    column: string,
    direction: 'asc' | 'desc' | ''
  ): CommentListItem[] {
    if (!direction) return comments;

    return [...comments].sort((a, b) => {
      let aValue: any;
      let bValue: any;

      switch (column) {
        case 'userName':
          aValue = a.userName?.toLowerCase() || '';
          bValue = b.userName?.toLowerCase() || '';
          break;
        case 'publicationDate':
          aValue = new Date(a.publicationDate).getTime();
          bValue = new Date(b.publicationDate).getTime();
          break;
        case 'text':
          aValue = a.text?.toLowerCase() || '';
          bValue = b.text?.toLowerCase() || '';
          break;
        default:
          aValue = a[column as keyof CommentListItem];
          bValue = b[column as keyof CommentListItem];
      }

      if (aValue < bValue) return direction === 'asc' ? -1 : 1;
      if (aValue > bValue) return direction === 'asc' ? 1 : -1;
      return 0;
    });
  }
}