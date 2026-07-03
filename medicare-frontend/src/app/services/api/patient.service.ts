import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreatePatientDto, Patient, UpdatePatientDto } from './models/patient.model';

@Injectable({ providedIn: 'root' })
export class PatientService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/patients`;

  private patientsSignal = signal<Patient[]>([]);
  readonly patients = this.patientsSignal.asReadonly();
  readonly isLoading = signal(false);
  readonly error = signal<string | null>(null);

  loadPatients(search?: string, page = 1, pageSize = 20): void {
    this.isLoading.set(true);
    this.error.set(null);

    let params = new HttpParams().set('page', page).set('pageSize', pageSize);
    if (search) params = params.set('search', search);

    this.http.get<Patient[]>(this.apiUrl, { params }).subscribe({
      next: patients => {
        this.patientsSignal.set(patients);
        this.isLoading.set(false);
      },
      error: () => {
        this.error.set('Impossible de charger les patients.');
        this.isLoading.set(false);
      }
    });
  }

  getById(idNat: string): Observable<Patient> {
    return this.http.get<Patient>(`${this.apiUrl}/${idNat}`);
  }

  create(dto: CreatePatientDto): Observable<Patient> {
    return this.http.post<Patient>(this.apiUrl, dto).pipe(
      tap(patient => this.patientsSignal.update(list => [...list, patient]))
    );
  }

  update(idNat: string, dto: UpdatePatientDto): Observable<Patient> {
    return this.http.put<Patient>(`${this.apiUrl}/${idNat}`, dto).pipe(
      tap(updated => this.patientsSignal.update(list =>
        list.map(p => p.id_nat === idNat ? updated : p)))
    );
  }

  delete(idNat: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${idNat}`).pipe(
      tap(() => this.patientsSignal.update(list => list.filter(p => p.id_nat !== idNat)))
    );
  }
}
