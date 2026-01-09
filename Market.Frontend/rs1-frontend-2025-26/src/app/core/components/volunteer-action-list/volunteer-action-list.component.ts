import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

import { VolunteerActionService } from '../../services/volunteer-action.service';
import { VolunteerActionListItem } from '../../models/volunteer-action.model';
import { FormsModule } from '@angular/forms';


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


  constructor(
    private volunteerActionService: VolunteerActionService
  ) {
  }

  ngOnInit(): void {
    this.loadActions();
  }

  loadActions(): void {
    this.loading = true;
    this.errorMessage = null;



      this.volunteerActionService.getActions().subscribe({
        next: (response) => {
          this.actions = response.items;
          this.applyFilters();   // 👈 OVDJE TAČNO IDE
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

    // samo akcije sa slobodnim mjestima
    if (this.showOnlyAvailable) {
      result = result.filter(a => a.freeSlots > 0);
    }

    // pretraga po nazivu
    if (this.searchTerm.trim()) {
      result = result.filter(a =>
        a.name.toLowerCase().includes(this.searchTerm.toLowerCase())
      );
    }

    // sortiranje po datumu
    result.sort((a, b) =>
      this.sortByDateAsc
        ? new Date(a.eventDate).getTime() - new Date(b.eventDate).getTime()
        : new Date(b.eventDate).getTime() - new Date(a.eventDate).getTime()
    );

    this.filteredActions = result;
  }


  // =============================
  // PRIJAVI SE
  // =============================
  joinAction(action: VolunteerActionListItem): void {

    if (action.freeSlots <= 0) return;

    this.joiningActionId = action.id;

    setTimeout(() => {
      action.participantsCount++;
      action.freeSlots--;
      this.joiningActionId = null;
    }, 800);
  }
}
