import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ResetPasswordRequest } from '../../models/reset-password.request';
import { ResetPasswordResponse } from '../../models/reset-password.response';


@Injectable({
  providedIn: 'root',
})
export class AuthApiService {
  constructor(private http: HttpClient) {}

  resetPassword(
    request: ResetPasswordRequest
  ): Observable<ResetPasswordResponse> {
    return this.http.post<ResetPasswordResponse>(
      '/api/auth/reset-password',
      request
    );
  }
}
