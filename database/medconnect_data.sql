-- =============================================================
--  MediCare Manager - Données de test
--  Mots de passe (BCrypt) :
--    Admin    -> Admin1234!
--    Médecins -> Medecin1234!
--    Secrét.  -> Secret1234!
-- =============================================================
USE medconnect;

-- ---------- Spécialisations ----------
INSERT INTO SpecialisationMedecin (id_specialisation, libelle) VALUES
  (1, 'Cardiologie'),
  (2, 'Pédiatrie'),
  (3, 'Médecine générale');

-- ---------- Succursales ----------
INSERT INTO Sucursale (id_sucursale, nom, adresse, telephone, email) VALUES
  (1, 'Cabinet Bruxelles-Centre', 'Rue de la Loi 100, 1000 Bruxelles', '02/123.45.67', 'bruxelles@medicare.be'),
  (2, 'Cabinet de Liège',         'Boulevard d''Avroy 25, 4000 Liège', '04/222.33.44', 'liege@medicare.be'),
  (3, 'Cabinet de Namur',         'Place d''Armes 8, 5000 Namur',      '081/55.66.77', 'namur@medicare.be');

-- ---------- Administrateur ----------
INSERT INTO Administrateur (id_admin, nom, prenom, email, mot_de_passe) VALUES
  (1, 'Système', 'Admin', 'admin@medicare.be', '$2a$11$CJoeGr.ubsgJn9IMlo5aROEVzIq4uOIDKsxTbrjTp/Ju6ywtlC8h6');

-- ---------- Médecins (mdp: Medecin1234!) ----------
INSERT INTO Medecin (id_nat, nom, prenom, email, mot_de_passe, id_specialisation, id_sucursale) VALUES
  (72041810126, 'Martin',  'Pierre', 'dr.martin@medicare.be',  '$2a$11$q8yW8s9ABzCLxA8JAYuHfusb8FJ7TxofqciTe3sJWVE7f5Usyk6Oy', 1, 1),
  (80092320275, 'Dubois',  'Sophie', 'dr.dubois@medicare.be',  '$2a$11$q8yW8s9ABzCLxA8JAYuHfusb8FJ7TxofqciTe3sJWVE7f5Usyk6Oy', 2, 2),
  (69120730350, 'Leroy',   'Jean',   'dr.leroy@medicare.be',   '$2a$11$q8yW8s9ABzCLxA8JAYuHfusb8FJ7TxofqciTe3sJWVE7f5Usyk6Oy', 3, 3),
  (88063040404, 'Lambert', 'Marie',  'dr.lambert@medicare.be', '$2a$11$q8yW8s9ABzCLxA8JAYuHfusb8FJ7TxofqciTe3sJWVE7f5Usyk6Oy', 3, 1);

-- ---------- Secrétaires (mdp: Secret1234!) ----------
INSERT INTO Secretaire (id_nat, nom, prenom, email, mot_de_passe, id_sucursale) VALUES
  (91021150523, 'Durand', 'Alice', 'secretaire@medicare.be', '$2a$11$KSASliRSbUE2MNwfj6hrCOGc6IdYB8o6eRkmphcN0pULJUmT8nLXu', 1),
  (86101960621, 'Moreau', 'Luc',   'l.moreau@medicare.be',   '$2a$11$KSASliRSbUE2MNwfj6hrCOGc6IdYB8o6eRkmphcN0pULJUmT8nLXu', 2),
  (93070870785, 'Petit',  'Emma',  'e.petit@medicare.be',    '$2a$11$KSASliRSbUE2MNwfj6hrCOGc6IdYB8o6eRkmphcN0pULJUmT8nLXu', 3);

-- ---------- Assurances ----------
INSERT INTO Assurance (id_assurance, nom, type) VALUES
  (1, 'Mutualité Chrétienne',  'Mutuelle'),
  (2, 'Mutualité Socialiste',  'Mutuelle'),
  (3, 'AXA',                   'Privée');

