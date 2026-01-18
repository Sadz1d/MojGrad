import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RewardListComponent } from './pages/reward-list/reward-list.component';
import { RewardCreateComponent } from './pages/reward-create/reward-create.component';
import { RewardEditComponent } from "./pages/reward-edit/reward-edit.component";

const routes: Routes = [
  { path: '', component: RewardListComponent },
  { path: 'new', component: RewardCreateComponent },
  { path: ':id/edit', component: RewardEditComponent }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RewardsRoutingModule {}
