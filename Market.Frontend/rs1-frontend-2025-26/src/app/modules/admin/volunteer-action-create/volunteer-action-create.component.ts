import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VolunteerActionService } from '../../../core/services/volunteer-action.service';

@Component({
  standalone:false,
  selector: 'app-volunteer-action-create',
  templateUrl: './volunteer-action-create.component.html',
  styleUrl: './volunteer-action-create.component.scss',
})

export class VolunteerActionCreateComponent implements OnInit {

  stepBasic!: FormGroup;
  stepDetails!: FormGroup;
  stepCapacity!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private volunteerActionService: VolunteerActionService
  ) {}

  // 👇 OVDJE SE FORME PRAVILNO KREIRAJU
  ngOnInit(): void {
    this.stepBasic = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required]
    });

    this.stepDetails = this.fb.group({
      location: ['', Validators.required],
      eventDate: ['', Validators.required]
    });

    this.stepCapacity = this.fb.group({
      maxParticipants: [1, [Validators.required, Validators.min(1)]]
    });
  }

  // =============================
  // SUBMIT
  // =============================
  submit(): void {
    if (
      this.stepBasic.invalid ||

      this.stepDetails.invalid ||
      this.stepCapacity.invalid
    ) {
      return;
    }

    const payload = {
      name: this.stepBasic.value.name,
      description: this.stepBasic.value.description,
      location: this.stepDetails.value.location,
      eventDate: this.stepDetails.value.eventDate,
      maxParticipants: this.stepCapacity.value.maxParticipants
    };

    this.volunteerActionService.createAction(payload).subscribe({
      next: () => {
        alert('✅ Volonterska akcija uspješno kreirana!');
        this.stepBasic.reset();
        this.stepDetails.reset();
        this.stepCapacity.reset({ maxParticipants: 1 });
      },
      error: () => {
        alert('❌ Greška pri kreiranju akcije.');
      }
    });
  }
}
