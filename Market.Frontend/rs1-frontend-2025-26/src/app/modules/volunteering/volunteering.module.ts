import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { VolunteerListComponent } from './volunteer-list/volunteer-list.component';
import {FormsModule} from '@angular/forms';

const routes: Routes = [
  { path: '', component: VolunteerListComponent }
];

@NgModule({
  declarations: [VolunteerListComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule
  ]
})
export class VolunteeringModule {}
