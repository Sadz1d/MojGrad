import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AiService {

  private apiUrl = 'https://localhost:7260/api/ai/ask';

  constructor(private http: HttpClient) {}

  ask(question: string): Observable<{ answer: string }> {
    return this.http.post<{ answer: string }>(this.apiUrl, {
      question: question
    });
  }
}
