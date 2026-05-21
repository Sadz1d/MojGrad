import { Component, OnInit, OnDestroy } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { forkJoin } from 'rxjs';
import { VolunteerActionService } from '../../../core/services/volunteer-action.service';
import { SurveyService } from '../../../core/services/survey.service';
import * as XLSX from 'xlsx';

Chart.register(...registerables);

@Component({
  selector: 'app-statistics-dashboard',
  standalone: false,
  templateUrl: './statistics-dashboard.component.html',
  styleUrls: ['./statistics-dashboard.component.scss']
})
export class StatisticsDashboardComponent implements OnInit, OnDestroy {

  // Podaci
  allActions: any[] = [];
  allSurveys: any[] = [];
  filteredActions: any[] = [];
  filteredSurveys: any[] = [];

  // Statistike – volontiranje
  totalActions = 0;
  totalParticipants = 0;
  availableSlots = 0;
  fullActions = 0;

  // Statistike – ankete
  totalSurveys = 0;
  activeSurveys = 0;
  totalResponses = 0;
  avgResponses = 0;

  // Filteri
  dateFrom = '';
  dateTo = '';

  loading = true;
  exporting = false;

  private barChart: Chart | null = null;
  private doughnutChart: Chart | null = null;
  private surveyChart: Chart | null = null;
  private trendChart: Chart | null = null;

  private readonly colors = [
    '#1e88e5', '#42a5f5', '#1565c0', '#64b5f6',
    '#0d47a1', '#90caf9', '#1976d2', '#bbdefb',
    '#1e88e5', '#42a5f5'
  ];

  constructor(
    private volunteerService: VolunteerActionService,
    private surveyService: SurveyService
  ) {}

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.loading = true;

