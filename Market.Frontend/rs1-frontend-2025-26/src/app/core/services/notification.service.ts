import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface AppNotification {
  id: string;
  title: string;
  message: string;
  icon: string;
  type: 'success' | 'info' | 'warning';
  read: boolean;
  createdAt: Date;
  link?: string;
}

@Injectable({ providedIn: 'root' })
export class NotificationService {

  private readonly storageKey = 'mojgrad_notifications';

  private notificationsSubject = new BehaviorSubject<AppNotification[]>(this.load());
  notifications$ = this.notificationsSubject.asObservable();

  get unreadCount(): number {
    return this.notificationsSubject.value.filter(n => !n.read).length;
  }

  get all(): AppNotification[] {
    return this.notificationsSubject.value;
  }

  add(title: string, message: string, icon: string, type: 'success' | 'info' | 'warning' = 'info', link?: string): void {
    const notification: AppNotification = {
      id: Date.now().toString(),
      title,
      message,
      icon,
      type,
      read: false,
      createdAt: new Date(),
      link
    };

    const current = this.notificationsSubject.value;
    const updated = [notification, ...current].slice(0, 20); // max 20
    this.save(updated);
    this.notificationsSubject.next(updated);
  }

  markAsRead(id: string): void {
    const updated = this.notificationsSubject.value.map(n =>
      n.id === id ? { ...n, read: true } : n
    );
    this.save(updated);
    this.notificationsSubject.next(updated);
  }

  markAllAsRead(): void {
    const updated = this.notificationsSubject.value.map(n => ({ ...n, read: true }));
    this.save(updated);
    this.notificationsSubject.next(updated);
  }

  delete(id: string): void {
    const updated = this.notificationsSubject.value.filter(n => n.id !== id);
    this.save(updated);
    this.notificationsSubject.next(updated);
  }

  clearAll(): void {
    this.save([]);
    this.notificationsSubject.next([]);
  }

  private load(): AppNotification[] {
    try {
      const raw = localStorage.getItem(this.storageKey);
      if (!raw) return [];
      const parsed = JSON.parse(raw);
      return parsed.map((n: any) => ({ ...n, createdAt: new Date(n.createdAt) }));
    } catch {
      return [];
    }
  }

  private save(notifications: AppNotification[]): void {
    try {
      localStorage.setItem(this.storageKey, JSON.stringify(notifications));
    } catch { }
  }
}
