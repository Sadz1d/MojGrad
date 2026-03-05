import { Component, OnInit, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

export interface UserProfile {
  id: number;
  userId: number;
  userFullName: string;
  email: string;
  address?: string;
  phone?: string;
  profilePicture?: string;
  biographyText?: string;
  points: number;
  registrationDate: string;
  isAdmin: boolean;
  isManager: boolean;
  isEmployee: boolean;
  reportsCount: number;
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, RouterModule, MatIconModule, MatButtonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  private http = inject(HttpClient);
  private auth = inject(AuthFacadeService);
  public router = inject(Router);

  profile: UserProfile | null = null;
  loading = true;
  error: string | null = null;

  readonly apiBase = 'https://localhost:7260';

  ngOnInit(): void {
    const user = this.auth.getCurrentUserValue();
    if (!user) {
      this.router.navigate(['/auth/login']);
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${user.token}`
    });

    this.http.get<UserProfile>(
      `${this.apiBase}/api/Profiles/by-user/${user.id}`,
      { headers }
    ).subscribe({
      next: (profile) => {
        this.profile = profile;
        this.loading = false;
      },
      error: () => {
        // Profile might not exist yet — show user data from token
        this.profile = {
          id: 0,
          userId: user.id,
          userFullName: user.fullName || user.email,
          email: user.email,
          points: 0,
          registrationDate: new Date().toISOString(),
          isAdmin: user.isAdmin,
          isManager: user.isManager,
          isEmployee: user.isEmployee,
          reportsCount: 0
        };
        this.loading = false;
      }
    });
  }

  getRoles(): string[] {
    if (!this.profile) return [];
    const roles: string[] = [];
    if (this.profile.isAdmin) roles.push('Administrator');
    if (this.profile.isManager) roles.push('Menadžer');
    if (this.profile.isEmployee) roles.push('Zaposlenik');
    if (roles.length === 0) roles.push('Građanin');
    return roles;
  }

  getAvatarLetter(): string {
    const name = this.profile?.userFullName || this.profile?.email || '?';
    return name.charAt(0).toUpperCase();
  }

  getProfilePictureUrl(): string | null {
    if (!this.profile?.profilePicture) return null;
    if (this.profile.profilePicture.startsWith('http')) return this.profile.profilePicture;
    return `${this.apiBase}${this.profile.profilePicture}`;
  }

  getMemberSince(): string {
    if (!this.profile?.registrationDate) return 'N/A';
    const d = new Date(this.profile.registrationDate);
    const months = [
      'januara', 'februara', 'marta', 'aprila', 'maja', 'juna',
      'jula', 'augusta', 'septembra', 'oktobra', 'novembra', 'decembra'
    ];
    return `${d.getDate()}. ${months[d.getMonth()]} ${d.getFullYear()}.`;
  }
}
