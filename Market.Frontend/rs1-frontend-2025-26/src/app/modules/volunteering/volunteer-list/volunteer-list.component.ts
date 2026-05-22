import { Component, OnInit } from '@angular/core';
import { VolunteerActionService } from '../../../core/services/volunteer-action.service';
import { VolunteerActionListItem } from '../../../core/models/volunteer-action.model';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import * as XLSX from 'xlsx';
import QRCode from 'qrcode';

@Component({
  selector: 'app-volunteer-list',
  standalone: false,
  templateUrl: './volunteer-list.component.html',
  styleUrls: ['./volunteer-list.component.scss']
})
export class VolunteerListComponent implements OnInit {

  actions: VolunteerActionListItem[] = [];
  filteredActions: VolunteerActionListItem[] = [];

  loading = false;
  joiningId: number | null = null;

  searchText = '';
  locationText = '';
  dateFrom = '';
  dateTo = '';
  onlyAvailable = false;

  // ── QR KOD ──
  qrModalVisible = false;
  qrImageUrl = '';
  qrActionName = '';

  constructor(
    private volunteerService: VolunteerActionService,
    public auth: AuthFacadeService
  ) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.volunteerService.getActions(1, 50).subscribe({
      next: (res) => {
        this.actions = res.items;
        this.applyFilters();
        this.loading = false;
      },
      error: () => { this.loading = false; }
    });
  }

  applyFilters(): void {
    let result = [...this.actions];

    if (this.searchText.trim()) {
      const search = this.searchText.toLowerCase().trim();
      result = result.filter(a =>
        a.name.toLowerCase().includes(search) ||
        a.description.toLowerCase().includes(search)
      );
    }

    if (this.locationText.trim()) {
      const location = this.locationText.toLowerCase().trim();
      result = result.filter(a =>
        a.location.toLowerCase().includes(location)
      );
    }

    if (this.dateFrom) {
      const from = new Date(this.dateFrom);
      result = result.filter(a => new Date(a.eventDate) >= from);
    }

    if (this.dateTo) {
      const to = new Date(this.dateTo);
      result = result.filter(a => new Date(a.eventDate) <= to);
    }

    if (this.onlyAvailable) {
      result = result.filter(a => a.freeSlots > 0);
    }

    this.filteredActions = result;
  }

  clearFilters(): void {
    this.searchText = '';
    this.locationText = '';
    this.dateFrom = '';
    this.dateTo = '';
    this.onlyAvailable = false;
    this.applyFilters();
  }

  join(action: VolunteerActionListItem): void {
    if (!this.auth.isLoggedIn()) {
      alert('Morate biti prijavljeni da biste se prijavili na akciju.');
      return;
    }
    if (action.freeSlots <= 0) return;

    this.joiningId = action.id;
    this.volunteerService.joinAction(action.id).subscribe({
      next: () => {
        action.isUserJoined = true;
        action.freeSlots--;
        action.participantsCount++;
        this.joiningId = null;
        this.applyFilters();
      },
      error: (err) => {
        alert(err?.error?.message || err?.error || 'Greška prilikom prijave.');
        this.joiningId = null;
      }
    });
  }

  // ── QR KOD METODE ──────────────────────────

  async showQR(action: VolunteerActionListItem): Promise<void> {
    const url = `${window.location.origin}/volunteering?action=${action.id}`;

    try {
      this.qrImageUrl = await QRCode.toDataURL(url, {
        width: 280,
        margin: 2,
        color: {
          dark: '#1f4e79',
          light: '#ffffff'
        }
      });
      this.qrActionName = action.name;
      this.qrModalVisible = true;
    } catch (err) {
      console.error('QR greška:', err);
    }
  }

  closeQR(): void {
    this.qrModalVisible = false;
    this.qrImageUrl = '';
    this.qrActionName = '';
  }

  downloadQR(): void {
    if (!this.qrImageUrl) return;
    const a = document.createElement('a');
    a.href = this.qrImageUrl;
    a.download = `qr-${this.qrActionName.replace(/\s+/g, '-').toLowerCase()}.png`;
    a.click();
  }

  // ── EXPORT ──────────────────────────────────

  exportCSV(): void {
    const headers = ['Naziv', 'Opis', 'Lokacija', 'Datum', 'Maks. volontera', 'Prijavljenih', 'Slobodnih mjesta'];
    const rows = this.filteredActions.map(a => [
      a.name, a.description, a.location,
      new Date(a.eventDate).toLocaleDateString('bs-BA'),
      a.maxParticipants, a.participantsCount, a.freeSlots
    ]);
    const csvContent = [headers, ...rows].map(row => row.map(cell => `"${cell}"`).join(',')).join('\n');
    this.downloadFile('\uFEFF' + csvContent, 'volonterske-akcije.csv', 'text/csv;charset=utf-8;');
  }

  exportExcel(): void {
    const data = this.filteredActions.map(a => ({
      'Naziv': a.name, 'Opis': a.description, 'Lokacija': a.location,
      'Datum': new Date(a.eventDate).toLocaleDateString('bs-BA'),
      'Maks. volontera': a.maxParticipants, 'Prijavljenih': a.participantsCount, 'Slobodnih mjesta': a.freeSlots
    }));
    const ws = XLSX.utils.json_to_sheet(data);
    ws['!cols'] = [{ wch: 30 }, { wch: 40 }, { wch: 25 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 18 }];
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Volonterske akcije');
    XLSX.writeFile(wb, 'volonterske-akcije.xlsx');
  }

  exportJSON(): void {
    const data = this.filteredActions.map(a => ({
      id: a.id, naziv: a.name, opis: a.description, lokacija: a.location,
      datum: new Date(a.eventDate).toISOString().split('T')[0],
      maksVolontera: a.maxParticipants, prijavljenih: a.participantsCount, slobodnihMjesta: a.freeSlots
    }));
    this.downloadFile(JSON.stringify(data, null, 2), 'volonterske-akcije.json', 'application/json');
  }

  private downloadFile(content: string, filename: string, mimeType: string): void {
    const blob = new Blob([content], { type: mimeType });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    URL.revokeObjectURL(url);
  }

  freePercent(a: VolunteerActionListItem): number {
    if (a.maxParticipants === 0) return 0;
    return Math.round((a.participantsCount / a.maxParticipants) * 100);
  }
}
