import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { ProductsComponent } from './catalogs/products/products.component';
import { ProductsAddComponent } from './catalogs/products/products-add/products-add.component';
import { ProductsEditComponent } from './catalogs/products/products-edit/products-edit.component';
import { ProductCategoriesComponent } from './catalogs/product-categories/product-categories.component';
import {AdminOrdersComponent} from './orders/admin-orders.component';
import {AdminSettingsComponent} from './admin-settings/admin-settings.component';
import { VolunteerActionCreateComponent } from './volunteer-action-create/volunteer-action-create.component';
import {SurveyCreateComponent} from './surveys/survey-create/survey-create.component';
import { ProblemCategoriesComponent } from './problem-categories/problem-categories.component';
import { ProblemStatusesComponent } from './problem-statuses/problem-statuses.component';
import { AdminUsersComponent } from './users/admin-users.component';


const routes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    children: [
      // PRODUCTS
      {
        path: 'products',
        component: ProductsComponent,
      },
      {
        path: 'products/add',
        component: ProductsAddComponent,
      },
      {
        path: 'products/:id/edit',
        component: ProductsEditComponent,
      },

      // PRODUCT CATEGORIES
      {
        path: 'product-categories',
        component: ProductCategoriesComponent,
      },

      {
        path: 'orders',
        component: AdminOrdersComponent,
      },

      {
        path: 'settings',
        component: AdminSettingsComponent,
      },

      {
        path: 'surveys/create',
        component: SurveyCreateComponent
      },
      {
        path: 'surveys/edit/:id',
        component: SurveyCreateComponent
      },
      {
        path: 'volunteer-actions/create',
        component: VolunteerActionCreateComponent
      },
      {
        path: 'volunteer-actions/edit/:id',
        component: VolunteerActionCreateComponent
      },
      // PROBLEM REPORTS — ADMIN
      { path: 'problem-categories', component: ProblemCategoriesComponent },
      { path: 'problem-statuses',   component: ProblemStatusesComponent },
      { path: 'users',              component: AdminUsersComponent },

      // default admin route → dashboard
      {
        path: '',
        redirectTo: 'problem-categories',
        pathMatch: 'full',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminRoutingModule {}
