import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { RewardService } from '../../services/reward.service';
import { Reward } from '../../models/reward.model';

@Component({
  selector: 'app-reward-create',
  templateUrl: './reward-create.component.html',
  styleUrls: ['./reward-create.component.scss'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class RewardCreateComponent implements OnInit {

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private rewardService: RewardService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // ✅ fb je sada sigurno inicijalizovan
    this.form = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      minimumPoints: [1, [Validators.required, Validators.min(1)]],
      isActive: [true]
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    this.rewardService.create(this.form.value as Reward)
      .subscribe(() => {
        this.router.navigate(['/rewards']);
      });
  }
}