-- ---------- Types de maladie (codes CIM-10) ----------
INSERT INTO TypeMaladie (id_maladie, libelle, code_CIM) VALUES
  (1, 'Hypertension',   'I10'),
  (2, 'Diabète type 2', 'E11'),
  (3, 'Asthme',         'J45'),
  (4, 'Dépression',     'F32'),
  (5, 'Fracture',       'S42');

-- ---------- Patients (numéros de Registre National belges valides) ----------
INSERT INTO Patient (id_nat, nom, prenom, date_naissance, adresse, telephone, email) VALUES
  (85031211174, 'Dupont',   'Jean',   '1985-03-12', 'Avenue Louise 12, 1050 Bruxelles', '0470/11.22.33', 'jean.dupont@example.be'),
  (90072522294, 'Lambert',  'Claire', '1990-07-25', 'Rue Haute 45, 1000 Bruxelles',     '0471/22.33.44', 'claire.lambert@example.be'),
  (78110333331, 'Martin',   'Sophie', '1978-11-03', 'Chaussée de Wavre 200, 1040 Bruxelles', '0472/33.44.55', 'sophie.martin@example.be'),
  (82013044494, 'Bernard',  'Marc',   '1982-01-30', 'Quai de la Batte 7, 4000 Liège',   '0473/44.55.66', 'marc.bernard@example.be'),
  (95091755523, 'Thomas',   'Julie',  '1995-09-17', 'Rue Saint-Léonard 88, 4000 Liège', '0474/55.66.77', 'julie.thomas@example.be'),
  (68050912303, 'Petit',    'Robert', '1968-05-09', 'Rue de Fer 30, 5000 Namur',        '0475/66.77.88', 'robert.petit@example.be'),
  (15061023446, 'Janssens', 'Lucas',  '2015-06-10', 'Rue des Brasseurs 14, 5000 Namur', '0476/77.88.99', NULL),
  (12031934566, 'Maes',     'Emma',   '2012-03-19', 'Avenue de la Toison d''Or 3, 1060 Bruxelles', '0477/88.99.00', NULL),
  (99021445653, 'Willems',  'Nina',   '1999-02-14', 'Boulevard de la Sauvenière 50, 4000 Liège', '0478/99.00.11', 'nina.willems@example.be'),
  (75082856730, 'Peeters',  'Daniel', '1975-08-28', 'Rue de Bruxelles 22, 5000 Namur',  '0479/00.11.22', 'daniel.peeters@example.be');

-- ---------- Liens Patient / Assurance ----------
INSERT INTO PatientAssurance (id_nat_patient, id_assurance, numero_affiliation, date_debut, date_fin) VALUES
  (85031211174, 1, 'MC-2020-001', '2020-01-01', NULL),
  (90072522294, 2, 'MS-2019-114', '2019-06-01', NULL),
  (78110333331, 3, 'AXA-77-882',  '2021-03-15', NULL),
  (82013044494, 1, 'MC-2018-552', '2018-09-01', NULL),
  (95091755523, 2, 'MS-2022-309', '2022-01-10', NULL);

-- ---------- Dossiers médicaux (diagnostics) ----------
INSERT INTO PatientMaladie (id_nat_patient, id_maladie, id_nat_medecin, date_diagnostic, observations) VALUES
  (85031211174, 1, 72041810126, '2024-02-10', 'Tension 150/95 mmHg. Traitement antihypertenseur initié.'),
  (90072522294, 3, 80092320275, '2023-11-05', 'Asthme allergique. Corticoïde inhalé prescrit.'),
  (78110333331, 4, 69120730350, '2024-06-20', 'Épisode dépressif modéré. Suivi psychologique recommandé.'),
  (82013044494, 2, 88063040404, '2022-09-12', 'Diabète de type 2. HbA1c à 7,8%. Metformine.'),
  (68050912303, 1, 69120730350, '2023-01-18', 'HTA essentielle. Surveillance trimestrielle.'),
  (99021445653, 5, 88063040404, '2025-12-01', 'Fracture du radius distal gauche. Plâtre 6 semaines.');

