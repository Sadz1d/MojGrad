// components/problem-report-form/problem-report-form.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProblemReportService } from '../../services/problem-report.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
@Component({
  selector: 'app-problem-report-form',
  templateUrl: './problem-report-form.component.html',
  styleUrls: ['./problem-report-form.component.scss'],
  standalone: true,
  imports: [
    MatIconModule,
  MatProgressSpinnerModule,
    CommonModule,
    ReactiveFormsModule,
    // ...other imports...
  ],
})
export class ProblemReportFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  reportId?: number;
  loading = false;
  submitting = false;
  error = '';
  successMessage = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    public router: Router,
    private problemReportService: ProblemReportService
  ) {
    this.form = this.fb.group({
      title: ['', [
        Validators.required,
        Validators.maxLength(150)
      ]],
      description: ['', [
        Validators.required,
        Validators.maxLength(2000)
      ]],
      location: ['', [
        Validators.maxLength(200)
      ]],
      categoryId: ['', [
        Validators.required,
        Validators.min(1)
      ]],
      statusId: ['', [
        Validators.required,
        Validators.min(1)
      ]]
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.reportId = +params['id'];
        this.loadReport();
      }
    });
  }

  loadReport(): void {
    if (!this.reportId) return;
    
    this.loading = true;
    this.problemReportService.getReport(this.reportId).subscribe({
      next: (report) => {
        this.form.patchValue({
          title: report.title,
          description: report.description,
          location: report.location || '',
          categoryId: report.categoryId,
          statusId: report.statusId
        });
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Greška pri učitavanju: ' + err.message;
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      Object.keys(this.form.controls).forEach(key => {
        this.form.get(key)?.markAsTouched();
      });
      return;
    }

    this.submitting = true;
    this.error = '';
    this.successMessage = '';

    const formData = this.form.value;

    if (this.isEditMode && this.reportId) {
      this.problemReportService.updateReport(this.reportId, formData).subscribe({
        next: () => {
          this.successMessage = 'Prijava uspešno ažurirana!';
          setTimeout(() => {
            this.router.navigate(['/problem-reports']);
          }, 1500);
        },
        error: (err) => {
          this.error = 'Greška pri čuvanju: ' + err.message;
          this.submitting = false;
        }
      });
    } else {
      const createData = {
        ...formData,
        userId: this.getCurrentUserId() // Treba implementirati
      };

      this.problemReportService.createReport(createData).subscribe({
        next: (id) => {
          this.successMessage = 'Prijava uspešno kreirana!';
          setTimeout(() => {
            this.router.navigate(['/problem-reports', id]);
          }, 1500);
        },
        error: (err) => {
          this.error = 'Greška pri kreiranju: ' + err.message;
          this.submitting = false;
        }
      });
    }
  }

  getCurrentUserId(): number {
    // TODO: Implementiraj dobijanje ID-a trenutnog korisnika
    // Na primer iz auth servisa ili localStorage
    return 1; // Za sada hardcode
  }

  getFieldError(fieldName: string): string {
    const field = this.form.get(fieldName);
    if (!field?.errors || !field.touched) return '';
    
    if (field.errors['required']) return 'Ovo polje je obavezno.';
    if (field.errors['maxlength']) {
      const max = field.errors['maxlength'].requiredLength;
      return `Maksimalno ${max} karaktera.`;
    }
    if (field.errors['min']) return 'Vrednost mora biti veća od 0.';
    
    return 'Nevalidan unos.';
  }
  
}