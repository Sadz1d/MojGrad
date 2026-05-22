import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VolunteerActionService } from '../../../core/services/volunteer-action.service';
import { ActivatedRoute, Router } from '@angular/router';

declare let L: any;

@Component({
  standalone: false,
  selector: 'app-volunteer-action-create',
  templateUrl: './volunteer-action-create.component.html',
  styleUrl: './volunteer-action-create.component.scss',
})
export class VolunteerActionCreateComponent implements OnInit, OnDestroy, AfterViewInit {

  stepBasic!: FormGroup;
  stepDetails!: FormGroup;
  stepCapacity!: FormGroup;

  actionId: number | null = null;
  isEditMode = false;

  map: any;
  marker: any;

  autosaveStatus: 'idle' | 'saving' | 'saved' | 'error' = 'idle';
  private readonly autosaveKey = 'autosave_volunteer_action';
  private autosaveTimer: any = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private volunteerActionService: VolunteerActionService
  ) {}

  ngOnInit(): void {
    this.stepBasic = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required]
    });

    this.stepDetails = this.fb.group({
      location: ['', Validators.required],
      eventDate: ['', Validators.required],
      latitude: [null, Validators.required],
      longitude: [null, Validators.required]
    });

    this.stepCapacity = this.fb.group({
      maxParticipants: [1, [Validators.required, Validators.min(1)]]
    });

    this.actionId = Number(this.route.snapshot.paramMap.get('id'));

    if (this.actionId) {
      this.isEditMode = true;

      this.volunteerActionService.getAction(this.actionId).subscribe(action => {
        this.stepBasic.patchValue({
          name: action.name,
          description: action.description
        });

        this.stepDetails.patchValue({
          location: action.location,
          eventDate: action.eventDate,
          latitude: action.latitude,
          longitude: action.longitude
        });

        this.stepCapacity.patchValue({
          maxParticipants: action.maxParticipants
        });

        if (action.latitude != null && action.longitude != null) {
          const lat = action.latitude;
          const lng = action.longitude;

          setTimeout(() => {
            this.setMarker(lat, lng);
          }, 500);
        }
      });
    } else {
      this.loadAutosave();
    }

    this.watchForms();
  }

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.initMap();
    }, 500);
  }

  initMap(): void {
    if (this.map) return;

    this.map = L.map('volunteer-map').setView([43.3438, 17.8078], 13);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    }).addTo(this.map);

    this.map.on('click', (e: any) => {
      const lat = e.latlng.lat;
      const lng = e.latlng.lng;

      this.stepDetails.patchValue({
        latitude: lat,
        longitude: lng
      });

      this.setMarker(lat, lng);
    });
  }

  setMarker(lat: number, lng: number): void {
    if (!this.map) return;

    if (this.marker) {
      this.map.removeLayer(this.marker);
    }

    this.marker = L.marker([lat, lng])
      .addTo(this.map)
      .bindPopup('Lokacija volontiranja')
      .openPopup();

    this.map.setView([lat, lng], 15);
  }

  private watchForms(): void {
    const trigger = () => {
      this.autosaveStatus = 'idle';
      clearTimeout(this.autosaveTimer);
      this.autosaveTimer = setTimeout(() => this.performAutosave(), 1500);
    };

    this.stepBasic.valueChanges.subscribe(trigger);
    this.stepDetails.valueChanges.subscribe(trigger);
    this.stepCapacity.valueChanges.subscribe(trigger);
  }

  private performAutosave(): void {
    const data = {
      basic: this.stepBasic.value,
      details: this.stepDetails.value,
      capacity: this.stepCapacity.value,
      savedAt: new Date().toISOString()
    };

    this.autosaveStatus = 'saving';

    try {
      localStorage.setItem(this.autosaveKey, JSON.stringify(data));
      this.autosaveStatus = 'saved';

      setTimeout(() => {
        if (this.autosaveStatus === 'saved') this.autosaveStatus = 'idle';
      }, 3000);
    } catch {
      this.autosaveStatus = 'error';
    }
  }

  private loadAutosave(): void {
    try {
      const saved = localStorage.getItem(this.autosaveKey);
      if (!saved) return;

      const parsed = JSON.parse(saved);

      if (parsed.basic) this.stepBasic.patchValue(parsed.basic);
      if (parsed.details) {
        this.stepDetails.patchValue(parsed.details);

        if (parsed.details.latitude && parsed.details.longitude) {
          setTimeout(() => {
            this.setMarker(parsed.details.latitude, parsed.details.longitude);
          }, 700);
        }
      }
      if (parsed.capacity) this.stepCapacity.patchValue(parsed.capacity);

      this.autosaveStatus = 'saved';
    } catch {
      // ignore
    }
  }

  clearAutosave(): void {
    localStorage.removeItem(this.autosaveKey);
    this.stepBasic.reset();
    this.stepDetails.reset();
    this.stepCapacity.reset({ maxParticipants: 1 });
    this.autosaveStatus = 'idle';

    if (this.marker) {
      this.map.removeLayer(this.marker);
      this.marker = null;
    }
  }

  get autosaveLabel(): string {
    switch (this.autosaveStatus) {
      case 'saving': return '💾 Snima se...';
      case 'saved': return '✅ Automatski sačuvano';
      case 'error': return '⚠️ Greška pri autosave';
      default: return '';
    }
  }

  submit(): void {
    if (this.stepBasic.invalid || this.stepDetails.invalid || this.stepCapacity.invalid) return;

    const payload = {
      name: this.stepBasic.value.name,
      description: this.stepBasic.value.description,
      location: this.stepDetails.value.location,
      eventDate: this.stepDetails.value.eventDate,
      latitude: this.stepDetails.value.latitude,
      longitude: this.stepDetails.value.longitude,
      maxParticipants: this.stepCapacity.value.maxParticipants
    };

    if (this.isEditMode && this.actionId) {
      this.volunteerActionService.updateAction(this.actionId, payload).subscribe(() => {
        localStorage.removeItem(this.autosaveKey);
        alert('✏️ Akcija izmijenjena');
        this.router.navigate(['/volunteering']);
      });
    } else {
      this.volunteerActionService.createAction(payload).subscribe(() => {
        localStorage.removeItem(this.autosaveKey);
        alert('✅ Akcija kreirana');
        this.router.navigate(['/volunteering']);
      });
    }
  }

  ngOnDestroy(): void {
    clearTimeout(this.autosaveTimer);

    if (this.map) {
      this.map.remove();
    }
  }
}
