export interface LoginDto {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  nom: string;
  prenom: string;
}
