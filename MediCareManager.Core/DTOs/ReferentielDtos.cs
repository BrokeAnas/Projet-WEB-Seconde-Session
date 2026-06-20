using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.DTOs;

public record CreateSpecialisationDto(
    [Required] string Libelle);

public record CreateAssuranceDto(
    [Required] string Nom,
    string? Type);

public record CreateTypeMaladieDto(
    [Required] string Libelle,
    string? CodeCIM);

/// <summary>Statistiques du tableau de bord administrateur.</summary>
public record AdminStatsDto(
    int TotalPatients,
    int TotalMedecins,
    int TotalSecretaires,
    int TotalSucursales,
    int RdvAujourdHui,
    int RdvCetteSemaine,
    decimal RevenuDuMois,
    int TotalPaiements);
