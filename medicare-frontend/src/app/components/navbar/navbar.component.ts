import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { AuthStateService } from '../../services/auth-state.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive, MatToolbarModule, MatButtonModule, MatIconModule, MatMenuModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {
  readonly authStateService = inject(AuthStateService);

  readonly roleLabel = computed(() => {
    switch (this.authStateService.role()) {
      case 'admin': return 'Administrateur';
      case 'medecin': return 'Médecin';
      case 'secretaire': return 'Secrétaire';
      default: return '';
    }
  });

  logout(): void {
    this.authStateService.logout();
  }
}
