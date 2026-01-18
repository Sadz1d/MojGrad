// src/app/core/services/auth/auth-facade.service.ts
import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, throwError, of } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { User } from './models/user.model';

import {
  AuthApiService,
  LoginRequest,
  LoginResponse,
  RegisterRequest,
  RegisterResponse,
  ForgotPasswordRequest,
  ForgotPasswordResponse,
  RefreshTokenRequest,
  CurrentUserResponse,

} from '../../../api-services/auth/auth-api.service';



@Injectable({
  providedIn: 'root'
})
export class AuthFacadeService {
  private api = inject(AuthApiService);
  private router = inject(Router);

  private currentUserSubject = new BehaviorSubject<User | null>(null);

  currentUser$ = this.currentUserSubject.asObservable();
  isAuthenticated$ = this.currentUser$.pipe(map(user => !!user));

  constructor() {
    this.loadUserFromStorage();
  }

  // ==================== PUBLIC METHODS ====================

  // 🔐 LOGIN
  login(data: LoginRequest): Observable<CurrentUserResponse> {
    const fingerprint = this.generateFingerprint();
    const loginData = { ...data, fingerprint };

    return this.api.login(loginData).pipe(
      tap(response => {
        this.handleLoginSuccess(response, fingerprint);
      }),
      switchMap(() => this.getCurrentUserInfo()),   // 👈 OVO JE KLJUČ
      catchError((error: any) => {
        this.clearAuthData();
        return throwError(() => error);
      })
    );
  }


  // 📝 REGISTER
  register(data: RegisterRequest): Observable<RegisterResponse> {
    return this.api.register(data).pipe(
      catchError((error: any) => {
        return throwError(() => error);
      })
    );
  }

  // 🔄 REFRESH TOKEN
  refreshToken(): Observable<LoginResponse> {
    const user = this.getCurrentUserValue();

    if (!user?.refreshToken) {
      return throwError(() => new Error('No refresh token available'));
    }

    const request: RefreshTokenRequest = {
      refreshToken: user.refreshToken,
      fingerprint: this.getFingerprint()
    };

    return this.api.refreshToken(request).pipe(
      tap(response => {
        this.updateTokens(response);
      }),
      catchError((error: any) => {
        this.logout();
        return throwError(() => error);
      })
    );
  }

  // 🚪 LOGOUT
  logout(): Observable<void> {
    const user = this.getCurrentUserValue();

    if (user?.refreshToken) {
      return this.api.logout({ refreshToken: user.refreshToken }).pipe(
        tap(() => {
          this.clearAuthData();
          this.router.navigate(['/auth/login']);
        }),
        catchError((error: any) => {
          this.clearAuthData();
          this.router.navigate(['/auth/login']);
          return throwError(() => error);
        })
      );
    } else {
      this.clearAuthData();
      this.router.navigate(['/auth/login']);
      return of(undefined);
    }
  }

  // 📧 FORGOT PASSWORD
  forgotPassword(email: string): Observable<ForgotPasswordResponse> {
    const request: ForgotPasswordRequest = { email };
    return this.api.forgotPassword(request).pipe(
      catchError((error: any) => {
        return throwError(() => error);
      })
    );
  }

  resetPassword(token: string, newPassword: string) {
    return this.api.resetPassword({
      token,
      newPassword,
    });
  }



  // 👤 GET CURRENT USER INFO
  getCurrentUserInfo(): Observable<CurrentUserResponse> {
    return this.api.getCurrentUser().pipe(
      tap((user: CurrentUserResponse) => {
        const current = this.getCurrentUserValue();

        if (!current) return;

        const updatedUser: User = {
          ...current,
          id: user.id,
          email: user.email,
          fullName: user.fullName ?? `${user.firstName} ${user.lastName}`,
          isAdmin: user.isAdmin,
          isManager: user.isManager,
          isEmployee: user.isEmployee
        };

        this.currentUserSubject.next(updatedUser);
        this.saveUserToStorage(updatedUser);
      }),
      catchError((error: any) => {
        return throwError(() => error);
      })
    );
  }


  // ==================== HELPER METHODS ====================

  private handleLoginSuccess(response: LoginResponse, fingerprint: string): void {

    const payload = this.decodeToken(response.accessToken);

    const user: User = {
      id: payload.sub ? Number(payload.sub) : 0,
      email: payload.email || '',
      fullName: this.getFullNameFromToken(payload),

      isAdmin: payload.is_admin === true || payload.is_admin === 'true',
      isManager: payload.is_manager === true || payload.is_manager === 'true',
      isEmployee: payload.is_employee === true || payload.is_employee === 'true',

      token: response.accessToken,
      refreshToken: response.refreshToken,
      expiresAt: new Date(response.expiresAtUtc)
    };

    this.currentUserSubject.next(user);
    this.saveUserToStorage(user);
    localStorage.setItem('auth_fingerprint', fingerprint);
  }



  private updateTokens(response: LoginResponse): void {
    const currentUser = this.getCurrentUserValue();
    if (!currentUser) return;

    const updatedUser: User = {
      ...currentUser,
      token: response.accessToken,
      refreshToken: response.refreshToken,
      expiresAt: new Date(response.expiresAtUtc)
    };

    this.currentUserSubject.next(updatedUser);
    this.saveUserToStorage(updatedUser);
  }

  private decodeToken(token: string): any {
    try {
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    } catch {
      return {};
    }
  }

  private getFullNameFromToken(payload: any): string {
    if (payload.fullName) return payload.fullName;
    if (payload.given_name && payload.family_name) {
      return `${payload.given_name} ${payload.family_name}`;
    }
    if (payload.name) return payload.name;
    return '';
  }

  private generateFingerprint(): string {
    const fingerprint = localStorage.getItem('auth_fingerprint');
    if (fingerprint) return fingerprint;

    const newFingerprint = `fp_${Date.now()}_${Math.random().toString(36).substr(2, 9)}`;
    localStorage.setItem('auth_fingerprint', newFingerprint);
    return newFingerprint;
  }

  private getFingerprint(): string {
    return localStorage.getItem('auth_fingerprint') || '';
  }

  private saveUserToStorage(user: User): void {
    localStorage.setItem('auth_user', JSON.stringify(user));
  }

  private loadUserFromStorage(): void {
    const userJson = localStorage.getItem('auth_user');
    if (userJson) {
      try {
        const user = JSON.parse(userJson);
        if (user.expiresAt && new Date(user.expiresAt) > new Date()) {
          this.currentUserSubject.next(user);
        } else {
          this.clearAuthData();
        }
      } catch {
        this.clearAuthData();
      }
    }
  }

  private clearAuthData(): void {
    localStorage.removeItem('auth_user');
    localStorage.removeItem('auth_fingerprint');
    this.currentUserSubject.next(null);
  }

  // ==================== GETTERS ====================

  getCurrentUserValue(): User | null {
    return this.currentUserSubject.value;
  }

  getToken(): string | null {
    return this.getCurrentUserValue()?.token || null;
  }

  isLoggedIn(): boolean {
    return !!this.getCurrentUserValue();
  }

  // For template use
  currentUser(): User | null {
    return this.getCurrentUserValue();
  }

  isAuthenticated(): boolean {
    return this.isLoggedIn();
  }

  isAdmin(): boolean {
    return this.getCurrentUserValue()?.isAdmin || false;
  }

  isManager(): boolean {
    return this.getCurrentUserValue()?.isManager || false;
  }

  isEmployee(): boolean {
    return this.getCurrentUserValue()?.isEmployee || false;
  }
}

export type { User };
