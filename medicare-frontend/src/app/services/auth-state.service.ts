import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, map, tap } from 'rxjs';
import { AuthService } from './api/auth.service';
import { LoginDto } from './api/models/auth.model';

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
  private nomSignal = signal<string | null>(null);

  readonly token = this.tokenSignal.asReadonly();
  readonly nom = this.nomSignal.asReadonly();
  // Le signal isAuthenticated est calculé en fonction de la présence d'un token.
  readonly isAuthenticated = computed(() => this.tokenSignal() !== null);

  login(dto: LoginDto): Observable<void> {
    return this.authService.login(dto).pipe(
      tap(response => {
        this.tokenSignal.set(response.token);
        this.nomSignal.set(`${response.prenom} ${response.nom}`);
      }),
      map(() => void 0)
    );
  }

  logout(): void {
    this.tokenSignal.set(null);
    this.nomSignal.set(null);
    this.router.navigate(['/login']);
  }
}
