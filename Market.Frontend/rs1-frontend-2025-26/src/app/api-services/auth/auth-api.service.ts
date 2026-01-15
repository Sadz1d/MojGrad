// src/app/api-services/auth/auth-api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ResetPasswordRequest } from '../../core/models/reset-password.request';
import { ResetPasswordResponse } from '../../core/models/reset-password.response';


export interface LoginRequest {
  email: string;
  password: string;
  fingerprint?: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  expiresAtUtc: string;

  id: number;
  email: string;
  firstName: string;
  lastName: string;

  isAdmin: boolean;
  isManager: boolean;
  isEmployee: boolean;
}


export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber?: string;
}

export interface RegisterResponse {
  userId: number;
  email: string;
  fullName: string;
  message: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ForgotPasswordResponse {
  email: string;
  message: string;
  resetTokenExpiresAt: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
  fingerprint?: string;
}

export interface LogoutRequest {
  refreshToken: string;
}

export interface CurrentUserResponse {
  id: number;
  email: string;

  firstName: string;
  lastName: string;
  fullName: string;

  isAdmin: boolean;
  isManager: boolean;
  isEmployee: boolean;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber?: string | undefined
}

export interface RegisterResponse {
  userId: number;
  email: string;
  fullName: string;
  message: string;
}
@Injectable({
  providedIn: 'root'
})
export class AuthApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(data: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, data);
  }

  register(data: RegisterRequest): Observable<RegisterResponse> {
    return this.http.post<RegisterResponse>(`${this.apiUrl}/auth/register`, data);
  }

  forgotPassword(data: ForgotPasswordRequest): Observable<ForgotPasswordResponse> {
    return this.http.post<ForgotPasswordResponse>(`${this.apiUrl}/auth/forgot-password`, data);
  }

  refreshToken(data: RefreshTokenRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/refresh`, data);
  }

  logout(data: LogoutRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/auth/logout`, data);
  }

  getCurrentUser(): Observable<CurrentUserResponse> {
    return this.http.get<CurrentUserResponse>(`${this.apiUrl}/auth/me`);
  }

  resetPassword(request: ResetPasswordRequest): Observable<ResetPasswordResponse> {
    return this.http.post<ResetPasswordResponse>(
      '/api/auth/reset-password',
      request
    );
  }

}
