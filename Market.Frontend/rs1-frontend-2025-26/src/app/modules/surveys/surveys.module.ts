import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveysRoutingModule } from './surveys-routing.module';
import { SurveyListComponent } from './survey-list/survey-list.component';

@NgModule({
  declarations: [
    SurveyListComponent
  ],
  imports: [
    CommonModule,
    SurveysRoutingModule
  ]
})
export class SurveysModule {}
