import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthStateService } from './auth-state.service';


// Le guard d'authentification vérifie si l'utilisateur est authentifié avant de permettre l'accès à certaines routes.
export const authGuard: CanActivateFn = () => {
  const auth = inject(AuthStateService);
  if (auth.isAuthenticated()) return true;
  inject(Router).navigate(['/login']);
  return false;
};