-- ---------- Rendez-vous (statuts variés) ----------
INSERT INTO RendezVous (id_nat_patient, id_nat_medecin, id_nat_secretaire, id_sucursale, date_rdv, heure_debut, heure_fin, motif, statut) VALUES
  (85031211174, 72041810126, 91021150523, 1, '2026-06-10', '09:00:00', '09:30:00', 'Consultation de contrôle',     'Terminé'),
  (90072522294, 72041810126, 91021150523, 1, '2026-06-10', '10:00:00', '10:30:00', 'Douleurs thoraciques',         'En cours'),
  (78110333331, 88063040404, 91021150523, 1, '2026-06-10', '11:00:00', '11:30:00', 'Renouvellement d''ordonnance', 'Planifié'),
  (82013044494, 80092320275, 86101960621, 2, '2026-06-10', '14:00:00', '14:45:00', 'Vaccination',                  'Planifié'),
  (15061023446, 80092320275, 86101960621, 2, '2026-06-10', '15:00:00', '15:30:00', 'Contrôle pédiatrique',         'Planifié'),
  (95091755523, 69120730350, 93070870785, 3, '2026-06-11', '09:30:00', '10:00:00', 'Consultation générale',        'Planifié'),
  (68050912303, 69120730350, 93070870785, 3, '2026-06-11', '10:30:00', '11:00:00', 'Suivi hypertension',           'Planifié'),
  (99021445653, 72041810126, 91021150523, 1, '2026-06-12', '09:00:00', '09:30:00', 'Bilan cardiologique',          'Planifié'),
  (12031934566, 80092320275, 86101960621, 2, '2026-06-12', '11:00:00', '11:30:00', 'Asthme - suivi',               'Planifié'),
  (75082856730, 88063040404, 91021150523, 1, '2026-06-09', '16:00:00', '16:30:00', 'Consultation',                 'Terminé'),
  (85031211174, 72041810126, 91021150523, 1, '2026-06-05', '09:00:00', '09:30:00', 'Consultation',                 'Terminé'),
  (90072522294, 88063040404, 91021150523, 1, '2026-06-02', '10:00:00', '10:30:00', 'Certificat médical',           'Terminé'),
  (78110333331, 69120730350, 93070870785, 3, '2026-05-28', '14:00:00', '14:30:00', 'Dépression - suivi',           'Terminé'),
  (82013044494, 80092320275, 86101960621, 2, '2026-06-13', '10:00:00', '10:30:00', 'Consultation',                 'Planifié'),
  (95091755523, 69120730350, 93070870785, 3, '2026-06-08', '15:00:00', '15:30:00', 'Consultation',                 'Annulé');

-- ---------- Paiements (mois courant : juin 2026) ----------
INSERT INTO Paiement (id_nat_patient, id_rdv, montant, date_paiement, mode_paiement) VALUES
  (85031211174,  1, 35.00, '2026-06-10', 'Carte'),
  (90072522294,  2, 50.00, '2026-06-10', 'Bancontact'),
  (75082856730, 10, 35.00, '2026-06-09', 'Espèces'),
  (85031211174, 11, 35.00, '2026-06-05', 'Carte'),
  (90072522294, 12, 25.00, '2026-06-02', 'Virement'),
  (78110333331, 13, 60.00, '2026-06-01', 'Carte'),
  (82013044494, NULL, 45.00, '2026-06-03', 'Carte'),
  (95091755523, NULL, 30.00, '2026-06-04', 'Espèces'),
  (68050912303, NULL, 40.00, '2026-06-06', 'Bancontact'),
  (99021445653, NULL, 55.00, '2026-06-07', 'Carte');

-- ---------- Déclenchement des triggers d'audit (modifications) ----------
-- Ces UPDATE génèrent automatiquement des lignes dans Paiement_Historique
-- via le trigger Paiement_Update_Log (preuve de fonctionnement de l'audit).
UPDATE Paiement SET montant = 40.00       WHERE id_paiement = 1;
UPDATE Paiement SET mode_paiement = 'Virement' WHERE id_paiement = 3;
