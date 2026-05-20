import { Component, OnInit } from '@angular/core';
import { VolunteerActionService } from '../../../core/services/volunteer-action.service';
import { VolunteerActionListItem } from '../../../core/models/volunteer-action.model';

interface CalendarDay {
  date: Date;
  dayNumber: number;
  isCurrentMonth: boolean;
  isToday: boolean;
  actions: VolunteerActionListItem[];
}

@Component({
  selector: 'app-volunteer-calendar',
  standalone: false,
  templateUrl: './volunteer-calendar.component.html',
  styleUrls: ['./volunteer-calendar.component.scss']
})
export class VolunteerCalendarComponent implements OnInit {
  actions: VolunteerActionListItem[] = [];
  calendarDays: CalendarDay[] = [];

  currentDate = new Date();
  selectedDate: Date | null = null;
  selectedActions: VolunteerActionListItem[] = [];

  monthNames = [
    'Januar', 'Februar', 'Mart', 'April', 'Maj', 'Juni',
    'Juli', 'August', 'Septembar', 'Oktobar', 'Novembar', 'Decembar'
  ];

  weekDays = ['Pon', 'Uto', 'Sri', 'Čet', 'Pet', 'Sub', 'Ned'];

  constructor(private volunteerService: VolunteerActionService) {}

  ngOnInit(): void {
    this.loadActions();
  }

  loadActions(): void {
    this.volunteerService.getActions(1, 100).subscribe({
      next: (res) => {
        this.actions = res.items;
        this.generateCalendar();
      },
      error: () => {
        alert('Greška prilikom učitavanja volonterskih akcija.');
      }
    });
  }

  generateCalendar(): void {
    this.calendarDays = [];

    const year = this.currentDate.getFullYear();
    const month = this.currentDate.getMonth();

    const firstDayOfMonth = new Date(year, month, 1);
    const lastDayOfMonth = new Date(year, month + 1, 0);

    let startDay = firstDayOfMonth.getDay();
    startDay = startDay === 0 ? 6 : startDay - 1;

    const startDate = new Date(year, month, 1 - startDay);

    for (let i = 0; i < 42; i++) {
      const date = new Date(startDate);
      date.setDate(startDate.getDate() + i);

      const actionsForDay = this.actions.filter(a =>
        this.isSameDate(new Date(a.eventDate), date)
      );

      this.calendarDays.push({
        date,
        dayNumber: date.getDate(),
        isCurrentMonth: date.getMonth() === month,
        isToday: this.isSameDate(date, new Date()),
        actions: actionsForDay
      });
    }
  }

  previousMonth(): void {
    this.currentDate = new Date(
      this.currentDate.getFullYear(),
      this.currentDate.getMonth() - 1,
      1
    );
    this.selectedDate = null;
    this.selectedActions = [];
    this.generateCalendar();
  }

  nextMonth(): void {
    this.currentDate = new Date(
      this.currentDate.getFullYear(),
      this.currentDate.getMonth() + 1,
      1
    );
    this.selectedDate = null;
    this.selectedActions = [];
    this.generateCalendar();
  }

  selectDay(day: CalendarDay): void {
    this.selectedDate = day.date;
    this.selectedActions = day.actions;
  }

  isSelected(day: CalendarDay): boolean {
    return !!this.selectedDate && this.isSameDate(day.date, this.selectedDate);
  }

  private isSameDate(date1: Date, date2: Date): boolean {
    return (
      date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate()
    );
  }
}
