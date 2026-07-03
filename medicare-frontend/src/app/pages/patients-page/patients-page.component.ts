import { Component, DestroyRef, OnInit, inject } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import { debounceTime, distinctUntilChanged, startWith } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBar } from '@angular/material/snack-bar';
import { PatientService } from '../../services/api/patient.service';
import { Patient } from '../../services/api/models/patient.model';

@Component({
  selector: 'app-patients-page',
  imports: [
    ReactiveFormsModule, RouterLink, DatePipe,
    MatCardModule, MatButtonModule, MatIconModule, MatFormFieldModule,
    MatInputModule, MatProgressSpinnerModule, MatTooltipModule
  ],
  templateUrl: './patients-page.component.html',
  styleUrl: './patients-page.component.scss'
})
export class PatientsPageComponent implements OnInit {
  readonly patientService = inject(PatientService);
  private readonly router = inject(Router);
  private readonly destroyRef = inject(DestroyRef);
  private readonly snackBar = inject(MatSnackBar);

  readonly searchControl = new FormControl('');

  ngOnInit(): void {
    this.searchControl.valueChanges.pipe(
      startWith(''),
      debounceTime(300),
      distinctUntilChanged(),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(term => this.patientService.loadPatients(term ?? ''));
  }

  editer(id: string): void {
    this.router.navigate(['/patients', id, 'edit']);
  }

  supprimer(patient: Patient): void {
    if (!confirm(`Supprimer définitivement le patient ${patient.prenom} ${patient.nom} ?`)) return;
    this.patientService.delete(patient.id_nat).subscribe({
      next: () => this.snackBar.open('Patient supprimé.', 'OK', { duration: 3000 }),
      error: (err) => this.snackBar.open(err.error?.error ?? 'Suppression impossible.', 'OK', { duration: 4000 })
    });
  }
}
