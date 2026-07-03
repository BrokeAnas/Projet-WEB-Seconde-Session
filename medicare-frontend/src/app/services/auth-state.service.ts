import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, map, tap } from 'rxjs';
import { AuthService } from './api/auth.service';
import { LoginDto, UserRole } from './api/models/auth.model';

/**
 * État d'authentification de l'application (Signals).
 * Délègue l'appel HTTP à AuthService (services/api) et conserve la session en mémoire.
 */
@Injectable({ providedIn: 'root' })
export class AuthStateService {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  // Stockage en mémoire vive uniquement (protection XSS) — aucun stockage persistant.
  private tokenSignal = signal<string | null>(null);
  private roleSignal = signal<UserRole | null>(null);
  private nomSignal = signal<string | null>(null);
  private userIdSignal = signal<string | null>(null);

  readonly token = this.tokenSignal.asReadonly();
  readonly role = this.roleSignal.asReadonly();
  readonly nom = this.nomSignal.asReadonly();
  readonly userId = this.userIdSignal.asReadonly();
  // Le signal isAuthenticated est calculé en fonction de la présence d'un token.
  readonly isAuthenticated = computed(() => this.tokenSignal() !== null);

  login(dto: LoginDto): Observable<void> {
    return this.authService.login(dto).pipe(
      tap(response => {
        this.tokenSignal.set(response.token);
        this.roleSignal.set(response.role as UserRole);
        this.nomSignal.set(`${response.prenom} ${response.nom}`);
        this.userIdSignal.set(this.extractSub(response.token));
      }),
      map(() => void 0)
    );
  }

  logout(): void {
    this.tokenSignal.set(null);
    this.roleSignal.set(null);
    this.nomSignal.set(null);
    this.userIdSignal.set(null);
    this.router.navigate(['/login']);
  }

  /** Extrait le claim « sub » (identifiant utilisateur) du JWT, sans librairie externe. */
  private extractSub(token: string): string | null {
    try {
      const payload = token.split('.')[1];
      const json = atob(payload.replace(/-/g, '+').replace(/_/g, '/'));
      const claims = JSON.parse(json);
      return claims.sub ?? null;
    } catch {
      return null;
    }
  }

  hasRole(...roles: UserRole[]): boolean {
    const current = this.roleSignal();
    return current !== null && roles.includes(current);
  }
}
