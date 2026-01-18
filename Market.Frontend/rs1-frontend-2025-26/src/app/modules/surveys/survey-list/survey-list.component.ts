import { Component, OnInit } from '@angular/core';
import { SurveyService } from '../../../core/services/survey.service';
import { SurveyListItem } from '../../../core/models/survey.model';

@Component({
  selector: 'app-survey-list',
  standalone: false,
  templateUrl: './survey-list.component.html',
  styleUrls: ['./survey-list.component.scss']
})
export class SurveyListComponent implements OnInit {

  surveys: SurveyListItem[] = [];
  loading = false;

  filters = {
    search: '',
    activeOn: '',
    onlyActive: false,
    fromDate: '',
    toDate: ''
  };

  constructor(private surveyService: SurveyService) {}

  ngOnInit(): void {
    this.load();

  }

  load(): void {

    // ✅ FE VALIDACIJA
    if (this.filters.search && this.filters.search.length < 3) {
      return;
    }

    this.loading = true;

    this.surveyService.getAll(this.filters).subscribe({
      next: (res) => {
        this.surveys = res.items;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onOnlyActiveChange(): void {
    if (this.filters.onlyActive) {
      this.filters.activeOn = '';
    }
    this.load();
  }


}
