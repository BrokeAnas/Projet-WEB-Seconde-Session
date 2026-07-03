-- =============================================================
--  MediCare Manager (version simplifiée) - Données de test
--  Compte de test : admin@medicare.be / Admin1234!
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

-- ---------- Administrateur (mdp : Admin1234!) ----------
INSERT INTO Administrateur (id_admin, nom, prenom, email, mot_de_passe) VALUES
  (1, 'Système', 'Admin', 'admin@medicare.be', '$2a$11$CJoeGr.ubsgJn9IMlo5aROEVzIq4uOIDKsxTbrjTp/Ju6ywtlC8h6');

-- ---------- Médecins ----------
INSERT INTO Medecin (id_nat, nom, prenom, id_specialisation, id_sucursale) VALUES
  (72041810126, 'Martin',  'Pierre', 1, 1),
  (80092320275, 'Dubois',  'Sophie', 2, 2),
  (69120730350, 'Leroy',   'Jean',   3, 3),
  (88063040404, 'Lambert', 'Marie',  3, 1);

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

-- ---------- Rendez-vous (statuts variés) ----------
INSERT INTO RendezVous (id_nat_patient, id_nat_medecin, id_sucursale, date_rdv, heure_debut, heure_fin, motif, statut) VALUES
  (85031211174, 72041810126, 1, '2026-06-10', '09:00:00', '09:30:00', 'Consultation de contrôle',     'Terminé'),
  (90072522294, 72041810126, 1, '2026-06-10', '10:00:00', '10:30:00', 'Douleurs thoraciques',         'En cours'),
  (78110333331, 88063040404, 1, '2026-06-10', '11:00:00', '11:30:00', 'Renouvellement d''ordonnance', 'Planifié'),
  (82013044494, 80092320275, 2, '2026-06-10', '14:00:00', '14:45:00', 'Vaccination',                  'Planifié'),
  (15061023446, 80092320275, 2, '2026-06-10', '15:00:00', '15:30:00', 'Contrôle pédiatrique',         'Planifié'),
  (95091755523, 69120730350, 3, '2026-06-11', '09:30:00', '10:00:00', 'Consultation générale',        'Planifié'),
  (68050912303, 69120730350, 3, '2026-06-11', '10:30:00', '11:00:00', 'Suivi hypertension',           'Planifié'),
  (99021445653, 72041810126, 1, '2026-06-12', '09:00:00', '09:30:00', 'Bilan cardiologique',          'Planifié'),
  (12031934566, 80092320275, 2, '2026-06-12', '11:00:00', '11:30:00', 'Asthme - suivi',               'Planifié'),
  (75082856730, 88063040404, 1, '2026-06-09', '16:00:00', '16:30:00', 'Consultation',                 'Terminé'),
  (85031211174, 72041810126, 1, '2026-06-05', '09:00:00', '09:30:00', 'Consultation',                 'Terminé'),
  (90072522294, 88063040404, 1, '2026-06-02', '10:00:00', '10:30:00', 'Certificat médical',           'Terminé'),
  (78110333331, 69120730350, 3, '2026-05-28', '14:00:00', '14:30:00', 'Suivi',                        'Terminé'),
  (82013044494, 80092320275, 2, '2026-06-13', '10:00:00', '10:30:00', 'Consultation',                 'Planifié'),
  (95091755523, 69120730350, 3, '2026-06-08', '15:00:00', '15:30:00', 'Consultation',                 'Annulé');