    forkJoin({
      volunteers: this.volunteerService.getActions(1, 100),
      surveys: this.surveyService.getAll({} as any)
    }).subscribe({
      next: ({ volunteers, surveys }) => {
        this.allActions = volunteers.items;
        this.allSurveys = surveys.items;
        this.loading = false;
        this.applyFilters();
      },
      error: () => { this.loading = false; }
    });
  }

  applyFilters(): void {
    let actions = [...this.allActions];
    let surveys = [...this.allSurveys];

    if (this.dateFrom) {
      const from = new Date(this.dateFrom);
      actions = actions.filter(a => new Date(a.eventDate) >= from);
      surveys = surveys.filter(s => new Date(s.startDate) >= from);
    }

    if (this.dateTo) {
      const to = new Date(this.dateTo);
      actions = actions.filter(a => new Date(a.eventDate) <= to);
      surveys = surveys.filter(s => new Date(s.endDate) <= to);
    }

    this.filteredActions = actions;
    this.filteredSurveys = surveys;

    // Izračunaj statistike
    this.totalActions = actions.length;
    this.totalParticipants = actions.reduce((s, a) => s + a.participantsCount, 0);
    this.availableSlots = actions.reduce((s, a) => s + a.freeSlots, 0);
    this.fullActions = actions.filter(a => a.freeSlots === 0).length;

    this.totalSurveys = surveys.length;
    this.activeSurveys = surveys.filter(s => s.isActive).length;
    this.totalResponses = surveys.reduce((s, a) => s + (a.responsesCount ?? 0), 0);
    this.avgResponses = this.totalSurveys > 0
      ? Math.round(this.totalResponses / this.totalSurveys)
      : 0;

    setTimeout(() => {
      this.renderBarChart();
      this.renderDoughnutChart();
      this.renderSurveyChart();
      this.renderTrendChart();
    }, 100);
  }

  clearFilters(): void {
    this.dateFrom = '';
    this.dateTo = '';
    this.applyFilters();
  }

  // ── GRAFIKONI ──────────────────────────────────────

  private renderBarChart(): void {
    const canvas = document.getElementById('barChart') as HTMLCanvasElement;
    if (!canvas) return;
    if (this.barChart) this.barChart.destroy();

    const top10 = this.filteredActions.slice(0, 10);

    this.barChart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: top10.map(a => a.name.length > 15 ? a.name.substring(0, 15) + '...' : a.name),
        datasets: [{
          label: 'Broj prijava',
          data: top10.map(a => a.participantsCount),
          backgroundColor: this.colors,
          borderRadius: 10,
          borderSkipped: false,
          borderWidth: 0
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: {
            backgroundColor: '#0d47a1',
            padding: 12,
            cornerRadius: 10,
            callbacks: {
              title: (items) => top10[items[0].dataIndex]?.name ?? '',
              label: (ctx) => ` ${ctx.parsed.y} prijava`
            }
          }
        },
        scales: {
          y: {
            beginAtZero: true,
            grid: { color: '#e3f2fd' },
            ticks: { color: '#546e7a', stepSize: 1 }
          },
          x: {
            grid: { display: false },
            ticks: { color: '#1e3a5f', font: { weight: 'bold' } }
          }
        }
      }
    });
  }

  private renderDoughnutChart(): void {
    const canvas = document.getElementById('doughnutChart') as HTMLCanvasElement;
    if (!canvas) return;
    if (this.doughnutChart) this.doughnutChart.destroy();

    this.doughnutChart = new Chart(canvas, {
      type: 'doughnut',
      data: {
        labels: ['Zauzeto', 'Slobodno'],
        datasets: [{
          data: [this.totalParticipants, this.availableSlots],
          backgroundColor: ['#1e88e5', '#e3f2fd'],
          borderColor: '#fff',
          borderWidth: 3,
          hoverOffset: 10
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        cutout: '68%',
        plugins: {
          legend: {
            position: 'bottom',
            labels: { color: '#1e3a5f', font: { size: 13 }, padding: 16, usePointStyle: true }
          },
          tooltip: { backgroundColor: '#0d47a1', padding: 12, cornerRadius: 10 }
        }
      } as any
    });
  }

  private renderSurveyChart(): void {
    const canvas = document.getElementById('surveyChart') as HTMLCanvasElement;
    if (!canvas) return;
    if (this.surveyChart) this.surveyChart.destroy();

    const top8 = this.filteredSurveys.slice(0, 8);

    this.surveyChart = new Chart(canvas, {
      type: 'bar',
      data: {
        labels: top8.map(s => s.question.length > 20 ? s.question.substring(0, 20) + '...' : s.question),
        datasets: [{
          label: 'Broj odgovora',
          data: top8.map(s => s.responsesCount ?? 0),
          backgroundColor: top8.map((_, i) => this.colors[i % this.colors.length]),
          borderRadius: 10,
          borderSkipped: false,
          borderWidth: 0
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        indexAxis: 'y',
        plugins: {
          legend: { display: false },
          tooltip: {
            backgroundColor: '#0d47a1',
            padding: 12,
            cornerRadius: 10,
            callbacks: {
              title: (items) => top8[items[0].dataIndex]?.question ?? '',
              label: (ctx) => ` ${ctx.parsed.x} odgovora`
            }
          }
        },
        scales: {
          x: { beginAtZero: true, grid: { color: '#e3f2fd' }, ticks: { color: '#546e7a', stepSize: 1 } },
          y: { grid: { display: false }, ticks: { color: '#1e3a5f', font: { weight: 'bold' } } }
        }
      }
    });
  }

  private renderTrendChart(): void {
    const canvas = document.getElementById('trendChart') as HTMLCanvasElement;
    if (!canvas) return;
    if (this.trendChart) this.trendChart.destroy();

    // Grupiši akcije po mjesecu
    const monthMap: { [key: string]: number } = {};

    this.filteredActions.forEach(a => {
      const d = new Date(a.eventDate);
      const key = `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}`;
      monthMap[key] = (monthMap[key] ?? 0) + a.participantsCount;
    });

    const sortedKeys = Object.keys(monthMap).sort();
    const labels = sortedKeys.map(k => {
      const [y, m] = k.split('-');
      const months = ['Jan', 'Feb', 'Mar', 'Apr', 'Maj', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dec'];
      return `${months[parseInt(m) - 1]} ${y}`;
    });
    const data = sortedKeys.map(k => monthMap[k]);

    this.trendChart = new Chart(canvas, {
      type: 'line',
      data: {
        labels,
        datasets: [{
          label: 'Prijave po mjesecu',
          data,
          borderColor: '#1e88e5',
          backgroundColor: 'rgba(30, 136, 229, 0.12)',
          borderWidth: 3,
          pointBackgroundColor: '#1e88e5',
          pointRadius: 6,
          pointHoverRadius: 9,
          fill: true,
          tension: 0.4
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { display: false },
          tooltip: {
            backgroundColor: '#0d47a1',
            padding: 12,
            cornerRadius: 10,
            callbacks: { label: (ctx) => ` ${ctx.parsed.y} prijava` }
          }
        },
        scales: {
          y: { beginAtZero: true, grid: { color: '#e3f2fd' }, ticks: { color: '#546e7a', stepSize: 1 } },
          x: { grid: { display: false }, ticks: { color: '#1e3a5f' } }
        }
      }
    });
  }

  // ── EXPORT ──────────────────────────────────────────

  exportExcel(): void {
    this.exporting = true;

    // Sheet 1 – Volonterske akcije
    const actionsData = this.filteredActions.map(a => ({
      'Naziv': a.name,
      'Opis': a.description,
      'Lokacija': a.location,
      'Datum': new Date(a.eventDate).toLocaleDateString('bs-BA'),
      'Maks. volontera': a.maxParticipants,
      'Prijavljenih': a.participantsCount,
      'Slobodnih mjesta': a.freeSlots,
      'Status': a.freeSlots === 0 ? 'Popunjeno' : 'Dostupno'
    }));

    // Sheet 2 – Ankete
    const surveysData = this.filteredSurveys.map(s => ({
      'Pitanje': s.question,
      'Datum početka': s.startDate,
      'Datum završetka': s.endDate,
      'Broj odgovora': s.responsesCount ?? 0,
      'Status': s.isActive ? 'Aktivna' : 'Neaktivna'
    }));

    // Sheet 3 – Sumarni izvještaj
    const summaryData = [
      { 'Kategorija': 'Ukupno akcija', 'Vrijednost': this.totalActions },
      { 'Kategorija': 'Ukupno prijava', 'Vrijednost': this.totalParticipants },
      { 'Kategorija': 'Slobodnih mjesta', 'Vrijednost': this.availableSlots },
      { 'Kategorija': 'Popunjenih akcija', 'Vrijednost': this.fullActions },
      { 'Kategorija': '—', 'Vrijednost': '' },
      { 'Kategorija': 'Ukupno anketa', 'Vrijednost': this.totalSurveys },
      { 'Kategorija': 'Aktivnih anketa', 'Vrijednost': this.activeSurveys },
      { 'Kategorija': 'Ukupno odgovora', 'Vrijednost': this.totalResponses },
      { 'Kategorija': 'Prosj. odgovora po anketi', 'Vrijednost': this.avgResponses },
    ];

    const wb = XLSX.utils.book_new();

    const ws1 = XLSX.utils.json_to_sheet(actionsData);
    ws1['!cols'] = [{ wch: 30 }, { wch: 40 }, { wch: 20 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 18 }, { wch: 12 }];
    XLSX.utils.book_append_sheet(wb, ws1, 'Volonterske akcije');

    const ws2 = XLSX.utils.json_to_sheet(surveysData);
    ws2['!cols'] = [{ wch: 50 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 12 }];
    XLSX.utils.book_append_sheet(wb, ws2, 'Ankete');

    const ws3 = XLSX.utils.json_to_sheet(summaryData);
    ws3['!cols'] = [{ wch: 30 }, { wch: 15 }];
    XLSX.utils.book_append_sheet(wb, ws3, 'Sumarni izvještaj');

    const period = this.dateFrom && this.dateTo
      ? `_${this.dateFrom}_do_${this.dateTo}`
      : '';

    XLSX.writeFile(wb, `statistike-mojgrad${period}.xlsx`);
    this.exporting = false;
  }

  exportCSV(): void {
    const headers = ['Naziv', 'Lokacija', 'Datum', 'Prijavljenih', 'Slobodnih mjesta'];
    const rows = this.filteredActions.map(a => [
      a.name, a.location,
      new Date(a.eventDate).toLocaleDateString('bs-BA'),
      a.participantsCount, a.freeSlots
    ]);

    const csv = [headers, ...rows]
      .map(r => r.map(c => `"${c}"`).join(','))
      .join('\n');

    const blob = new Blob(['\uFEFF' + csv], { type: 'text/csv;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'statistike-akcije.csv';
    a.click();
    URL.revokeObjectURL(url);
  }

  ngOnDestroy(): void {
    this.barChart?.destroy();
    this.doughnutChart?.destroy();
    this.surveyChart?.destroy();
    this.trendChart?.destroy();
  }
}
