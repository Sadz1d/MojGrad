import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SurveyListComponent } from './survey-list/survey-list.component';
import { SurveyStatsComponent } from './survey-stats/survey-stats.component';
import { SurveyVoteComponent } from './survey-vote/survey-vote.component';

const routes: Routes = [
  { path: '', component: SurveyListComponent },
  { path: ':id/stats', component: SurveyStatsComponent },
  { path: ':id/vote', component: SurveyVoteComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SurveysRoutingModule {}
