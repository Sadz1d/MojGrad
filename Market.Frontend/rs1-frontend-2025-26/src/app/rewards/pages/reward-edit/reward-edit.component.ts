import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { RewardService } from '../../services/reward.service';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-reward-edit',
  templateUrl: './reward-edit.component.html',
  styleUrls: ['./reward-edit.component.scss'],
  imports: [
    CommonModule,                   // ✅ OBAVEZNO
    ReactiveFormsModule             // ✅ OVO RJEŠAVA formGroup GREŠKU
  ]
})
export class RewardEditComponent implements OnInit {

  id!: number;
  form!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private rewardService: RewardService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id'));

    this.form = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      minimumPoints: [1, [Validators.required, Validators.min(1)]],
      isActive: [true]
    });

    this.rewardService.getById(this.id).subscribe((reward: any) => {
      this.form.patchValue(reward);
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    this.rewardService.update(this.id, this.form.value).subscribe(() => {
      this.router.navigate(['/rewards']);
    });
  }
}
