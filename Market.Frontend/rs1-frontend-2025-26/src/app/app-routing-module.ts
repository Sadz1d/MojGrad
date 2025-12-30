import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { myAuthGuard, myAuthData } from './core/guards/my-auth-guard';
import { TestAuthComponent } from './pages/test-auth/test-auth.component';

const routes: Routes = [
  {
    path: 'admin',
    canActivate: [myAuthGuard],
    data: myAuthData({ requireAuth: true, requireAdmin: true }),
    loadChildren: () =>
      import('./modules/admin/admin-module').then(m => m.AdminModule)
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
    canActivate: [myAuthGuard],
    data: myAuthData({ requireAuth: true }), // bilo ko logiran
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
