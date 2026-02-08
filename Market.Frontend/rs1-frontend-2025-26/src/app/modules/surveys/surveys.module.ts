import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveysRoutingModule } from './surveys-routing.module';
import { SurveyListComponent } from './survey-list/survey-list.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    SurveyListComponent
  ],
  imports: [
    CommonModule,
    SurveysRoutingModule,
    FormsModule
  ]
})
export class SurveysModule {}
