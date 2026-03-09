// components/problem-report-form/problem-report-form.component.ts
import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ProblemReportService } from '../../services/problem-report.service';
import { AuthFacadeService} from '../../../core/services/auth/auth-facade.service';
import { User } from '../../../core/services/auth/models/user.model';
import { CategoryService } from '../../services/category.service';
import { StatusService } from '../../services/status.service';
import { CreateProblemReportCommand, UpdateProblemReportCommand } from '../../models/problem-report.model';
import { CategoryDropdown, StatusDropdown } from '../../models/problem-report.model';
import { MatIcon } from "@angular/material/icon";
import { MatProgressSpinner } from "@angular/material/progress-spinner";
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { MatProgressBar } from "@angular/material/progress-bar";

declare const L: any;

@Component({
  selector: 'app-problem-report-form',
  templateUrl: './problem-report-form.component.html',
  styleUrls: ['./problem-report-form.component.scss'],
  imports: [MatIcon, MatProgressSpinner, RouterModule, ReactiveFormsModule, CommonModule, MatProgressBar]
})
export class ProblemReportFormComponent implements OnInit, OnDestroy, AfterViewInit {
  form: FormGroup;
  loading = false;
  submitting = false;
  error: string | null = null;
  successMessage: string | null = null;
  isEditMode = false;
  reportId?: number;
  isAuthenticated = false;

  currentUserFullName: string = 'Korisnik';

  // Dropdown podaci
  categories: CategoryDropdown[] = [];
  statuses: StatusDropdown[] = [];
  loadingCategories = false;
  loadingStatuses = false;

  // RxJS unsubscribe subject
  private destroy$ = new Subject<void>();

  // Leaflet mapa
  private map: any = null;
  private marker: any = null;
  mapInitialized = false;

  constructor(
    private fb: FormBuilder,
    private problemReportService: ProblemReportService,
    private authFacadeService: AuthFacadeService, // OVO PROMIJENITE
    private categoryService: CategoryService,
    private statusService: StatusService,
    private route: ActivatedRoute,
    public router: Router,
    private http: HttpClient
  ) {
    this.form = this.createForm();
  }

