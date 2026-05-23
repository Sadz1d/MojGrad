import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class ThemeService {

  private readonly key = 'mojgrad_dark_mode';
  private darkMode$ = new BehaviorSubject<boolean>(this.loadFromStorage());

  isDark$ = this.darkMode$.asObservable();

  get isDark(): boolean {
    return this.darkMode$.value;
  }

  constructor() {
    // Primijeni temu odmah pri pokretanju
    this.applyTheme(this.isDark);
  }

  toggle(): void {
    const newValue = !this.darkMode$.value;
    this.darkMode$.next(newValue);
    this.applyTheme(newValue);
    localStorage.setItem(this.key, String(newValue));
  }

  private applyTheme(dark: boolean): void {
    if (dark) {
      document.body.classList.add('dark-mode');
    } else {
      document.body.classList.remove('dark-mode');
    }
  }

  private loadFromStorage(): boolean {
    return localStorage.getItem(this.key) === 'true';
  }
}
