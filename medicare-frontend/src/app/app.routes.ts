import { Routes } from '@angular/router';
import { authGuard } from './services/auth.guard';

import { LoginPageComponent } from './pages/login-page/login-page.component';
import { PatientsPageComponent } from './pages/patients-page/patients-page.component';
import { PatientFormPageComponent } from './pages/patient-form-page/patient-form-page.component';
import { AgendaPageComponent } from './pages/agenda-page/agenda-page.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginPageComponent },

  // Patients
  { path: 'patients', component: PatientsPageComponent, canActivate: [authGuard] },
  { path: 'patients/new', component: PatientFormPageComponent, canActivate: [authGuard] },
  { path: 'patients/:id/edit', component: PatientFormPageComponent, canActivate: [authGuard] },

  // Agenda
  { path: 'agenda', component: AgendaPageComponent, canActivate: [authGuard] },

  { path: '**', redirectTo: 'patients' }
];
