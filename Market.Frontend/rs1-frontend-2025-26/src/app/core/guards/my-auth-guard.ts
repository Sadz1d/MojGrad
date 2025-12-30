import { inject } from '@angular/core';
import { CanActivateFn, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthFacadeService } from '../services/auth/auth-facade.service';

export const myAuthGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const authFacade = inject(AuthFacadeService);
  const router = inject(Router);

  // 👇 čitamo iz route.data.auth (profesor-style)
  const authData = route.data['auth'] ?? {};

  const requireAuth = authData.requireAuth === true;
  const requireAdmin = authData.requireAdmin === true;
  const requireManager = authData.requireManager === true;
  const requireEmployee = authData.requireEmployee === true;

  const isAuth = authFacade.isAuthenticated();

  // 1️⃣ ruta traži auth, a user NIJE logiran
  if (requireAuth && !isAuth) {
    return router.createUrlTree(['/auth/login']);
  }

  // 2️⃣ javna ruta
  if (!requireAuth) {
    return true;
  }

  const user = authFacade.currentUser();
  if (!user) {
    return router.createUrlTree(['/auth/login']);
  }

  // 3️⃣ role check
  if (requireAdmin && !user.isAdmin) {
    return router.createUrlTree(['/']);
  }

  if (requireManager && !user.isManager) {
    return router.createUrlTree(['/']);
  }

  if (requireEmployee && !user.isEmployee) {
    return router.createUrlTree(['/']);
  }

  return true;
};
export interface MyAuthRouteData {
  requireAuth?: boolean;
  requireAdmin?: boolean;
  requireManager?: boolean;
  requireEmployee?: boolean;
}

export function myAuthData(data: MyAuthRouteData): { auth: MyAuthRouteData } {
  return { auth: data };
}
