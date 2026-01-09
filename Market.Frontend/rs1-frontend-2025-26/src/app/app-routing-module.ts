import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard, AuthData } from './core/guards/my-auth-guard';
import { TestAuthComponent } from './pages/test-auth/test-auth.component';
import { ProblemReportListComponent } from './core/components/problem-report-list/problem-report-list.component';
import { ProblemReportFormComponent } from './core/components/problem-report-form/problem-report-form.component';
import { ProblemReportImportComponent } from './core/components/problem-report-import/problem-report-import.component';
import { VolunteerActionListComponent } from './core/components/volunteer-action-list/volunteer-action-list.component';

const routes: Routes = [
  {
    path: 'admin',
    canActivate: [AuthGuard],
    data: AuthData({ requireAuth: true, requireAdmin: true }),
    loadChildren: () =>
      import('./modules/admin/admin-module').then(m => m.AdminModule)
  },

  { path: 'problem-reports', component: ProblemReportListComponent },
  { path: 'problem-reports/new', component: ProblemReportFormComponent },
  { path: 'problem-reports/edit/:id', component: ProblemReportFormComponent },
  { path: 'problem-reports/import', component: ProblemReportImportComponent },
  { path: 'problem-reports/:id', component: ProblemReportFormComponent },

  {
    path: 'volunteering',
    component: VolunteerActionListComponent
  },

  // 🔓 test ruta (javna)
  {
    path: 'test-auth',
    component: TestAuthComponent
  },

  // 🔐 auth module (login, register, itd.)
  {
    path: 'auth',
    loadChildren: () =>
      import('./modules/auth/auth-module').then(m => m.AuthModule)
  },

  {
    path: 'client',
    canActivate: [AuthGuard],
    data: AuthData({ requireAuth: true }), // bilo ko logiran
    loadChildren: () =>
      import('./modules/client/client-module').then(m => m.ClientModule)
  },

  // 🌍 public module
  {
    path: '',
    loadChildren: () =>
      import('./modules/public/public-module').then(m => m.PublicModule)
  },

  // fallback
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
