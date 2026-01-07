// src/app/modules/public/public-layout/public-layout.component.ts
import { Component, inject } from '@angular/core';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-public-layout',
  standalone: false,
  templateUrl: './public-layout.component.html',
  styleUrl: './public-layout.component.scss',
})
export class PublicLayoutComponent {
  private auth = inject(AuthFacadeService);
  
  currentYear: string = "2025";
  
  // Observable za praćenje stanja prijave
  isLoggedIn$ = this.auth.isAuthenticated$;
  
  // Metoda za odjavu
  logout(): void {
    this.auth.logout().subscribe();
  }
  
  // Getter za trenutnog korisnika (za ime/email u dugmetu)
  get currentUser() {
    return this.auth.getCurrentUserValue();
  }
}