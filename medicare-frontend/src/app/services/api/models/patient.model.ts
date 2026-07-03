export interface Patient {
  id_nat: string;
  nom: string;
  prenom: string;
  date_naissance: string;
  adresse?: string;
  telephone?: string;
  email?: string;
}

export interface CreatePatientDto {
  id_nat: string;
  nom: string;
  prenom: string;
  date_naissance: string;
  adresse?: string;
  telephone?: string;
  email?: string;
}

export interface UpdatePatientDto {
  nom?: string;
  prenom?: string;
  date_naissance?: string;
  adresse?: string;
  telephone?: string;
  email?: string;
}
