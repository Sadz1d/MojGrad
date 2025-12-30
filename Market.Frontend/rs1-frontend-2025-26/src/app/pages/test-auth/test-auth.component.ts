import { Component, OnInit } from '@angular/core';
import { TestAuthService } from '../../services/test-auth';
@Component({
  selector: 'app-test-auth',
  templateUrl: './test-auth.component.html',
  styleUrl: './test-auth.component.scss',
})
export class TestAuthComponent implements OnInit {

  token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIzIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoic3RyaW5nIiwidmVyIjoiMCIsImlhdCI6MTc2NzA5NjUzMSwianRpIjoiMTM2ZDJlMDMxNDAyNGVlM2JkYmUxY2RhODkzMThlZjkiLCJhdWQiOlsiTWFya2V0LlNwYSIsIk1hcmtldC5TcGEiXSwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiRW1wbG95ZWUiLCJuYmYiOjE3NjcwOTY1MzEsImV4cCI6MTc2NzA5NzczMSwiaXNzIjoiTWFya2V0LkFwaSJ9.y3ZgpDOIkuNwsmmrKw-ElPBvW4V9_av-bbxzssfb2d0';

  constructor(private testAuthService: TestAuthService) {}

  ngOnInit(): void {
    // PUBLIC endpoint
    this.testAuthService.publicEndpoint().subscribe({
      next: res => console.log('PUBLIC OK:', res),
      error: err => console.error('PUBLIC ERROR:', err)
    });

    // AUTHENTICATED endpoint
    this.testAuthService.authenticated(this.token).subscribe({
      next: res => console.log('AUTH OK:', res),
      error: err => console.error('AUTH ERROR:', err)
    });

    // ADMIN endpoint
    this.testAuthService.admin(this.token).subscribe({
      next: res => console.log('ADMIN OK:', res),
      error: err => console.error('ADMIN ERROR:', err)
    });
  }
}
