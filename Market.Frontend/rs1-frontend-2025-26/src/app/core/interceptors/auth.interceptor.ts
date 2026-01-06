// src/app/core/interceptors/auth.interceptor.ts
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { AuthFacadeService } from '../services/auth/auth-facade.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private auth: AuthFacadeService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    // Get token
    const token = this.auth.getToken();
    
    // Add Authorization header if token exists and not auth endpoint
    if (token && !request.url.includes('/auth/')) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          return this.handle401Error(request, next);
        }
        return throwError(() => error);
      })
    );
  }

  private handle401Error(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return this.auth.refreshToken().pipe(
      switchMap((response: any) => {
        // Retry with new token
        const newRequest = request.clone({
          setHeaders: {
            Authorization: `Bearer ${response.accessToken}`
          }
        });
        return next.handle(newRequest);
      }),
      catchError((refreshError: any) => {
        // If refresh fails, logout
        this.auth.logout();
        return throwError(() => refreshError);
      })
    );
  }
}