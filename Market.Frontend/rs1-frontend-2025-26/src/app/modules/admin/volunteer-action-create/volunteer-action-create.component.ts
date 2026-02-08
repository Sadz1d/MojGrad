import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VolunteerActionService } from '../../../core/services/volunteer-action.service';
import { ActivatedRoute, Router } from '@angular/router';

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
  actionId: number | null = null;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private volunteerActionService: VolunteerActionService
  ) {
  }

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
          eventDate: action.eventDate
        });

        this.stepCapacity.patchValue({
          maxParticipants: action.maxParticipants
        });
      });
    }


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

    if (this.isEditMode && this.actionId) {
      this.volunteerActionService
        .updateAction(this.actionId, payload)
        .subscribe(() => {
          alert('✏️ Akcija izmijenjena');
          this.router.navigate(['/volunteering']);
        });
    } else {
      this.volunteerActionService
        .createAction(payload)
        .subscribe(() => {
          alert('✅ Akcija kreirana');
          this.router.navigate(['/volunteering']);
        });
    }
  }
}
