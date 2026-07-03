import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthStateService } from './auth-state.service';
import { UserRole } from './api/models/auth.model';

// Le guard de rôle vérifie si l'utilisateur est authentifié et possède le rôle requis avant de permettre l'accès à certaines routes.
export const roleGuard = (...requiredRoles: UserRole[]): CanActivateFn => () => {
  const auth = inject(AuthStateService);
  const router = inject(Router);
  if (!auth.isAuthenticated()) {
    router.navigate(['/login']);
    return false;
  }
  if (auth.hasRole(...requiredRoles)) return true;
  router.navigate(['/dashboard']);
  return false;
};
