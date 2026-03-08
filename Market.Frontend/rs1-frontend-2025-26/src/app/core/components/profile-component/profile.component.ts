import { Component, OnInit, inject } from '@angular/core';
import { HttpClient, HttpHeaders, HttpEvent, HttpEventType } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
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
  imports: [CommonModule, RouterModule, FormsModule, MatIconModule, MatButtonModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {
  private http = inject(HttpClient);
  private auth = inject(AuthFacadeService);
  public router = inject(Router);

  profile: UserProfile | null = null;
  loading = true;
  saving = false;
  editMode = false;
  saveSuccess = false;
  saveError: string | null = null;

  editForm = {
    phone: '',
    address: '',
    biographyText: ''
  };

  picturePreview: string | null = null;
  selectedPictureFile: File | null = null;
  uploadProgress = 0;
  removePictureOnSave = false;

  readonly apiBase = 'https://localhost:7260';

  ngOnInit(): void {
    const user = this.auth.getCurrentUserValue();
    if (!user) {
      this.router.navigate(['/auth/login']);
      return;
    }

    const headers = new HttpHeaders({ Authorization: `Bearer ${user.token}` });

    this.http.get<UserProfile>(
      `${this.apiBase}/api/Profiles/by-user/${user.id}`,
      { headers }
    ).subscribe({
      next: (profile) => {
        this.profile = profile;
        this.loading = false;
      },
      error: () => {
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

  openEdit(): void {
    if (!this.profile) return;
    this.editForm = {
      phone: this.profile.phone || '',
      address: this.profile.address || '',
      biographyText: this.profile.biographyText || ''
    };
    this.picturePreview = this.getProfilePictureUrl();
    this.selectedPictureFile = null;
    this.uploadProgress = 0;
    this.removePictureOnSave = false;
    this.editMode = true;
    this.saveError = null;
    this.saveSuccess = false;
  }

  cancelEdit(): void {
    this.editMode = false;
    this.saveError = null;
    this.picturePreview = null;
    this.selectedPictureFile = null;
    this.uploadProgress = 0;
    this.removePictureOnSave = false;
  }

  onPictureSelected(event: any): void {
    const file = event.target.files[0];
    if (!file) return;
    this.selectedPictureFile = file;
    this.removePictureOnSave = false;
    const reader = new FileReader();
    reader.onload = () => this.picturePreview = reader.result as string;
    reader.readAsDataURL(file);
  }

  removePicture(): void {
    this.selectedPictureFile = null;
    this.picturePreview = null;
    this.uploadProgress = 0;
    this.removePictureOnSave = true;
  }

  saveProfile(): void {
    if (!this.profile) return;
    const user = this.auth.getCurrentUserValue();
    if (!user) return;

    this.saving = true;
    this.saveError = null;

    const headers = new HttpHeaders({ Authorization: `Bearer ${user.token}` });
    const body = {
      phone: this.editForm.phone || null,
      address: this.editForm.address || null,
      biographyText: this.editForm.biographyText || null,
      profilePicture: this.removePictureOnSave ? null : (this.profile.profilePicture || null),
      clearProfilePicture: this.removePictureOnSave
    };

    this.http.put(
      `${this.apiBase}/api/Profiles/by-user/${user.id}`,
      body,
      { headers }
    ).subscribe({
      next: () => {
        this.profile!.phone = this.editForm.phone || undefined;
        this.profile!.address = this.editForm.address || undefined;
        this.profile!.biographyText = this.editForm.biographyText || undefined;

        if (this.removePictureOnSave) {
          this.profile!.profilePicture = undefined;
          this.removePictureOnSave = false;
          this.saving = false;
          this.editMode = false;
          this.saveSuccess = true;
          setTimeout(() => this.saveSuccess = false, 3000);
        } else if (this.selectedPictureFile) {
          this.uploadPictureAndFinish(this.selectedPictureFile, user.token, user.id);
        } else {
          this.saving = false;
          this.editMode = false;
          this.saveSuccess = true;
          setTimeout(() => this.saveSuccess = false, 3000);
        }
      },
      error: (err) => {
        this.saving = false;
        this.saveError = err.error?.message || 'Greška pri čuvanju. Pokušajte ponovo.';
      }
    });
  }

  private uploadPictureAndFinish(file: File, token: string, userId: number): void {
    const formData = new FormData();
    formData.append('image', file);
    const headers = new HttpHeaders({ Authorization: `Bearer ${token}` });

    this.http.post(
      `${this.apiBase}/api/Profiles/by-user/${userId}/upload-picture`,
      formData,
      { headers, reportProgress: true, observe: 'events' }
    ).subscribe({
      next: (event: HttpEvent<any>) => {
        if (event.type === HttpEventType.UploadProgress) {
          this.uploadProgress = Math.round(100 * event.loaded / (event.total ?? 1));
        }
        if (event.type === HttpEventType.Response) {
          const imageUrl = event.body?.imageUrl;
          if (imageUrl) this.profile!.profilePicture = imageUrl;
          this.saving = false;
          this.editMode = false;
          this.saveSuccess = true;
          this.uploadProgress = 0;
          this.selectedPictureFile = null;
          setTimeout(() => this.saveSuccess = false, 3000);
        }
      },
      error: (err) => {
        this.saving = false;
        this.saveError = err.error?.message || 'Greška pri uploadu slike.';
        this.uploadProgress = 0;
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
