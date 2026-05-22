import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SurveyService } from '../../../../core/services/survey.service';

@Component({
  selector: 'app-survey-create',
  standalone: false,
  templateUrl: './survey-create.component.html',
  styleUrls: ['./survey-create.component.scss'],
})
export class SurveyCreateComponent implements OnInit, OnDestroy {

  model = {
    question: '',
    startDate: '',
    endDate: ''
  };

  saving = false;
  isEdit = false;
  id: number | null = null;

  // ── AUTOSAVE ──
  autosaveStatus: 'idle' | 'saving' | 'saved' | 'error' = 'idle';
  private autosaveKey = 'autosave_survey';
  private autosaveTimer: any = null;

  constructor(
    private surveyService: SurveyService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');

    if (idParam) {
      this.isEdit = true;
      this.id = +idParam;
      this.autosaveKey = `autosave_survey_${this.id}`;

      this.surveyService.getById(this.id).subscribe(res => {
        this.model = {
          question: res.question,
          startDate: res.startDate,
          endDate: res.endDate
        };
      });
    } else {
      // Učitaj autosave ako postoji
      this.loadAutosave();
    }
  }

  // Pozovi ovu metodu na svaku promjenu inputa: (ngModelChange)="onFieldChange()"
  onFieldChange(): void {
    this.autosaveStatus = 'idle';
    clearTimeout(this.autosaveTimer);

    // Sačekaj 1.5 sekundi nakon što korisnik prestane tipkati
    this.autosaveTimer = setTimeout(() => {
      this.performAutosave();
    }, 1500);
  }

  private performAutosave(): void {
    if (!this.model.question && !this.model.startDate && !this.model.endDate) return;

    this.autosaveStatus = 'saving';

    try {
      localStorage.setItem(this.autosaveKey, JSON.stringify({
        model: this.model,
        savedAt: new Date().toISOString()
      }));
      this.autosaveStatus = 'saved';

      // Reset na idle nakon 3 sekunde
      setTimeout(() => {
        if (this.autosaveStatus === 'saved') this.autosaveStatus = 'idle';
      }, 3000);
    } catch {
      this.autosaveStatus = 'error';
    }
  }

  private loadAutosave(): void {
    try {
      const saved = localStorage.getItem(this.autosaveKey);
      if (saved) {
        const parsed = JSON.parse(saved);
        if (parsed.model) {
          this.model = parsed.model;
          this.autosaveStatus = 'saved';
        }
      }
    } catch {
      // ignore
    }
  }

  clearAutosave(): void {
    localStorage.removeItem(this.autosaveKey);
    this.model = { question: '', startDate: '', endDate: '' };
    this.autosaveStatus = 'idle';
  }

  get autosaveLabel(): string {
    switch (this.autosaveStatus) {
      case 'saving': return '💾 Snima se...';
      case 'saved':  return '✅ Automatski sačuvano';
      case 'error':  return '⚠️ Greška pri autosave';
      default:       return '';
    }
  }

  save(): void {
    this.saving = true;

    const request$ = this.isEdit && this.id
      ? this.surveyService.update(this.id, this.model)
      : this.surveyService.create(this.model);

    request$.subscribe({
      next: () => {
        localStorage.removeItem(this.autosaveKey);
        alert(this.isEdit ? 'Anketa ažurirana' : 'Anketa kreirana');
        this.router.navigate(['/admin/surveys']);
      },
      error: () => {
        this.saving = false;
        alert('Greška prilikom snimanja ankete');
      }
    });
  }

  ngOnDestroy(): void {
    clearTimeout(this.autosaveTimer);
  }
}
