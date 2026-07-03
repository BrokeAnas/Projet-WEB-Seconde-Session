using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.Models;

public record CreateMedecinDto(
    [Required, RegularExpression(@"^\d{11}$", ErrorMessage = "Le numéro national doit contenir 11 chiffres.")] string IdNat,
    [Required] string Nom,
    [Required] string Prenom,
    [Required, EmailAddress] string Email,
    [Required, MinLength(8)] string MotDePasse,
    [Required] int IdSpecialisation,
    int? IdSucursale);

public record UpdateMedecinDto(
    string? Nom,
    string? Prenom,
    [EmailAddress] string? Email,
    int? IdSpecialisation,
    int? IdSucursale);

public record MedecinResponseDto(
    long IdNat,
    string Nom,
    string Prenom,
    string Email,
    string? Specialisation,
    string? Sucursale);
