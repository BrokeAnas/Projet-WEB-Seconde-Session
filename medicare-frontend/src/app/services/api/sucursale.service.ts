import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Sucursale } from './models/sucursale.model';

@Injectable({ providedIn: 'root' })
export class SucursaleService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/sucursales`;

  private sucursalesSignal = signal<Sucursale[]>([]);
  readonly sucursales = this.sucursalesSignal.asReadonly();
  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);

  loadSucursales(): void {
    this.isLoading.set(true);
    this.error.set(null);
    this.http.get<Sucursale[]>(this.apiUrl).subscribe({
      next: sucursales => {
        this.sucursalesSignal.set(sucursales);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Impossible de charger les succursales.');
        this.isLoading.set(false);
      }
    });
  }
}
