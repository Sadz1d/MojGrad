import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SurveyService } from '../../../../core/services/survey.service';

@Component({
  standalone:false,
  selector: 'app-survey-create',
  templateUrl: './survey-create.component.html',
  styleUrls: ['./survey-create.component.scss'],
})
export class SurveyCreateComponent {

  model = {
    question: '',
    startDate: '',
    endDate: ''
  };

  saving = false;

  constructor(
    private surveyService: SurveyService,
    private router: Router
  ) {}

  save(): void {
    this.surveyService.create(this.model).subscribe({
      next: () => {
        alert('Anketa uspješno sačuvana');
        this.router.navigate(['/surveys']);
      },
      error: () => {
        alert('Greška prilikom snimanja ankete');
      }
    });
  }
}
