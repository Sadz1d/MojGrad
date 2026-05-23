// src/app/modules/public/public-layout/public-layout.component.ts
import { Component, inject, OnInit, AfterViewInit, OnDestroy, HostListener } from '@angular/core';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { ProblemReportService } from '../../../core/services/problem-report.service';
import { ProblemReportListItem } from '../../../core/models/problem-report.model';
import { VolunteerActionService } from '../../../core/services/volunteer-action.service';
import { VolunteerActionListItem } from '../../../core/models/volunteer-action.model';
import { SurveyService } from '../../../core/services/survey.service';
import { Router } from '@angular/router';
import { NotificationService, AppNotification } from '../../../core/services/notification.service';
import { ThemeService } from '../../../core/services/theme.service';
declare const L: any;

@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.scss',
})
export class PublicLayoutComponent implements OnInit, AfterViewInit, OnDestroy {
  private auth = inject(AuthFacadeService);
  private problemReportService = inject(ProblemReportService);
  private volunteerService = inject(VolunteerActionService);
  private surveyService = inject(SurveyService);
  private router = inject(Router);
  public notifService = inject(NotificationService);
  public themeService = inject(ThemeService);

  currentYear: string = '2025';
  isLoggedIn$ = this.auth.isAuthenticated$;
  newestReports: ProblemReportListItem[] = [];
  upcomingActions: VolunteerActionListItem[] = [];

  readonly apiBase = 'https://localhost:7260';

  // Brojači
  counterActions = 0;
  counterVolunteers = 0;
  counterSurveys = 0;
  counterProblems = 0;

  private targetActions = 0;
  private targetVolunteers = 0;
  private targetSurveys = 0;
  private targetProblems = 0;

  private map: any = null;
  private mapInitialized = false;

  private readonly defaultLat = 43.3438;
  private readonly defaultLng = 17.8078;

  private readonly cityLocations = [
    { lat: 43.3438, lng: 17.8078 }, { lat: 43.3500, lng: 17.8100 },
    { lat: 43.3380, lng: 17.8150 }, { lat: 43.3460, lng: 17.7980 },
    { lat: 43.3520, lng: 17.8200 }, { lat: 43.3350, lng: 17.8050 },
    { lat: 43.3480, lng: 17.8250 }, { lat: 43.3400, lng: 17.8300 },
  ];

