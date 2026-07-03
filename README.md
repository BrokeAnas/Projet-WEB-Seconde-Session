# MediCare Manager — Gestion de patients et de rendez-vous

Application fullstack de gestion d'un réseau de cabinets médicaux : dossier patients
(CRUD + recherche) et agenda de rendez-vous (filtres, création avec détection de conflit
d'horaire, suivi de statut), protégée par une authentification JWT.

- **Backend** : ASP.NET Core (.NET 8) — Clean Architecture 3 couches — Dapper — MySQL
- **Frontend** : Angular (standalone, syntaxe `@if` / `@for` / `@switch`, Signals)
- **Auth** : JWT + mot de passe haché BCrypt

---

## Prérequis

| Outil | Version utilisée |
|---|---|
| .NET SDK | 8.0 (ou plus récent) |
| Node.js | 20 ou plus récent (développé avec la v24) |
| Angular CLI | 21 (`npm install -g @angular/cli`) |
| MySQL | 8.0 ou plus récent |

---

## Installation

### 1. Base de données

Depuis la racine du projet, exécuter les deux scripts SQL dans cet ordre :

```bash
mysql -u root -p < database/medconnect_schema.sql
mysql -u root -p < database/medconnect_data.sql
```

Le premier script crée la base `medconnect` et ses **6 tables**
(Patient, Medecin, SpecialisationMedecin, Sucursale, RendezVous, Administrateur)
avec leurs index. Le second insère les données de démonstration
(1 compte administrateur, 4 médecins, 10 patients, 15 rendez-vous).

### 2. Chaîne de connexion

La chaîne de connexion se trouve dans `MediCareManager.API/appsettings.json` :

```json
"DefaultConnection": "Server=localhost;Database=medconnect;User=root;Password=root;CharSet=utf8mb4;"
```

Adapter `User` / `Password` à votre installation MySQL si nécessaire.
Aucune variable d'environnement n'est requise.

---

## Lancement du Backend

```bash
cd MediCareManager.API
dotnet restore
dotnet run
```

- API : http://localhost:5000
- Swagger UI : http://localhost:5000/swagger

## Lancement du Frontend

Dans un second terminal :

```bash
cd medicare-frontend
npm install
ng serve
```

- Application : http://localhost:4200

---

## Compte de test

| Rôle | Email | Mot de passe |
|---|---|---|
| Administrateur | admin@medicare.be | Admin1234! |

---

## Architecture

### Clean Architecture — 3 projets séparés (UseCases / Gateways / Repositories)

```
MediCareManager.sln
├── MediCareManager.Core/            ← Domaine (ne connaît ni MySQL ni ASP.NET)
│   ├── Models/                      ← Modèles du domaine + DTOs des use cases
│   ├── IGateways/                   ← Contrats des gateways (portes vers l'Infrastructure)
│   ├── UseCases/                    ← Logique métier (Auth, Patient, RendezVous, …)
│   │   └── Abstractions/            ← Contrats des use cases (injectés dans l'API)
│   ├── Exceptions/                  ← Exceptions métier
│   ├── Common/                      ← Validateur NISS (modulo 97), convertisseur JSON
│   ├── Security/                    ← IPasswordHasher (abstraction)
│   ├── Settings/                    ← JwtSettings
│   └── ServiceCollectionExtension   ← AddCoreServices() : DI des use cases
├── MediCareManager.Infrastructure/  ← Accès données (référence Core uniquement)
│   ├── Gateways/                    ← Implémentations des IGateways (reçoivent les repositories)
│   ├── Repositories/                ← Implémentations Dapper (requêtes paramétrées)
│   │   └── Abstractions/            ← Contrats des repositories (internes à l'Infrastructure)
│   ├── Security/                    ← BCryptPasswordHasher
│   ├── Configuration/               ← Connexion MySQL + handlers Dapper (DateOnly/TimeOnly)
│   └── ServiceCollectionExtension   ← AddInfrastructureServices() : DI gateways + repositories
└── MediCareManager.API/             ← Présentation (référence Core + Infrastructure)
    ├── EndPoints/                   ← Minimal API (MapGroup + RequireAuthorization)
    ├── Models/                      ← DTOs propres au HTTP (LoginDto, AuthResponseDto, …)
    ├── Middleware/                  ← Traduction exceptions métier → codes HTTP
    └── Program.cs                   ← JWT, CORS, JSON snake_case, Swagger, Map des endpoints
```

**Dépendances** : `API → Core, Infrastructure` · `Infrastructure → Core` · `Core → rien d'externe`.
**Flux d'une requête** : EndPoint → UseCase (Core) → IGateway → Gateway (Infrastructure) → Repository → Dapper → MySQL.

### Frontend Angular

```
src/app/
├── components/navbar/               ← Barre de navigation
├── pages/                           ← 4 pages standalone
│   ├── login-page/                  ← Connexion (Reactive Forms)
│   ├── patients-page/               ← Liste + recherche (debounceTime / distinctUntilChanged)
│   ├── patient-form-page/           ← Création / édition (validateur NISS modulo 97)
│   └── agenda-page/                 ← Rendez-vous filtrés + dialog de création + statut
└── services/
    ├── auth-state.service.ts        ← État de session (signals token/nom, isAuthenticated)
    ├── auth.guard.ts                ← Protection des routes
    ├── auth.interceptor.ts          ← Ajout automatique de « Authorization: Bearer {token} »
    └── api/                         ← Services HTTP (1 par ressource) + models/
```

- Composants **standalone**, syntaxe **`@if` / `@for` / `@switch`** exclusivement.
- Gestion d'état **uniquement via Services Angular + `signal()`** (aucun NgRx / Redux).
- Le **JWT est conservé en mémoire** (signal), jamais dans `localStorage`.

### Règles métier

- Numéro de Registre National belge validé par **contrôle modulo 97** (backend + frontend).
- Unicité du numéro national d'un patient (HTTP 409 en cas de doublon).
- **Détection de conflit d'agenda** : un médecin ne peut pas avoir deux rendez-vous
  qui se chevauchent le même jour (HTTP 409).
- L'heure de fin d'un rendez-vous doit être postérieure à l'heure de début (HTTP 400).
- Un patient lié à des rendez-vous ne peut pas être supprimé (HTTP 400).

---

## Dépannage

- **Port 5000 occupé (macOS)** : le « Récepteur AirPlay » de macOS écoute sur le port 5000.
  Désactivez-le (Réglages Système → Général → AirDrop et Handoff → Récepteur AirPlay), ou lancez
  l'API sur un autre port : `dotnet run --urls http://localhost:5050` (et adaptez
  `environment.apiUrl` côté Angular).
- **CORS** : le backend autorise `http://localhost:4200`. Si vous changez le port du frontend,
  ajustez la politique CORS dans `Program.cs`.
