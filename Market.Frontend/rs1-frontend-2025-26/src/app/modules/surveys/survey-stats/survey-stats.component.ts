import { Component, OnInit, AfterViewInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SurveyService } from '../../../core/services/survey.service';
import { Chart, ChartConfiguration, registerables } from 'chart.js';

Chart.register(...registerables);

export interface SurveyStatsDto {
  surveyId: number;
  question: string;
  totalResponses: number;
  startDate: string;
  endDate: string;
  isActive: boolean;
  options: SurveyOptionStat[];
}

export interface SurveyOptionStat {
  label: string;
  count: number;
  percentage: number;
}

@Component({
  selector: 'app-survey-stats',
  standalone: false,
  templateUrl: './survey-stats.component.html',
  styleUrls: ['./survey-stats.component.scss']
})
export class SurveyStatsComponent implements OnInit, AfterViewInit, OnDestroy {

  @ViewChild('barCanvas') barCanvas!: ElementRef<HTMLCanvasElement>;
  @ViewChild('doughnutCanvas') doughnutCanvas!: ElementRef<HTMLCanvasElement>;

  surveyId!: number;
  stats: SurveyStatsDto | null = null;
  loading = true;
  error = false;

  private barChart?: Chart;
  private doughnutChart?: Chart;

  // Lijepe boje za grafikone
  private readonly colors = [
    '#1e88e5', '#42a5f5', '#1565c0', '#64b5f6',
    '#0d47a1', '#90caf9', '#1976d2', '#bbdefb'
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private surveyService: SurveyService
  ) {}

  ngOnInit(): void {
    this.surveyId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadStats();
  }

  ngAfterViewInit(): void {}

  loadStats(): void {
    this.loading = true;
    this.error = false;

    // Učitavamo anketu, a stats simuliramo dok backend ne bude spreman
    this.surveyService.getById(this.surveyId).subscribe({
      next: (survey) => {
        // Simulirani podaci – zamijeni sa pravim API pozivom kada backend bude spreman
        this.stats = {
          surveyId: survey.id ?? this.surveyId,
          question: survey.question ?? 'Pitanje ankete',
          totalResponses: survey.responsesCount ?? 0,
          startDate: survey.startDate,
          endDate: survey.endDate,
          isActive: survey.isActive ?? false,
          options: this.generateMockOptions(survey.responsesCount ?? 0)
        };
        this.loading = false;
        setTimeout(() => this.renderCharts(), 100);
      },
      error: () => {
        this.error = true;
        this.loading = false;
      }
    });
  }

  // Kada imaš pravi stats endpoint, pozovi ga ovako:
  // loadStats(): void {
  //   this.surveyService.getStats(this.surveyId).subscribe({
  //     next: (stats) => { this.stats = stats; this.loading = false; setTimeout(() => this.renderCharts(), 100); },
  //     error: () => { this.error = true; this.loading = false; }
  //   });
  // }

  private generateMockOptions(total: number): SurveyOptionStat[] {
    const options = [
      { label: 'Odlično', count: 0, percentage: 0 },
      { label: 'Dobro', count: 0, percentage: 0 },
      { label: 'Zadovoljavajuće', count: 0, percentage: 0 },
      { label: 'Loše', count: 0, percentage: 0 },
    ];

    if (total === 0) return options;

    // Distribuiraj odgovore
    const counts = [
      Math.round(total * 0.45),
      Math.round(total * 0.30),
      Math.round(total * 0.15),
      Math.round(total * 0.10),
    ];

    return options.map((opt, i) => ({
      ...opt,
      count: counts[i],
      percentage: total > 0 ? Math.round((counts[i] / total) * 100) : 0
    }));
  }

  private renderCharts(): void {
    if (!this.stats || !this.barCanvas || !this.doughnutCanvas) return;

    const labels = this.stats.options.map(o => o.label);
    const data = this.stats.options.map(o => o.count);

    // ── BAR CHART ──
    if (this.barChart) this.barChart.destroy();

    const barConfig: ChartConfiguration = {
      type: 'bar',
      data: {
        labels,
        datasets: [{
          label: 'Broj odgovora',
          data,
          backgroundColor: this.colors.slice(0, labels.length),
          borderColor: this.colors.slice(0, labels.length).map(c => c),
          borderWidth: 0,
          borderRadius: 10,
          borderSkipped: false,
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: {
            backgroundColor: '#0d47a1',
            titleColor: '#fff',
            bodyColor: '#e3f2fd',
            padding: 12,
            cornerRadius: 10,
            callbacks: {
              label: (ctx) => ` ${ctx.parsed.y} odgovora`
            }
          }
        },
        scales: {
          y: {
            beginAtZero: true,
            grid: { color: '#e3f2fd' },
            ticks: {
              color: '#546e7a',
              font: { size: 13 },
              stepSize: 1
            }
          },
          x: {
            grid: { display: false },
            ticks: { color: '#1e3a5f', font: { size: 13, weight: 'bold' } }
          }
        }
      }
    };

    this.barChart = new Chart(this.barCanvas.nativeElement, barConfig);

    // ── DOUGHNUT CHART ──
    if (this.doughnutChart) this.doughnutChart.destroy();

    const doughnutConfig: any = {
      type: 'doughnut',
      data: {
        labels,
        datasets: [{
          data,
          backgroundColor: this.colors.slice(0, labels.length),
          borderColor: '#fff',
          borderWidth: 3,
          hoverOffset: 12
        }]
      },
      options: {
        cutout: '65%',
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              color: '#1e3a5f',
              font: { size: 13 },
              padding: 16,
              usePointStyle: true,
              pointStyleWidth: 10
            }
          },
          tooltip: {
            backgroundColor: '#0d47a1',
            titleColor: '#fff',
            bodyColor: '#e3f2fd',
            padding: 12,
            cornerRadius: 10,
            callbacks: {
              label: (ctx:any) => {
                const pct = this.stats!.totalResponses > 0
                  ? Math.round((Number(ctx.parsed) / this.stats!.totalResponses) * 100)
                  : 0;
                return ` ${ctx.parsed} odgovora (${pct}%)`;
              }
            }
          }
        }
      }
    };

    this.doughnutChart = new Chart(this.doughnutCanvas.nativeElement, doughnutConfig);
  }

  goBack(): void {
    this.router.navigate(['/surveys']);
  }

  private readonly dotColors = [
    '#1e88e5', '#42a5f5', '#1565c0', '#64b5f6',
    '#0d47a1', '#90caf9', '#1976d2', '#bbdefb'
  ];

  getDotColor(index: number): string {
    return this.dotColors[index % this.dotColors.length];
  }

  ngOnDestroy(): void {
    this.barChart?.destroy();
    this.doughnutChart?.destroy();
  }
}
