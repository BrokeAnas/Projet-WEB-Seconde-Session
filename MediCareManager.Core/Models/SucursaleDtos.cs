using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.Models;

public record CreateSucursaleDto(
    [Required] string Nom,
    [Required] string Adresse,
    string? Telephone,
    [EmailAddress] string? Email);

public record UpdateSucursaleDto(
    string? Nom,
    string? Adresse,
    string? Telephone,
    [EmailAddress] string? Email);
