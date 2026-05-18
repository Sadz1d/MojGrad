import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SurveyListComponent } from './survey-list/survey-list.component';
import { SurveyStatsComponent } from './survey-stats/survey-stats.component';

const routes: Routes = [
  {
    path: '',
    component: SurveyListComponent
  },
  {
    path: ':id/stats',
    component: SurveyStatsComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SurveysRoutingModule {}
