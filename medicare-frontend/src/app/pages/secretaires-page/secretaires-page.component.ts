import { Component, OnInit, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SecretaireService } from '../../services/api/secretaire.service';
import { Secretaire } from '../../services/api/models/secretaire.model';
import { SecretaireFormPageComponent } from '../secretaire-form-page/secretaire-form-page.component';

@Component({
  selector: 'app-secretaires-page',
  imports: [
    MatCardModule, MatButtonModule, MatIconModule, MatProgressSpinnerModule,
    MatTooltipModule, MatDialogModule
  ],
  templateUrl: './secretaires-page.component.html',
  styleUrl: './secretaires-page.component.scss'
})
export class SecretairesPageComponent implements OnInit {
  readonly secretaireService = inject(SecretaireService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  ngOnInit(): void {
    this.secretaireService.loadSecretaires();
  }

  nouveau(): void {
    const ref = this.dialog.open(SecretaireFormPageComponent, { width: '560px', maxWidth: '95vw' });
    ref.afterClosed().subscribe(ok => { if (ok) this.secretaireService.loadSecretaires(); });
  }

  modifier(s: Secretaire): void {
    const ref = this.dialog.open(SecretaireFormPageComponent, { width: '560px', maxWidth: '95vw', data: s });
    ref.afterClosed().subscribe(ok => { if (ok) this.secretaireService.loadSecretaires(); });
  }

  supprimer(s: Secretaire): void {
    if (!confirm(`Supprimer la secrétaire ${s.prenom} ${s.nom} ?`)) return;
    this.secretaireService.delete(s.id_nat).subscribe({
      next: () => this.snackBar.open('Secrétaire supprimée.', 'OK', { duration: 3000 }),
      error: (err) => this.snackBar.open(err.error?.error ?? 'Suppression impossible.', 'OK', { duration: 4000 })
    });
  }
}
