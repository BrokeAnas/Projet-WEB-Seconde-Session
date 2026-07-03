import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Medecin } from './models/medecin.model';

@Injectable({ providedIn: 'root' })
export class MedecinService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/medecins`;

  private medecinsSignal = signal<Medecin[]>([]);
  readonly medecins = this.medecinsSignal.asReadonly();
  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);

  loadMedecins(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.http.get<Medecin[]>(this.apiUrl).subscribe({
      next: medecins => {
        this.medecinsSignal.set(medecins);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Impossible de charger les médecins.');
        this.isLoading.set(false);
      }
    });
  }
}
