import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SurveysRoutingModule } from './surveys-routing.module';
import { SurveyListComponent } from './survey-list/survey-list.component';
import { SurveyStatsComponent } from './survey-stats/survey-stats.component';
import { SurveyVoteComponent } from './survey-vote/survey-vote.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    SurveyListComponent,
    SurveyStatsComponent,
    SurveyVoteComponent
  ],
  imports: [
    CommonModule,
    SurveysRoutingModule,
    FormsModule,
    RouterModule
  ]
})
export class SurveysModule {}
