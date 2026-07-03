import { Component, OnInit, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MedecinService } from '../../services/api/medecin.service';
import { AuthStateService } from '../../services/auth-state.service';
import { Medecin } from '../../services/api/models/medecin.model';
import { MedecinFormPageComponent } from '../medecin-form-page/medecin-form-page.component';

@Component({
  selector: 'app-medecins-page',
  imports: [
    MatCardModule, MatButtonModule, MatIconModule, MatProgressSpinnerModule,
    MatTooltipModule, MatDialogModule
  ],
  templateUrl: './medecins-page.component.html',
  styleUrl: './medecins-page.component.scss'
})
export class MedecinsPageComponent implements OnInit {
  readonly medecinService = inject(MedecinService);
  readonly authStateService = inject(AuthStateService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  ngOnInit(): void {
    this.medecinService.loadMedecins();
  }

  nouveau(): void {
    const ref = this.dialog.open(MedecinFormPageComponent, { width: '560px', maxWidth: '95vw' });
    ref.afterClosed().subscribe(ok => { if (ok) this.medecinService.loadMedecins(); });
  }

  modifier(m: Medecin): void {
    const ref = this.dialog.open(MedecinFormPageComponent, { width: '560px', maxWidth: '95vw', data: m });
    ref.afterClosed().subscribe(ok => { if (ok) this.medecinService.loadMedecins(); });
  }

  supprimer(m: Medecin): void {
    if (!confirm(`Supprimer le médecin Dr ${m.prenom} ${m.nom} ?`)) return;
    this.medecinService.delete(m.id_nat).subscribe({
      next: () => this.snackBar.open('Médecin supprimé.', 'OK', { duration: 3000 }),
      error: (err) => {
        const message = err.status === 409
          ? (err.error?.error ?? 'Ce médecin a des rendez-vous futurs planifiés.')
          : (err.error?.error ?? 'Suppression impossible.');
        this.snackBar.open(message, 'OK', { duration: 5000 });
      }
    });
  }
}
