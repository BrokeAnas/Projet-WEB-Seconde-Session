namespace MediCareManager.Core.Models;

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
