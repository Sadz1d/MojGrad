// src/app/core/guards/my-auth-guard.ts
import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthFacadeService } from '../services/auth/auth-facade.service';

export interface AuthGuardData {
  requireAuth: boolean;
  requireAdmin?: boolean;
  requireManager?: boolean;
  requireEmployee?: boolean;
  allowedRoles?: string[];
}

// Helper function to create route data
export function AuthData(data: AuthGuardData): AuthGuardData {
  return data;
}

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authFacade: AuthFacadeService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    
    const authData = route.data as AuthGuardData;
    
    // Ako ne zahtijeva autentikaciju, dozvoli pristup
    if (!authData?.requireAuth) {
      return true;
    }

    // Proveri da li je korisnik ulogovan
    const user = this.authFacade.getCurrentUserValue();
    if (!user) {
      // Nije ulogovan - redirect na login
      return this.router.createUrlTree(['/auth/login'], {
        queryParams: { returnUrl: state.url }
      });
    }

    // Provera za admina
    if (authData.requireAdmin && !user.isAdmin) {
      return this.router.createUrlTree(['/unauthorized']);
    }

    // Provera za managera
    if (authData.requireManager && !user.isManager && !user.isAdmin) {
      return this.router.createUrlTree(['/unauthorized']);
    }

    // Provera za employee
    if (authData.requireEmployee && !user.isEmployee && !user.isManager && !user.isAdmin) {
      return this.router.createUrlTree(['/unauthorized']);
    }

    // Provera za allowedRoles
    if (authData.allowedRoles && authData.allowedRoles.length > 0) {
      const hasAllowedRole = this.checkUserRoles(user, authData.allowedRoles);
      if (!hasAllowedRole) {
        return this.router.createUrlTree(['/unauthorized']);
      }
    }

    return true;
  }

  private checkUserRoles(user: any, allowedRoles: string[]): boolean {
    for (const role of allowedRoles) {
      switch (role.toLowerCase()) {
        case 'admin':
          if (user.isAdmin) return true;
          break;
        case 'manager':
          if (user.isManager || user.isAdmin) return true;
          break;
        case 'employee':
          if (user.isEmployee || user.isManager || user.isAdmin) return true;
          break;
        case 'user':
          // Svaki autentifikovani korisnik je "user"
          return true;
      }
    }
    return false;
  }
}