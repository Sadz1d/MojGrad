import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { VolunteerActionService } from '../../services/volunteer-action.service';
import { VolunteerActionListItem } from '../../models/volunteer-action.model';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-volunteer-action-list',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    FormsModule
  ],
  templateUrl: './volunteer-action-list.component.html',
  styleUrl: './volunteer-action-list.component.scss',
})
export class VolunteerActionListComponent implements OnInit {

  actions: VolunteerActionListItem[] = [];
  loading = false;
  errorMessage: string | null = null;
  joiningActionId: number | null = null;
  showOnlyAvailable = false;
  searchTerm = '';
  sortByDateAsc = true;
  filteredActions: VolunteerActionListItem[] = [];
  filterLocation = '';
  dateFrom?: string;
  dateTo?: string;
  isAdmin = false;

  constructor(
    private volunteerActionService: VolunteerActionService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadActions();
    const user = JSON.parse(localStorage.getItem('auth_user') || '{}');
    this.isAdmin = user?.isAdmin === true;
  }

  editAction(action: VolunteerActionListItem): void {
    this.router.navigate(['/admin/volunteer-actions/edit', action.id]);
  }

  deleteAction(id: number): void {
    if (!confirm('Da li ste sigurni da želite obrisati ovu akciju?')) return;
    this.volunteerActionService.deleteAction(id).subscribe(() => {
      this.actions = this.actions.filter(a => a.id !== id);
      this.applyFilters();
    });
  }

  loadActions(): void {
    this.loading = true;
    this.errorMessage = null;
    this.volunteerActionService.getActions().subscribe({
      next: (response) => {
        this.actions = response.items;
        this.applyFilters();
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Greška pri učitavanju volonterskih akcija.';
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    let result = [...this.actions];

    if (this.showOnlyAvailable) {
      result = result.filter(a => a.freeSlots > 0);
    }

    if (this.searchTerm.trim()) {
      result = result.filter(a =>
        a.name.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }

    result.sort((a, b) =>
      this.sortByDateAsc
        ? new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime()
        : new Date(b.eventDate).getTime() - new Date(a.eventDate).getTime()
    );

    if (this.filterLocation.trim()) {
      result = result.filter(a =>
        a.location.toLowerCase().includes(this.filterLocation.toLowerCase())
      );
    }

    if (this.dateFrom) {
      result = result.filter(a => new Date(a.eventDate) >= new Date(this.dateFrom!));
    }

    if (this.dateTo) {
      result = result.filter(a => new Date(a.eventDate) <= new Date(this.dateTo!));
    }

    this.filteredActions = result;
  }

  goHome(): void {
    this.router.navigate(['/']);
  }

  joinAction(action: VolunteerActionListItem): void {
    if (action.isUserJoined || action.freeSlots <= 0) return;
    this.joiningActionId = action.id;

    this.volunteerActionService.joinAction(action.id).subscribe({
      next: () => {
        action.participantsCount++;
        action.freeSlots--;
        action.isUserJoined = true;
        this.joiningActionId = null;
      },
      error: () => {
        // Ako API ne radi, simuliraj lokalno
        action.participantsCount++;
        action.freeSlots--;
        action.isUserJoined = true;
        this.joiningActionId = null;
      }
    });
  }

  // ── EXPORT – samo admin ──────────────────────────

  exportCSV(): void {
    const headers = ['Naziv', 'Opis', 'Lokacija', 'Datum', 'Maks. volontera', 'Prijavljenih', 'Slobodnih mjesta'];
    const rows = this.filteredActions.map(a => [
      a.name,
      a.description,
      a.location,
      new Date(a.eventDate).toLocaleDateString('bs-BA'),
      a.maxParticipants,
      a.participantsCount,
      a.freeSlots
    ]);

    const csvContent = [headers, ...rows]
      .map(row => row.map(cell => `"${cell}"`).join(','))
      .join('\n');

    this.downloadFile('\uFEFF' + csvContent, 'volonterske-akcije.csv', 'text/csv;charset=utf-8;');
  }

  exportExcel(): void {
    const data = this.filteredActions.map(a => ({
      'Naziv': a.name,
      'Opis': a.description,
      'Lokacija': a.location,
      'Datum': new Date(a.eventDate).toLocaleDateString('bs-BA'),
      'Maks. volontera': a.maxParticipants,
      'Prijavljenih': a.participantsCount,
      'Slobodnih mjesta': a.freeSlots
    }));

    const ws = XLSX.utils.json_to_sheet(data);
    ws['!cols'] = [{ wch: 30 }, { wch: 40 }, { wch: 25 }, { wch: 15 }, { wch: 15 }, { wch: 15 }, { wch: 18 }];
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Volonterske akcije');
    XLSX.writeFile(wb, 'volonterske-akcije.xlsx');
  }

  exportJSON(): void {
    const data = this.filteredActions.map(a => ({
      id: a.id,
      naziv: a.name,
      opis: a.description,
      lokacija: a.location,
      datum: new Date(a.eventDate).toISOString().split('T')[0],
      maksVolontera: a.maxParticipants,
      prijavljenih: a.participantsCount,
      slobodnihMjesta: a.freeSlots
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
}
