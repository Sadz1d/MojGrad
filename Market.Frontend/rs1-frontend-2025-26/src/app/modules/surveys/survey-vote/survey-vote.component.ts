import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SurveyService } from '../../../core/services/survey.service';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-survey-vote',
  standalone: false,
  templateUrl: './survey-vote.component.html',
  styleUrls: ['./survey-vote.component.scss']
})
export class SurveyVoteComponent implements OnInit {

  surveyId!: number;
  survey: any = null;
  loading = true;
  submitting = false;
  submitted = false;
  error = false;
  errorMessage = '';

  selectedOption: string = '';

  // Opcije za glasanje – prilagodi po potrebi
  readonly options = [
    { value: 'Odlično', icon: '😄', color: '#1e88e5' },
    { value: 'Dobro', icon: '🙂', color: '#42a5f5' },
    { value: 'Zadovoljavajuće', icon: '😐', color: '#64b5f6' },
    { value: 'Loše', icon: '😞', color: '#90caf9' },
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private surveyService: SurveyService,
    private auth: AuthFacadeService
  ) {}

  ngOnInit(): void {
    this.surveyId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadSurvey();


    // Provjeri je li već glasao
    if (this.alreadyVoted()) {
      this.error = true;
      this.errorMessage = 'Već ste glasali na ovoj anketi!';
      this.loading = false;
    }
  }

  loadSurvey(): void {
    this.surveyService.getById(this.surveyId).subscribe({
      next: (s) => {
        this.survey = s;
        this.loading = false;

        // Blokiraj neaktivne ankete
        if (!s.isActive) {
          this.error = true;
          this.errorMessage = 'Ova anketa nije trenutno aktivna.';
        }
      },
      error: () => {
        this.error = true;
        this.loading = false;
      }
    });
  }

  select(option: string): void {
    this.selectedOption = option;
  }

  private alreadyVoted(): boolean {
    const key = `voted_survey_${this.surveyId}_user_${this.auth.currentUser()?.id}`;
    return localStorage.getItem(key) === 'true';
  }

  private markAsVoted(): void {
    const key = `voted_survey_${this.surveyId}_user_${this.auth.currentUser()?.id}`;
    localStorage.setItem(key, 'true');
  }

  submit(): void {
    if (!this.selectedOption || this.submitting) return;

    const userId = this.auth.currentUser()?.id;
    if (!userId) {
      alert('Morate biti prijavljeni da biste glasali.');
      this.router.navigate(['/auth/login']);
      return;
    }

    this.submitting = true;

    this.surveyService.createResponse({
      surveyId: this.surveyId,
      userId: userId,
      responseText: this.selectedOption
    }).subscribe({
      next: () => {
        this.markAsVoted();
        this.submitted = true;
        this.submitting = false;
        // Nakon 2 sekunde preusmjeri na rezultate
        setTimeout(() => {
          this.router.navigate(['/surveys', this.surveyId, 'stats']);
        }, 2000);
      },
      error: () => {
        this.submitting = false;
        alert('Greška prilikom slanja odgovora. Pokušajte ponovo.');
      }
    });
  }

  goBack(): void {
    this.router.navigate(['/surveys']);
  }
}
