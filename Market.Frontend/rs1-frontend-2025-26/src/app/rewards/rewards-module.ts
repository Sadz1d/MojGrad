import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { RewardsRoutingModule } from './rewards-routing.module';

import { RewardListComponent } from './pages/reward-list/reward-list.component';
import { RewardCreateComponent } from './pages/reward-create/reward-create.component';
import { RewardEditComponent } from './pages/reward-edit/reward-edit.component';

@NgModule({

  imports: [
    CommonModule,
    ReactiveFormsModule,
    RewardsRoutingModule
  ]
})
export class RewardsModule {}
