import { Component, OnInit } from '@angular/core';
import { SurveyService } from '../../../core/services/survey.service';
import { SurveyListItem } from '../../../core/models/survey.model';

@Component({
  selector: 'app-survey-list',
  standalone: false,
  templateUrl: './survey-list.component.html',
  styleUrls: ['./survey-list.component.scss'] // ⬅️ OVO JE FALILO
})
export class SurveyListComponent implements OnInit {

  surveys: SurveyListItem[] = [];
  loading = true;

  constructor(private surveyService: SurveyService) {}

  ngOnInit(): void {
    this.surveyService.getAll().subscribe({
      next: (res) => {
        this.surveys = res.items;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
