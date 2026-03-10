import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MatIcon } from '@angular/material/icon';
import { environment } from '../../../../environments/environment';

interface AdminUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  isAdmin: boolean;
  isManager: boolean;
  isEmployee: boolean;
  isEnabled: boolean;
  registrationDate: string;
}

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatIcon]
})
export class AdminUsersComponent implements OnInit {
  users: AdminUser[] = [];
  loading = false;
  error: string | null = null;
  success: string | null = null;

  editingUser: AdminUser | null = null;
  form: FormGroup;

  private apiUrl = `${environment.apiUrl}/users`;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.form = this.fb.group({
      isAdmin: [false],
      isManager: [false],
      isEmployee: [false],
      isEnabled: [true]
    });
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.http.get<any>(`${this.apiUrl}/list`).subscribe({
      next: (res) => {
        this.users = res.items ?? res;
        this.loading = false;
      },
      error: () => { this.loading = false; }
    });
  }

  openEdit(user: AdminUser): void {
    this.editingUser = user;
    this.form.patchValue({
      isAdmin: user.isAdmin,
      isManager: user.isManager,
      isEmployee: user.isEmployee,
      isEnabled: user.isEnabled
    });
  }

  cancelEdit(): void {
    this.editingUser = null;
    this.form.reset();
  }

  saveRoles(): void {
    if (!this.editingUser) return;

    const payload = {
      firstName: this.editingUser.firstName,
      lastName: this.editingUser.lastName,
      email: this.editingUser.email,
      isAdmin: this.form.value.isAdmin,
      isManager: this.form.value.isManager,
      isEmployee: this.form.value.isEmployee,
      isEnabled: this.form.value.isEnabled
    };

    this.http.put(`${this.apiUrl}/${this.editingUser.id}`, payload).subscribe({
      next: () => {
        this.success = 'Korisnik ažuriran.';
        this.cancelEdit();
        this.loadUsers();
        setTimeout(() => this.success = null, 3000);
      },
      error: (err) => this.error = 'Greška: ' + (err.error?.message ?? err.message)
    });
  }

  getRoleBadges(user: AdminUser): string[] {
    const roles = [];
    if (user.isAdmin) roles.push('Admin');
    if (user.isManager) roles.push('Manager');
    if (user.isEmployee) roles.push('Employee');
    if (!user.isAdmin && !user.isManager && !user.isEmployee) roles.push('Citizen');
    return roles;
  }

  getRoleClass(role: string): string {
    switch (role) {
      case 'Admin': return 'role-admin';
      case 'Manager': return 'role-manager';
      case 'Employee': return 'role-employee';
      default: return 'role-citizen';
    }
  }
}
