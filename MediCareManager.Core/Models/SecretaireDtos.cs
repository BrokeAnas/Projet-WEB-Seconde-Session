using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.Models;

public record CreateSecretaireDto(
    [Required, RegularExpression(@"^\d{11}$", ErrorMessage = "Le numéro national doit contenir 11 chiffres.")] string IdNat,
    [Required] string Nom,
    [Required] string Prenom,
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string MotDePasse,
    [Required] int IdSucursale);

public record UpdateSecretaireDto(
    string? Nom,
    string? Prenom,
    [EmailAddress] string? Email,
    int? IdSucursale);

public record SecretaireResponseDto(
    long IdNat,
    string Nom,
    string Prenom,
    string Email,
    int IdSucursale,
    string? Sucursale);
