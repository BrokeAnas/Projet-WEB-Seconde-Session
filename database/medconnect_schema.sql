-- =============================================================
--  MediCare Manager (version simplifiée) - Schéma MySQL 8
--  Gestion de patients et de rendez-vous d'un réseau de cabinets
-- =============================================================

SET NAMES utf8mb4;

-- On repart d'une base propre à chaque exécution du script.
DROP DATABASE IF EXISTS medconnect;
CREATE DATABASE medconnect CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE medconnect;

CREATE TABLE SpecialisationMedecin (
  id_specialisation INT PRIMARY KEY AUTO_INCREMENT,
  libelle VARCHAR(100) NOT NULL UNIQUE
) ENGINE=InnoDB;

CREATE TABLE Sucursale (
  id_sucursale INT PRIMARY KEY AUTO_INCREMENT,
  nom VARCHAR(150) NOT NULL,
  adresse VARCHAR(255) NOT NULL,
  telephone VARCHAR(20),
  email VARCHAR(150)
) ENGINE=InnoDB;

CREATE TABLE Medecin (
  id_nat BIGINT(11) PRIMARY KEY,
  nom VARCHAR(100) NOT NULL,
  prenom VARCHAR(100) NOT NULL,
  id_specialisation INT NOT NULL,
  id_sucursale INT,
  FOREIGN KEY (id_specialisation) REFERENCES SpecialisationMedecin(id_specialisation),
  FOREIGN KEY (id_sucursale) REFERENCES Sucursale(id_sucursale) ON DELETE SET NULL
) ENGINE=InnoDB;

CREATE TABLE Administrateur (
  id_admin INT PRIMARY KEY AUTO_INCREMENT,
  nom VARCHAR(100) NOT NULL,
  prenom VARCHAR(100) NOT NULL,
  email VARCHAR(150) NOT NULL UNIQUE,
  mot_de_passe VARCHAR(255) NOT NULL
) ENGINE=InnoDB;

CREATE TABLE Patient (
  id_nat BIGINT(11) PRIMARY KEY,
  nom VARCHAR(100) NOT NULL,
  prenom VARCHAR(100) NOT NULL,
  date_naissance DATE NOT NULL,
  adresse VARCHAR(255),
  telephone VARCHAR(20),
  email VARCHAR(150)
) ENGINE=InnoDB;

CREATE TABLE RendezVous (
  id_rdv INT PRIMARY KEY AUTO_INCREMENT,
  id_nat_patient BIGINT(11) NOT NULL,
  id_nat_medecin BIGINT(11) NOT NULL,
  id_sucursale INT NOT NULL,
  date_rdv DATE NOT NULL,
  heure_debut TIME NOT NULL,
  heure_fin TIME NOT NULL,
  motif VARCHAR(255),
  statut ENUM('Planifié','En cours','Terminé','Annulé') NOT NULL DEFAULT 'Planifié',
  FOREIGN KEY (id_nat_patient) REFERENCES Patient(id_nat),
  FOREIGN KEY (id_nat_medecin) REFERENCES Medecin(id_nat),
  FOREIGN KEY (id_sucursale) REFERENCES Sucursale(id_sucursale)
) ENGINE=InnoDB;

-- INDEX D'OPTIMISATION
CREATE INDEX idx_patient_nom_prenom ON Patient(nom, prenom);
CREATE INDEX idx_rdv_medecin_date ON RendezVous(id_nat_medecin, date_rdv);
