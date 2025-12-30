import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TestAuthService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  publicEndpoint() {
    return this.http.get(`${this.baseUrl}/test-auth/public`, {
      responseType: 'text'
    });
  }

  authenticated(token: string) {
    return this.http.get(`${this.baseUrl}/test-auth/authenticated`, {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    });
  }

  admin(token: string) {
    return this.http.get(`${this.baseUrl}/test-auth/admin`, {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    });
  }
}
