import { Component, OnInit, inject, signal } from '@angular/core';
import { DatePipe, DecimalPipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { PaiementService } from '../../services/api/paiement.service';
import { AuditLog } from '../../services/api/models/paiement.model';

@Component({
  selector: 'app-audit-page',
  imports: [DatePipe, DecimalPipe, MatCardModule, MatProgressSpinnerModule],
  templateUrl: './audit-page.component.html',
  styleUrl: './audit-page.component.scss'
})
export class AuditPageComponent implements OnInit {
  private readonly paiementService = inject(PaiementService);

  readonly logs = signal<AuditLog[]>([]);
  readonly loading = signal(true);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    this.paiementService.getAuditLog().subscribe({
      next: (data) => { this.logs.set(data); this.loading.set(false); },
      error: (err) => { this.error.set(err.error?.error ?? 'Erreur de chargement'); this.loading.set(false); }
    });
  }
}