  ngOnInit(): void {
    this.isAuthenticated = this.authFacadeService.isAuthenticated();

    const user: User | null = this.authFacadeService.getCurrentUserValue();
    if (user) {
      this.currentUserFullName = user.fullName;
    }

    // Dohvati dropdown podatke
    this.loadCategories();
    this.loadStatuses();

    // Provjeri edit mode
    this.route.params
      .pipe(takeUntil(this.destroy$))
      .subscribe(params => {
        if (params['id']) {
          this.isEditMode = true;
          this.reportId = +params['id'];
          this.loadReport();
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    if (this.map) {
      this.map.remove();
      this.map = null;
    }
  }

  // Getter za trenutnog korisnika - FIXED
  getCurrentUser() {
    return this.authFacadeService.getCurrentUserValue(); // OVO SADA RADI
  }

  createForm(): FormGroup {
    return this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.maxLength(2000)]],
      location: ['', [Validators.maxLength(200)]],
      latitude: [null],
      longitude: [null],
      categoryId: ['', [Validators.required, Validators.min(1)]],
      statusId: ['', [Validators.required, Validators.min(1)]],
      categoryName: [''],
      statusName: ['']
    });
  }

  ngAfterViewInit(): void {
    // Za new mode loading je odmah false, pa inicijaliziramo mapu odmah
    if (!this.isEditMode) {
      setTimeout(() => this.initMap(), 0);
    }
    // Za edit mode, initMap() se poziva iz loadReport() nakon što se podaci učitaju
  }

  private initMap(): void {
    if (typeof L === 'undefined') return;
    // Ako mapa već postoji (npr. component reuse), uništi je
    if (this.map) {
      this.map.remove();
      this.map = null;
      this.marker = null;
    }

    const defaultLat = 43.8563;
    const defaultLng = 18.4131;

    this.map = L.map('location-map').setView([defaultLat, defaultLng], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);

    this.map.on('click', (e: any) => {
      this.setMarker(e.latlng.lat, e.latlng.lng);
      this.reverseGeocode(e.latlng.lat, e.latlng.lng);
    });

    this.mapInitialized = true;

    // Provjeri da li forma već ima koordinate (ako loadReport završio prije initMap)
    const lat = this.form.get('latitude')?.value;
    const lng = this.form.get('longitude')?.value;
    if (lat && lng) {
      this.setMarker(lat, lng);
      this.map.setView([lat, lng], 15);
    }
  }

  private setMarker(lat: number, lng: number): void {
    if (this.marker) {
      this.marker.setLatLng([lat, lng]);
    } else {
      this.marker = L.marker([lat, lng], { draggable: true }).addTo(this.map);
      this.marker.on('dragend', (e: any) => {
        const pos = e.target.getLatLng();
        this.form.patchValue({ latitude: pos.lat, longitude: pos.lng });
        this.reverseGeocode(pos.lat, pos.lng);
      });
    }
    this.form.patchValue({ latitude: lat, longitude: lng });
  }

  private reverseGeocode(lat: number, lng: number): void {
    const url = `https://nominatim.openstreetmap.org/reverse?lat=${lat}&lon=${lng}&format=json`;
    this.http.get<any>(url).subscribe({
      next: (result) => {
        const addr = result.address;
        // Uzmi najkorisniji dio adrese: ulica + grad ili samo grad
        const parts = [
          addr.road || addr.pedestrian || addr.suburb,
          addr.city || addr.town || addr.village || addr.municipality
        ].filter(Boolean);
        const locationName = parts.join(', ') || result.display_name?.split(',').slice(0, 2).join(',');
        this.form.patchValue({ location: locationName });
      },
      error: () => {
        // Nominatim nije dostupan — ostavi prazno, korisnik može ručno unijeti
      }
    });
  }

  clearLocation(): void {
    if (this.marker) {
      this.marker.remove();
      this.marker = null;
    }
    this.form.patchValue({ location: '', latitude: null, longitude: null });
  }

  loadCategories(): void {
    this.loadingCategories = true;
    this.categoryService.getCategories()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (categories) => {
          this.categories = categories;
          this.loadingCategories = false;
        },
        error: (err) => {
          console.error('Greška pri učitavanju kategorija:', err);
          this.loadingCategories = false;
        }
      });
  }

  loadStatuses(): void {
    this.loadingStatuses = true;
    this.statusService.getStatuses()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (statuses) => {
          this.statuses = statuses;
          this.loadingStatuses = false;
        },
        error: (err) => {
          console.error('Greška pri učitavanju statusa:', err);
          this.loadingStatuses = false;
        }
      });
  }

  loadReportImage(reportId: number) {
    const headers = { Authorization: `Bearer ${this.authFacadeService.getToken()}` };
    this.http.get(`https://localhost:7260/api/reports/problem-reports/${reportId}/image`,
      { headers, responseType: 'blob' })
      .subscribe(blob => {
        this.imagePreview = URL.createObjectURL(blob);
      }, err => {
        console.error('Greška pri učitavanju slike:', err);
      });
  }

  loadReport(): void {
    if (!this.reportId) return;

    this.loading = true;
    this.problemReportService.getReport(this.reportId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (report) => {
          const category = this.categories.find(c => c.id === report.categoryId);
          const status = this.statuses.find(s => s.id === report.statusId);

          this.form.patchValue({
            title: report.title,
            description: report.description,
            location: report.location || '',
            latitude: report.latitude || null,
            longitude: report.longitude || null,
            categoryId: report.categoryId,
            statusId: report.statusId,
            categoryName: category ? category.name : '',
            statusName: status ? status.name : ''
          });

          this.loading = false; // Postavi loading=false PRVO da Angular rendira form i location-map div

          // Inicijaliziraj mapu tek nakon što Angular re-rendira DOM (loading=false → *ngIf="!loading" postaje true)
          setTimeout(() => {
            this.initMap();
            if (report.latitude && report.longitude) {
              this.setMarker(report.latitude, report.longitude);
              this.map.setView([report.latitude, report.longitude], 15);
            }
          }, 50);

          // Use imagePath from DTO directly — no extra HTTP call needed
          if (report.imagePath) {
            this.imagePreview = `https://localhost:7260${report.imagePath}`;
          }

        },
        error: (err) => {
          this.error = 'Greška pri učitavanju prijave: ' + err.message;
          this.loading = false;
        }
      });
  }

  onCategoryChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedId = parseInt(selectElement.value, 10);
    const selectedCategory = this.categories.find(c => c.id === selectedId);

    if (selectedCategory) {
      this.form.patchValue({
        categoryId: selectedCategory.id,
        categoryName: selectedCategory.name
      });
    }
  }

  onStatusChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    const selectedId = parseInt(selectElement.value, 10);
    const selectedStatus = this.statuses.find(s => s.id === selectedId);

    if (selectedStatus) {
      this.form.patchValue({
        statusId: selectedStatus.id,
        statusName: selectedStatus.name
      });
    }
  }

  onSubmit(): void {
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

    if (!this.form.value.categoryId) {
      this.error = 'Morate odabrati kategoriju';
      return;
    }

    if (!this.form.value.statusId) {
      this.error = 'Morate odabrati status';
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
    const currentUser = this.getCurrentUser(); // KORISTIMO FIXED GETTER
    if (!currentUser) {
      this.error = 'Korisnik nije pronađen. Molimo prijavite se ponovo.';
      this.submitting = false;
      return;
    }

    const formValue = this.form.value;
    const command: CreateProblemReportCommand = {
      title: formValue.title,
      description: formValue.description,
      location: formValue.location,
      latitude: formValue.latitude,
      longitude: formValue.longitude,
      categoryId: formValue.categoryId,
      statusId: formValue.statusId,
      userId: currentUser.id
    };

    this.problemReportService.createReport(command)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.successMessage = 'Problem uspješno prijavljen!';
          this.form.reset();

          if (this.imagePreview && this.selectedFile) {
            // POST image nakon što imamo reportId
            this.reportId = response;
            this.uploadImage(this.selectedFile);
          }

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

    const formValue = this.form.value;
    const command: UpdateProblemReportCommand = {
      title: formValue.title,
      description: formValue.description,
      location: formValue.location,
      latitude: formValue.latitude,
      longitude: formValue.longitude,
      categoryId: formValue.categoryId,
      statusId: formValue.statusId
    };

    this.problemReportService.updateReport(this.reportId, command)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.successMessage = 'Prijava uspješno ažurirana!';

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


  progress = 0;
  imagePreview: string | null = null;
  selectedFile: File | null = null;

  removeImage(): void {
    this.imagePreview = null;
    this.selectedFile = null;
    this.progress = 0;
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (!file) return;

    this.selectedFile = file;
    // preview
    const reader = new FileReader();
    reader.onload = () => this.imagePreview = reader.result as string;
    reader.readAsDataURL(file);

    if (this.reportId) {
      this.uploadImage(file);
    }
  }

  private uploadImage(file: File) {
    const formData = new FormData();
    formData.append('image', file);

    this.http.post(
      `https://localhost:7260/api/reports/problem-reports/${this.reportId}/upload-image`,
      formData,
      {
        reportProgress: true,
        observe: 'events'
      }
    ).subscribe(event => {

      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round(
          100 * event.loaded / (event.total ?? 1)
        );
      }

      if (event.type === HttpEventType.Response) {
        console.log('Upload završen');
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

  getImageUrl(reportId: number): string {
    if (!reportId) return '';
    return `https://localhost:7260/api/reports/problem-reports/${reportId}/image`;
  }


}
