using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.Models;

public record CreatePatientDto(
    [Required, RegularExpression(@"^\d{11}$", ErrorMessage = "Le numéro national doit contenir 11 chiffres.")] string IdNat,
    [Required, MaxLength(100)] string Nom,
    [Required, MaxLength(100)] string Prenom,
    [Required] DateOnly DateNaissance,
    string? Adresse,
    string? Telephone,
    [EmailAddress] string? Email);

public record UpdatePatientDto(
    [MaxLength(100)] string? Nom,
    [MaxLength(100)] string? Prenom,
    DateOnly? DateNaissance,
    string? Adresse,
    string? Telephone,
    [EmailAddress] string? Email);

public record PatientResponseDto(
    long IdNat,
    string Nom,
    string Prenom,
    DateOnly DateNaissance,
    string? Adresse,
    string? Telephone,
    string? Email);

public record AddMaladieDto(
    [Required] int IdMaladie,
    [Required] DateOnly DateDiagnostic,
    string? Observations);

public record AddAssuranceDto(
    [Required] int IdAssurance,
    string? NumeroAffiliation,
    DateOnly? DateDebut,
    DateOnly? DateFin);
