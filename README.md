# MediCare Manager — Gestion centralisée de cabinets médicaux

Application fullstack de gestion d'un réseau de cabinets médicaux multi-succursales (Belgique),
avec trois profils (Administrateur, Médecin, Secrétaire), authentification JWT et journal d'audit
financier automatique (triggers SQL).

- **Backend** : ASP.NET Core (.NET 8) — Clean Architecture 3 couches — Dapper — MySQL 8
- **Frontend** : Angular 17+ (standalone, nouvelle syntaxe `@if` / `@for` / `@switch`, Signals)
- **Auth** : JWT + mots de passe hachés BCrypt

---

## Prérequis

- .NET 8 SDK
- Node.js 20+
- Angular CLI 17+
- MySQL 8.0+

---

## Installation

### 1. Base de données

```bash
mysql -u root -p < database/medconnect_schema.sql
mysql -u root -p medconnect < database/medconnect_data.sql
```

Le schéma crée les **12 tables**, les **index d'optimisation** et les **2 triggers d'audit**
(`Paiement_Update_Log`, `Paiement_Delete_Log`). Le jeu de données contient des comptes prêts à
l'emploi pour les 3 rôles, des numéros de Registre National belges valides, 15 rendez-vous et
10 paiements (dont 2 déjà modifiés → entrées présentes dans `Paiement_Historique`).

### 2. Backend

```bash
cd MediCareManager.API
# Ouvrir appsettings.json et configurer si besoin :
#  - ConnectionStrings.DefaultConnection  (user / password MySQL)
#  - Jwt.Key (clé secrète, minimum 32 caractères)
dotnet restore
dotnet run
```

- API : http://localhost:5000
- Swagger UI : http://localhost:5000/swagger

### 3. Frontend

```bash
cd medicare-frontend
npm install
ng serve
```

- Application : http://localhost:4200

---

## Comptes de test

| Rôle        | Email                   | Mot de passe |
|-------------|-------------------------|--------------|
| Admin       | admin@medicare.be       | Admin1234!   |
| Médecin     | dr.martin@medicare.be   | Medecin1234! |
| Secrétaire  | secretaire@medicare.be  | Secret1234!  |

Autres médecins : `dr.dubois@medicare.be`, `dr.leroy@medicare.be`, `dr.lambert@medicare.be` (même mot de passe).

---

## Architecture

### Clean Architecture — 3 projets séparés

```
MediCareManager.sln
├── MediCareManager.Core/            ← Domaine (AUCUNE dépendance externe)
│   ├── Entities/                    ← 13 entités (1 par table)
│   ├── Interfaces/Repositories/     ← Contrats des repositories
│   ├── Interfaces/Services/         ← Contrats des services
│   ├── Services/                    ← Logique métier (Auth, Patient, RDV, …)
│   ├── DTOs/                        ← DTOs (DataAnnotations, base .NET)
│   ├── Exceptions/                  ← Exceptions métier
│   ├── Common/                      ← Validateur NISS, convertisseurs JSON
│   └── Settings/                    ← JwtSettings
├── MediCareManager.Infrastructure/  ← Accès données (référence Core uniquement)
│   ├── Repositories/                ← Implémentations Dapper (requêtes paramétrées)
│   ├── Security/                    ← BCryptPasswordHasher
│   └── Configuration/               ← Connexion MySQL + handlers Dapper
└── MediCareManager.API/             ← Présentation (référence Core + Infrastructure)
    ├── Controllers/                 ← Contrôleurs REST (injectent les services)
    ├── Middleware/                  ← Traduction exceptions → codes HTTP
    └── Program.cs                   ← DI, JWT, CORS, JSON snake_case, Swagger
```

**Dépendances** : `API → Core, Infrastructure` · `Infrastructure → Core` · `Core → rien d'externe`.

### Frontend Angular

- Composants **standalone**, nouvelle syntaxe **`@if` / `@for` / `@switch`** exclusivement.
- Gestion d'état **uniquement via Services Angular + `signal()`** (aucun NgRx / Redux).
- **Guards** (`authGuard`, `roleGuard`) pour la protection des routes par rôle.
- **Intercepteur HTTP** qui injecte automatiquement `Authorization: Bearer {token}`.
- Le **JWT est conservé en mémoire** (signal), jamais dans `localStorage`.

---

## Dépannage

- **Port 5000 occupé (macOS)** : le « Récepteur AirPlay » de macOS écoute sur le port 5000.
  Désactivez-le (Réglages Système → Général → AirDrop et Handoff → Récepteur AirPlay), ou lancez
  l'API sur un autre port : `dotnet run --urls http://localhost:5050` (et adaptez
  `environment.apiUrl` côté Angular).
- **`npm install`** nécessite un accès réseau pour télécharger Angular Material
  (`@angular/material`, `@angular/cdk`).
- **CORS** : le backend autorise `http://localhost:4200`. Si vous changez le port du frontend,
  ajustez la politique CORS dans `Program.cs`.
