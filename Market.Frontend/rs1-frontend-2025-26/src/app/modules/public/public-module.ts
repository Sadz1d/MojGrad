import {NgModule} from '@angular/core';
import { CommonModule } from '@angular/common';
import {PublicRoutingModule} from './public-routing-module';
import {PublicLayoutComponent} from './public-layout/public-layout.component';
import {SearchProductsComponent} from './search-products/search-products.component';
import {SharedModule} from '../shared/shared-module';


@NgModule({
  declarations: [
    PublicLayoutComponent,
    SearchProductsComponent
  ],
  imports: [
    SharedModule,
    PublicRoutingModule,
    CommonModule
  ]
})
export class PublicModule { }
