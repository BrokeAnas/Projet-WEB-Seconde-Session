import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateRendezVousDto, RendezVous, StatutRdv } from './models/rendezvous.model';

export interface RendezVousFiltres {
  medecinId?: string;
  date?: string;
}

@Injectable({ providedIn: 'root' })
export class RendezVousService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/rendezvous`;

  private rendezVousSignal = signal<RendezVous[]>([]);
  readonly rendezVous = this.rendezVousSignal.asReadonly();
  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);

  loadRendezVous(filtres: RendezVousFiltres = {}): void {
    this.isLoading.set(true);
    this.error.set(null);

    let params = new HttpParams();
    if (filtres.medecinId) params = params.set('medecinId', filtres.medecinId);
    if (filtres.date) params = params.set('date', filtres.date);

    this.http.get<RendezVous[]>(this.apiUrl, { params }).subscribe({
      next: rdvs => {
        this.rendezVousSignal.set(rdvs);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Impossible de charger les rendez-vous.');
        this.isLoading.set(false);
      }
    });
  }

  create(dto: CreateRendezVousDto): Observable<RendezVous> {
    return this.http.post<RendezVous>(this.apiUrl, dto).pipe(
      tap(rdv => this.rendezVousSignal.update(list => [...list, rdv]))
    );
  }

  updateStatut(id: number, statut: StatutRdv): Observable<RendezVous> {
    return this.http.patch<RendezVous>(`${this.apiUrl}/${id}/statut`, { statut }).pipe(
      tap(updated => this.rendezVousSignal.update(list =>
        list.map(r => r.id_rdv === id ? updated : r)))
    );
  }
}
