using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.DTOs;

public record CreateRendezVousDto(
    [Required] long IdNatPatient,
    [Required] long IdNatMedecin,
    long? IdNatSecretaire,
    [Required] int IdSucursale,
    [Required] DateOnly DateRdv,
    [Required] TimeOnly HeureDebut,
    [Required] TimeOnly HeureFin,
    string? Motif);

public record UpdateRendezVousDto(
    DateOnly? DateRdv,
    TimeOnly? HeureDebut,
    TimeOnly? HeureFin,
    string? Motif,
    string? Statut);

public record UpdateStatutDto(
    [Required] string Statut);

public record RendezVousResponseDto(
    int IdRdv,
    long IdNatPatient,
    string PatientNom,
    long IdNatMedecin,
    string MedecinNom,
    int IdSucursale,
    string SucursaleNom,
    DateOnly DateRdv,
    TimeOnly HeureDebut,
    TimeOnly HeureFin,
    string? Motif,
    string Statut);
