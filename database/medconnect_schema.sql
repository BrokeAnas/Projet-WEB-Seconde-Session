-- =============================================================
--  MediCare Manager - Schéma de la base de données MySQL 8
--  Réseau de cabinets médicaux multi-succursales (Belgique)
-- =============================================================

CREATE DATABASE IF NOT EXISTS medconnect CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
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
  email VARCHAR(150) NOT NULL UNIQUE,
  mot_de_passe VARCHAR(255) NOT NULL,
  id_specialisation INT NOT NULL,
  id_sucursale INT,
  FOREIGN KEY (id_specialisation) REFERENCES SpecialisationMedecin(id_specialisation),
  FOREIGN KEY (id_sucursale) REFERENCES Sucursale(id_sucursale) ON DELETE SET NULL
) ENGINE=InnoDB;

CREATE TABLE Secretaire (
  id_nat BIGINT(11) PRIMARY KEY,
  nom VARCHAR(100) NOT NULL,
  prenom VARCHAR(100) NOT NULL,
  email VARCHAR(150) NOT NULL UNIQUE,
  mot_de_passe VARCHAR(255) NOT NULL,
  id_sucursale INT NOT NULL,
  FOREIGN KEY (id_sucursale) REFERENCES Sucursale(id_sucursale)
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
  email VARCHAR(150) UNIQUE
) ENGINE=InnoDB;

CREATE TABLE Assurance (
  id_assurance INT PRIMARY KEY AUTO_INCREMENT,
  nom VARCHAR(150) NOT NULL,
  type VARCHAR(50)
) ENGINE=InnoDB;

CREATE TABLE TypeMaladie (
  id_maladie INT PRIMARY KEY AUTO_INCREMENT,
  libelle VARCHAR(150) NOT NULL UNIQUE,
  code_CIM VARCHAR(10)
) ENGINE=InnoDB;

CREATE TABLE PatientAssurance (
  id_nat_patient BIGINT(11) NOT NULL,
  id_assurance INT NOT NULL,
  numero_affiliation VARCHAR(50),
  date_debut DATE,
  date_fin DATE,
  PRIMARY KEY (id_nat_patient, id_assurance),
  FOREIGN KEY (id_nat_patient) REFERENCES Patient(id_nat) ON DELETE CASCADE,
  FOREIGN KEY (id_assurance) REFERENCES Assurance(id_assurance) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE PatientMaladie (
  id_nat_patient BIGINT(11) NOT NULL,
  id_maladie INT NOT NULL,
  id_nat_medecin BIGINT(11),
  date_diagnostic DATE NOT NULL,
  observations TEXT,
  PRIMARY KEY (id_nat_patient, id_maladie),
  FOREIGN KEY (id_nat_patient) REFERENCES Patient(id_nat) ON DELETE CASCADE,
  FOREIGN KEY (id_maladie) REFERENCES TypeMaladie(id_maladie),
  FOREIGN KEY (id_nat_medecin) REFERENCES Medecin(id_nat) ON DELETE SET NULL
) ENGINE=InnoDB;

CREATE TABLE RendezVous (
  id_rdv INT PRIMARY KEY AUTO_INCREMENT,
  id_nat_patient BIGINT(11) NOT NULL,
  id_nat_medecin BIGINT(11) NOT NULL,
  id_nat_secretaire BIGINT(11),
  id_sucursale INT NOT NULL,
  date_rdv DATE NOT NULL,
  heure_debut TIME NOT NULL,
  heure_fin TIME NOT NULL,
  motif VARCHAR(255),
  statut ENUM('Planifié','En cours','Terminé','Annulé') NOT NULL DEFAULT 'Planifié',
  FOREIGN KEY (id_nat_patient) REFERENCES Patient(id_nat),
  FOREIGN KEY (id_nat_medecin) REFERENCES Medecin(id_nat),
  FOREIGN KEY (id_nat_secretaire) REFERENCES Secretaire(id_nat) ON DELETE SET NULL,
  FOREIGN KEY (id_sucursale) REFERENCES Sucursale(id_sucursale)
) ENGINE=InnoDB;

CREATE TABLE Paiement (
  id_paiement INT PRIMARY KEY AUTO_INCREMENT,
  id_nat_patient BIGINT(11) NOT NULL,
  id_rdv INT,
  montant DECIMAL(10,2) NOT NULL,
  date_paiement DATE NOT NULL,
  mode_paiement VARCHAR(50),
  FOREIGN KEY (id_nat_patient) REFERENCES Patient(id_nat),
  FOREIGN KEY (id_rdv) REFERENCES RendezVous(id_rdv) ON DELETE SET NULL
) ENGINE=InnoDB;

CREATE TABLE Paiement_Historique (
  id_historique INT PRIMARY KEY AUTO_INCREMENT,
  id_paiement INT NOT NULL,
  id_nat_patient BIGINT(11) NOT NULL,
  id_rdv INT,
  montant DECIMAL(10,2) NOT NULL,
  date_paiement DATE NOT NULL,
  operation ENUM('UPDATE','DELETE') NOT NULL,
  date_operation TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB;

-- INDEX D'OPTIMISATION
CREATE INDEX idx_patient_nom_prenom ON Patient(nom, prenom);
CREATE INDEX idx_rdv_medecin_date ON RendezVous(id_nat_medecin, date_rdv);
CREATE INDEX idx_rdv_patient ON RendezVous(id_nat_patient);
CREATE INDEX idx_paiement_patient ON Paiement(id_nat_patient);
CREATE INDEX idx_historique_paiement ON Paiement_Historique(id_paiement);

-- TRIGGERS D'AUDIT OBLIGATOIRES
DELIMITER $
CREATE TRIGGER Paiement_Update_Log
AFTER UPDATE ON Paiement FOR EACH ROW
BEGIN
  INSERT INTO Paiement_Historique (id_paiement, id_nat_patient, id_rdv, montant, date_paiement, operation)
  VALUES (OLD.id_paiement, OLD.id_nat_patient, OLD.id_rdv, OLD.montant, OLD.date_paiement, 'UPDATE');
END$

CREATE TRIGGER Paiement_Delete_Log
AFTER DELETE ON Paiement FOR EACH ROW
BEGIN
  INSERT INTO Paiement_Historique (id_paiement, id_nat_patient, id_rdv, montant, date_paiement, operation)
  VALUES (OLD.id_paiement, OLD.id_nat_patient, OLD.id_rdv, OLD.montant, OLD.date_paiement, 'DELETE');
END$
DELIMITER ;
