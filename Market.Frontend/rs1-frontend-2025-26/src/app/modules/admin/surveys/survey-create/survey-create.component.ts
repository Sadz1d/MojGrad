import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SurveyService } from '../../../../core/services/survey.service';

@Component({
  selector: 'app-survey-create',
  standalone: false,
  templateUrl: './survey-create.component.html',
  styleUrls: ['./survey-create.component.scss'],
})
export class SurveyCreateComponent implements OnInit {

  model = {
    question: '',
    startDate: '',
    endDate: ''
  };

  saving = false;
  isEdit = false;
  id: number | null = null;

  constructor(
    private surveyService: SurveyService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  // ✅ OVO DODAJEŠ
  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.isEdit = true;
      this.id = +idParam;

      this.surveyService.getById(this.id).subscribe(res => {
        this.model = {
          question: res.question,
          startDate: res.startDate,
          endDate: res.endDate
        };
      });
    }
  }

  // ✅ OVO MIJENJAŠ
  save(): void {
    this.saving = true;

    const request$ = this.isEdit && this.id
      ? this.surveyService.update(this.id, this.model)
      : this.surveyService.create(this.model);

    request$.subscribe({
      next: () => {
        alert(this.isEdit ? 'Anketa ažurirana' : 'Anketa kreirana');
        this.router.navigate(['/admin/surveys']);
      },
      error: () => {
        this.saving = false;
        alert('Greška prilikom snimanja ankete');
      }
    });
  }
}
