import { NgModule, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideAnimations} from '@angular/platform-browser/animations';
import {HTTP_INTERCEPTORS, HttpClient, provideHttpClient, withInterceptors} from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing-module';
import { AppComponent } from './app.component';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';
import {loadingBarInterceptor} from './core/interceptors/loading-bar-interceptor.service';
import {errorLoggingInterceptor} from './core/interceptors/error-logging-interceptor.service';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {CustomTranslateLoader} from './core/services/custom-translate-loader';
import {materialModules} from './modules/shared/material-modules';
import {SharedModule} from './modules/shared/shared-module';
import { TestAuthComponent } from './pages/test-auth/test-auth.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProblemReportService } from './core/services/problem-report.service';
import { ProblemReportListComponent } from './core/components/problem-report-list/problem-report-list.component';
import { ProblemReportFormComponent } from './core/components/problem-report-form/problem-report-form.component';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    AppComponent,
    
  ],
  imports: [
    BrowserAnimationsModule,
    HttpClientModule,
    TestAuthComponent,
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    RouterModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (http: HttpClient) => new CustomTranslateLoader(http),
        deps: [HttpClient]
      }
    }),
    SharedModule,
    materialModules,
    ProblemReportListComponent,
    ProblemReportFormComponent
  ],
  providers: [{provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true},
    provideAnimations(),
    ProblemReportService,
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection(),
    provideHttpClient(
      withInterceptors([
        loadingBarInterceptor,
        
        errorLoggingInterceptor
      ])
    )
  ],
  exports: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }