import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { VolunteerListComponent } from './volunteer-list/volunteer-list.component';
import { VolunteerCalendarComponent } from './volunteer-calendar/volunteer-calendar.component';

const routes: Routes = [
  { path: '', component: VolunteerListComponent },
  { path: 'calendar', component: VolunteerCalendarComponent }
];

@NgModule({
  declarations: [
    VolunteerListComponent,
    VolunteerCalendarComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule
  ]
})
export class VolunteeringModule {}
