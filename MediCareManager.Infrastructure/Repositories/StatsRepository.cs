using Dapper;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Repositories;

public class StatsRepository : BaseRepository, IStatsRepository
{
    public StatsRepository(string connectionString) : base(connectionString) { }

    public async Task<AdminStatsDto> GetStatsAsync()
    {
        const string sql = @"
            SELECT
                (SELECT COUNT(*) FROM Patient)     AS TotalPatients,
                (SELECT COUNT(*) FROM Medecin)     AS TotalMedecins,
                (SELECT COUNT(*) FROM Secretaire)  AS TotalSecretaires,
                (SELECT COUNT(*) FROM Sucursale)   AS TotalSucursales,
                (SELECT COUNT(*) FROM RendezVous
                    WHERE date_rdv = CURDATE())    AS RdvAujourdHui,
                (SELECT COUNT(*) FROM RendezVous
                    WHERE YEARWEEK(date_rdv, 1) = YEARWEEK(CURDATE(), 1)) AS RdvCetteSemaine,
                (SELECT COALESCE(SUM(montant), 0) FROM Paiement
                    WHERE YEAR(date_paiement)  = YEAR(CURDATE())
                      AND MONTH(date_paiement) = MONTH(CURDATE()))        AS RevenuDuMois,
                (SELECT COUNT(*) FROM Paiement)    AS TotalPaiements;";

        using var connection = CreateConnection();
        return await connection.QueryFirstAsync<AdminStatsDto>(sql);
    }
}
