// components/problem-report-form/problem-report-form.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProblemReportService } from '../../services/problem-report.service';
import { CurrentUserService } from '../../../core/services/auth/current-user.service'; // OVO KORISTITE
import { CreateProblemReportCommand, UpdateProblemReportCommand } from '../../models/problem-report.model';
import { MatIcon } from "@angular/material/icon";
import { MatProgressSpinner } from "@angular/material/progress-spinner";
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgIf } from '@angular/common';
@Component({
  selector: 'app-problem-report-form',
  templateUrl: './problem-report-form.component.html',
  styleUrls: ['./problem-report-form.component.scss'],
  imports: [MatIcon, MatProgressSpinner, RouterModule, ReactiveFormsModule,CommonModule, NgIf]
})
export class ProblemReportFormComponent implements OnInit {
  form: FormGroup;
  loading = false;
  submitting = false;
  error: string | null = null;
  successMessage: string | null = null;
  isEditMode = false;
  reportId?: number;
  isAuthenticated = false;
  currentUser: any = null;
  // Dohvati trenutnog korisnika
  

  constructor(
    private fb: FormBuilder,
    private problemReportService: ProblemReportService,
    private currentUserService: CurrentUserService, // OVO KORISTITE
    private route: ActivatedRoute,
    public router: Router,
    
  ) {
    this.form = this.createForm();
  }

  ngOnInit(): void {
    this.currentUser = this.currentUserService.currentUser;
    this.isAuthenticated = this.currentUserService.isAuthenticated();
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.reportId = +params['id'];
        this.loadReport();
      }
    });
  }

  createForm(): FormGroup {
    return this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      location: ['', [Validators.maxLength(200)]],
      categoryId: ['', [Validators.required, Validators.min(1)]],
      statusId: ['', [Validators.required, Validators.min(1)]]
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
        this.error = 'Greška pri učitavanju prijave: ' + err.message;
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    // Provjeri da li je korisnik prijavljen
    if (!this.isAuthenticated) {
      this.error = 'Morate biti prijavljeni da biste kreirali prijavu problema';
      this.router.navigate(['/auth/login'], { 
        queryParams: { returnUrl: this.router.url } 
      });
      return;
    }

    if (this.form.invalid) {
      this.markFormGroupTouched(this.form);
      return;
    }

    this.submitting = true;
    this.error = null;
    this.successMessage = null;

    if (this.isEditMode && this.reportId) {
      this.updateReport();
    } else {
      this.createReport();
    }
  }

  createReport(): void {
    const currentUser = this.currentUser();
    if (!currentUser) {
      this.error = 'Korisnik nije pronađen. Molimo prijavite se ponovo.';
      this.submitting = false;
      return;
    }

    const command: CreateProblemReportCommand = {
      ...this.form.value,
      userId: parseInt(currentUser.id, 10)  // Konvertujte string u number
    };

    this.problemReportService.createReport(command).subscribe({
      next: (response) => {
        this.successMessage = 'Problem uspješno prijavljen!';
        this.form.reset();
        
        // Preusmjeri nakon 2 sekunde
        setTimeout(() => {
          this.router.navigate(['/problem-reports']);
        }, 2000);
      },
      error: (err) => {
        this.error = 'Greška pri kreiranju prijave: ' + err.message;
        this.submitting = false;
      },
      complete: () => {
        this.submitting = false;
      }
    });
  }

  updateReport(): void {
    if (!this.reportId) return;

    const command: UpdateProblemReportCommand = {
      ...this.form.value
    };

    this.problemReportService.updateReport(this.reportId, command).subscribe({
      next: (response) => {
        this.successMessage = 'Prijava uspješno ažurirana!';
        
        // Preusmjeri nakon 2 sekunde
        setTimeout(() => {
          this.router.navigate(['/problem-reports']);
        }, 2000);
      },
      error: (err) => {
        this.error = 'Greška pri ažuriranju prijave: ' + err.message;
        this.submitting = false;
      },
      complete: () => {
        this.submitting = false;
      }
    });
  }

  getFieldError(fieldName: string): string | null {
    const field = this.form.get(fieldName);
    if (field?.errors && (field?.touched || field?.dirty)) {
      if (field.errors['required']) return 'Ovo polje je obavezno';
      if (field.errors['maxlength']) return `Maksimalno ${field.errors['maxlength'].requiredLength} karaktera`;
      if (field.errors['min']) return `Minimalna vrijednost je ${field.errors['min'].min}`;
    }
    return null;
  }

  markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }
}