  // ── NOTIFIKACIJE ──
  notifOpen = false;
  notifications: AppNotification[] = [];

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.notif-wrap')) {
      this.notifOpen = false;
    }
  }

  ngOnInit(): void {
    this.loadProblems();
    this.loadVolunteerData();
    this.loadSurveyData();

    // Prati notifikacije
    this.notifService.notifications$.subscribe(n => {
      this.notifications = n;
    });
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.initMap(), 500);
  }

  // ── NOTIFIKACIJE METODE ──────────────────────────

  toggleNotif(): void {
    this.notifOpen = !this.notifOpen;
  }

  markRead(id: string): void {
    this.notifService.markAsRead(id);
  }

  markAllRead(): void {
    this.notifService.markAllAsRead();
  }

  deleteNotif(id: string, event: MouseEvent): void {
    event.stopPropagation();
    this.notifService.delete(id);
  }

  clearAll(): void {
    this.notifService.clearAll();
  }

  navigateNotif(notif: AppNotification): void {
    this.notifService.markAsRead(notif.id);
    this.notifOpen = false;
    if (notif.link) this.router.navigate([notif.link]);
  }

  timeAgo(date: Date): string {
    const diff = Math.floor((Date.now() - new Date(date).getTime()) / 1000);
    if (diff < 60) return 'Upravo';
    if (diff < 3600) return `${Math.floor(diff / 60)} min`;
    if (diff < 86400) return `${Math.floor(diff / 3600)} h`;
    return `${Math.floor(diff / 86400)} d`;
  }

  // ── UČITAVANJE PODATAKA ──────────────────────────

  private loadProblems(): void {
    this.problemReportService.getReports({
      page: 1, pageSize: 4, sortBy: 'creationdate', sortDirection: 'desc'
    }).subscribe({
      next: (result) => {
        this.newestReports = result.items ?? [];
        this.targetProblems = this.newestReports.length;
        this.animateCounter('problems');
      },
      error: () => { this.newestReports = []; }
    });
  }

  private loadVolunteerData(): void {
    this.volunteerService.getActions(1, 50).subscribe({
      next: (res) => {
        const all = res.items ?? [];
        const today = new Date();
        this.upcomingActions = all
          .filter(a => new Date(a.eventDate) >= today)
          .sort((a, b) => new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime())
          .slice(0, 3);

        this.targetActions = all.length;
        this.targetVolunteers = all.reduce((s, a) => s + a.participantsCount, 0);
        this.animateCounter('actions');
        this.animateCounter('volunteers');

        if (this.mapInitialized) this.addMarkers(all);
        else setTimeout(() => this.addMarkers(all), 1000);
      },
      error: () => {}
    });
  }

  private loadSurveyData(): void {
    this.surveyService.getAll({}).subscribe({
      next: (res) => {
        this.targetSurveys = res.items?.filter(s => s.isActive).length ?? 0;
        this.animateCounter('surveys');
      },
      error: () => {}
    });
  }

  // ── MAPA ────────────────────────────────────────

  private initMap(): void {
    if (typeof L === 'undefined' || this.mapInitialized) return;
    const mapEl = document.getElementById('volunteer-map');
    if (!mapEl) return;

    this.map = L.map('volunteer-map', {
      center: [this.defaultLat, this.defaultLng],
      zoom: 13, zoomControl: true, scrollWheelZoom: false
    });

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors', maxZoom: 18
    }).addTo(this.map);

    this.mapInitialized = true;
  }

  private addMarkers(actions: VolunteerActionListItem[]): void {
    if (!this.map || typeof L === 'undefined') return;

    const customIcon = L.divIcon({
      html: `<div style="background:linear-gradient(135deg,#1e88e5,#1565c0);width:36px;height:36px;border-radius:50% 50% 50% 0;transform:rotate(-45deg);border:3px solid #fff;box-shadow:0 4px 12px rgba(30,136,229,0.5);display:flex;align-items:center;justify-content:center;"><span style="transform:rotate(45deg);font-size:16px;">🤝</span></div>`,
      className: '', iconSize: [36, 36], iconAnchor: [18, 36]
    });

    actions.forEach((action, index) => {
      const loc = this.cityLocations[index % this.cityLocations.length];
      const popup = `<div style="min-width:200px;font-family:sans-serif;">
        <h3 style="color:#0d47a1;margin:0 0 8px;font-size:15px;">${action.name}</h3>
        <p style="margin:4px 0;color:#546e7a;font-size:13px;">📍 ${action.location}</p>
        <p style="margin:4px 0;color:#546e7a;font-size:13px;">📅 ${new Date(action.eventDate).toLocaleDateString('bs-BA')}</p>
        <p style="margin:4px 0;color:#546e7a;font-size:13px;">👥 ${action.participantsCount}/${action.maxParticipants}</p>
        <a href="/volunteering" style="display:inline-block;margin-top:10px;padding:6px 14px;background:#1e88e5;color:#fff;border-radius:20px;text-decoration:none;font-size:12px;font-weight:700;">Prijavi se →</a>
      </div>`;
      L.marker([loc.lat, loc.lng], { icon: customIcon }).addTo(this.map).bindPopup(popup, { maxWidth: 260 });
    });
  }

  // ── ANIMIRANI BROJAČI ────────────────────────────

  private animateCounter(type: 'actions' | 'volunteers' | 'surveys' | 'problems'): void {
    const steps = 50;
    let target: number;
    switch (type) {
      case 'actions':    target = this.targetActions; break;
      case 'volunteers': target = this.targetVolunteers; break;
      case 'surveys':    target = this.targetSurveys; break;
      case 'problems':   target = this.targetProblems; break;
    }

    let current = 0;
    const step = Math.ceil(target / steps);
    const timer = setInterval(() => {
      current = Math.min(current + step, target);
      switch (type) {
        case 'actions':    this.counterActions = current; break;
        case 'volunteers': this.counterVolunteers = current; break;
        case 'surveys':    this.counterSurveys = current; break;
        case 'problems':   this.counterProblems = current; break;
      }
      if (current >= target) clearInterval(timer);
    }, 1500 / steps);
  }

  // ── HELPERS ─────────────────────────────────────

  getImageUrl(imagePath?: string): string | null {
    if (!imagePath) return null;
    return `${this.apiBase}${imagePath}`;
  }

  getStatusClass(status: string): string {
    const s = status?.toLowerCase();
    if (s === 'novo' || s === 'new') return 'status-new';
    if (s === 'u toku' || s === 'in progress') return 'status-progress';
    if (s === 'riješeno' || s === 'resolved' || s === 'done') return 'status-done';
    return 'status-new';
  }

  getStatusLabel(status: string): string {
    const s = status?.toLowerCase();
    if (s === 'novo' || s === 'new') return 'NOVO';
    if (s === 'u toku' || s === 'in progress') return 'U TOKU';
    if (s === 'riješeno' || s === 'resolved' || s === 'done') return 'RIJEŠENO';
    return status?.toUpperCase() ?? 'NOVO';
  }

  logout(): void { this.auth.logout().subscribe(); }
  get currentUser() { return this.auth.getCurrentUserValue(); }
  get isAdmin(): boolean { return this.auth.isAdmin(); }

  ngOnDestroy(): void {
    if (this.map) { this.map.remove(); this.map = null; }
  }
}
