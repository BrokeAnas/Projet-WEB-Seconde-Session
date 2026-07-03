using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.Models;

public record CreateSpecialisationDto(
    [Required] string Libelle);

public record CreateAssuranceDto(
    [Required] string Nom,
    string? Type);

public record CreateTypeMaladieDto(
    [Required] string Libelle,
    string? CodeCIM);
