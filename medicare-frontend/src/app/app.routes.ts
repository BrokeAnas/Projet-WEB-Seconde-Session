import { Routes } from '@angular/router';
import { authGuard } from './services/auth.guard';
import { roleGuard } from './services/role.guard';

import { LoginPageComponent } from './pages/login-page/login-page.component';
import { DashboardPageComponent } from './pages/dashboard-page/dashboard-page.component';
import { PatientsPageComponent } from './pages/patients-page/patients-page.component';
import { PatientDetailPageComponent } from './pages/patient-detail-page/patient-detail-page.component';
import { PatientFormPageComponent } from './pages/patient-form-page/patient-form-page.component';
import { AgendaPageComponent } from './pages/agenda-page/agenda-page.component';
import { PaiementsPageComponent } from './pages/paiements-page/paiements-page.component';
import { AuditPageComponent } from './pages/audit-page/audit-page.component';
import { MedecinsPageComponent } from './pages/medecins-page/medecins-page.component';
import { SecretairesPageComponent } from './pages/secretaires-page/secretaires-page.component';
import { AdministrationPageComponent } from './pages/administration-page/administration-page.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginPageComponent },
  { path: 'dashboard', component: DashboardPageComponent, canActivate: [authGuard] },

  // Patients
  { path: 'patients', component: PatientsPageComponent, canActivate: [authGuard] },
  { path: 'patients/new', component: PatientFormPageComponent, canActivate: [(route, state) => roleGuard('secretaire', 'admin')(route, state)] },
  { path: 'patients/:id', component: PatientDetailPageComponent, canActivate: [authGuard] },
  { path: 'patients/:id/edit', component: PatientFormPageComponent, canActivate: [(route, state) => roleGuard('secretaire', 'admin')(route, state)] },

  // Agenda
  { path: 'agenda', component: AgendaPageComponent, canActivate: [authGuard] },

  // Paiements
  { path: 'paiements', component: PaiementsPageComponent, canActivate: [(route, state) => roleGuard('secretaire', 'admin')(route, state)] },
  { path: 'paiements/audit', component: AuditPageComponent, canActivate: [(route, state) => roleGuard('admin')(route, state)] },

  // Personnel
  { path: 'personnel/medecins', component: MedecinsPageComponent, canActivate: [authGuard] },
  { path: 'personnel/secretaires', component: SecretairesPageComponent, canActivate: [(route, state) => roleGuard('admin')(route, state)] },

  // Administration
  { path: 'administration', component: AdministrationPageComponent, canActivate: [(route, state) => roleGuard('admin')(route, state)] },

  { path: '**', redirectTo: 'dashboard' }
];
