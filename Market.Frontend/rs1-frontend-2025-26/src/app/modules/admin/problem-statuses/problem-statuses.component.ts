import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { MatIcon } from '@angular/material/icon';
import { environment } from '../../../../environments/environment';

interface ProblemStatus {
  id: number;
  name: string;
}

@Component({
  selector: 'app-problem-statuses',
  templateUrl: './problem-statuses.component.html',
  styleUrls: ['./problem-statuses.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatIcon]
})
export class ProblemStatusesComponent implements OnInit {
  statuses: ProblemStatus[] = [];
  loading = false;
  submitting = false;
  error: string | null = null;
  success: string | null = null;

  form: FormGroup;
  editingId: number | null = null;
  showForm = false;

  private apiUrl = `${environment.apiUrl}/reports/statuses`;

  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(50)]]
    });
  }

  ngOnInit(): void {
    this.loadStatuses();
  }

  loadStatuses(): void {
    this.loading = true;
    this.http.get<any>(this.apiUrl).subscribe({
      next: (res) => {
        this.statuses = res.items ?? res;
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

  openEdit(s: ProblemStatus): void {
    this.editingId = s.id;
    this.form.patchValue({ name: s.name });
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

    const req = this.editingId
      ? this.http.put(`${this.apiUrl}/${this.editingId}`, this.form.value)
      : this.http.post(this.apiUrl, this.form.value);

    req.subscribe({
      next: () => {
        this.success = this.editingId ? 'Status ažuriran.' : 'Status kreiran.';
        this.submitting = false;
        this.cancelForm();
        this.loadStatuses();
        setTimeout(() => this.success = null, 3000);
      },
      error: (err) => {
        this.error = 'Greška: ' + (err.error?.message ?? err.message);
        this.submitting = false;
      }
    });
  }

  delete(s: ProblemStatus): void {
    if (!confirm(`Obrisati status "${s.name}"?`)) return;
    this.http.delete(`${this.apiUrl}/${s.id}`).subscribe({
      next: () => { this.success = 'Status obrisan.'; this.loadStatuses(); },
      error: () => this.error = 'Greška pri brisanju.'
    });
  }

  getStatusColor(name: string): string {
    const s = name.toLowerCase();
    if (s.includes('nov') || s.includes('prijavljen')) return '#f44336';
    if (s.includes('toku') || s.includes('obrađ')) return '#ff9800';
    if (s.includes('rije') || s.includes('rešen') || s.includes('završen')) return '#4caf50';
    return '#9e9e9e';
  }

  getStatusColorLabel(name: string): string {
    const s = name.toLowerCase();
    if (s.includes('nov') || s.includes('prijavljen')) return 'Crvena (novo)';
    if (s.includes('toku') || s.includes('obrađ')) return 'Narandžasta (u toku)';
    if (s.includes('rije') || s.includes('rešen') || s.includes('završen')) return 'Zelena (riješeno)';
    return 'Siva (ostalo)';
  }
}
