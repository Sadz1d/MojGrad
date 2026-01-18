import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PointsComponent } from './pages/points/points.component';

const routes: Routes = [
  {
    path: 'points',
    component: PointsComponent
  }
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    PointsComponent   // 👈 OVO JE KLJUČNO
  ],
  exports: [RouterModule]
})
export class ClientRoutingModule {}



