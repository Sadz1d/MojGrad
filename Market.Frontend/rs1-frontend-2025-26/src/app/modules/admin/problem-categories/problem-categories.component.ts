import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MatIcon } from '@angular/material/icon';
import { environment } from '../../../../environments/environment';

interface ProblemCategory {
  id: number;
  name: string;
  description?: string;
  isEnabled: boolean;
}

@Component({
  selector: 'app-problem-categories',
  templateUrl: './problem-categories.component.html',
  styleUrls: ['./problem-categories.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatIcon]
})
export class ProblemCategoriesComponent implements OnInit {
  categories: ProblemCategory[] = [];
  loading = false;
  submitting = false;
  error: string | null = null;
  success: string | null = null;

  form: FormGroup;
  editingId: number | null = null;
  showForm = false;

  private apiUrl = `${environment.apiUrl}/reports/categories`;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.maxLength(500)]]
    });
  }

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.loading = true;
    this.http.get<any>(this.apiUrl).subscribe({
      next: (res) => {
        this.categories = res.items ?? res;
        this.loading = false;
      },
      error: () => { this.loading = false; }
    });
  }

  openCreate(): void {
    this.editingId = null;
    this.form.reset();
    this.showForm = true;
  }

  openEdit(cat: ProblemCategory): void {
    this.editingId = cat.id;
    this.form.patchValue({ name: cat.name, description: cat.description ?? '' });
    this.showForm = true;
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingId = null;
    this.form.reset();
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.submitting = true;
    this.error = null;

    const payload = this.form.value;

    const req = this.editingId
      ? this.http.put(`${this.apiUrl}/${this.editingId}`, payload)
      : this.http.post(this.apiUrl, payload);

    req.subscribe({
      next: () => {
        this.success = this.editingId ? 'Kategorija ažurirana.' : 'Kategorija kreirana.';
        this.submitting = false;
        this.cancelForm();
        this.loadCategories();
        setTimeout(() => this.success = null, 3000);
      },
      error: (err) => {
        this.error = 'Greška: ' + (err.error?.message ?? err.message);
        this.submitting = false;
      }
    });
  }

  toggleEnabled(cat: ProblemCategory): void {
    const url = `${this.apiUrl}/${cat.id}/${cat.isEnabled ? 'disable' : 'enable'}`;
    this.http.put(url, {}).subscribe({
      next: () => this.loadCategories(),
      error: (err) => this.error = 'Greška pri promjeni statusa.'
    });
  }

  delete(cat: ProblemCategory): void {
    if (!confirm(`Obrisati kategoriju "${cat.name}"?`)) return;
    this.http.delete(`${this.apiUrl}/${cat.id}`).subscribe({
      next: () => { this.success = 'Kategorija obrisana.'; this.loadCategories(); },
      error: () => this.error = 'Greška pri brisanju.'
    });
  }
}
