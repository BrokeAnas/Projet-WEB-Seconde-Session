using Dapper;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Repositories;

public class RendezVousRepository : BaseRepository, IRendezVousRepository
{
    public RendezVousRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<RendezVous>> GetAllAsync(long? medecinId = null, DateOnly? date = null)
    {
        const string sql = @"
            SELECT r.id_rdv, r.id_nat_patient, r.id_nat_medecin, r.id_sucursale,
                   r.date_rdv, r.heure_debut, r.heure_fin, r.motif, r.statut,
                   CONCAT(p.nom, ' ', p.prenom) AS PatientNom,
                   CONCAT(m.nom, ' ', m.prenom) AS MedecinNom,
                   su.nom AS SucursaleNom
            FROM RendezVous r
            JOIN Patient p   ON p.id_nat = r.id_nat_patient
            JOIN Medecin m   ON m.id_nat = r.id_nat_medecin
            JOIN Sucursale su ON su.id_sucursale = r.id_sucursale
            WHERE (@MedecinId IS NULL OR r.id_nat_medecin = @MedecinId)
              AND (@Date IS NULL      OR r.date_rdv       = @Date)
            ORDER BY r.date_rdv, r.heure_debut;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<RendezVous>(sql, new
        {
            MedecinId = medecinId,
            Date = date
        });
    }

    public async Task<RendezVous?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT r.id_rdv, r.id_nat_patient, r.id_nat_medecin, r.id_sucursale,
                   r.date_rdv, r.heure_debut, r.heure_fin, r.motif, r.statut,
                   CONCAT(p.nom, ' ', p.prenom) AS PatientNom,
                   CONCAT(m.nom, ' ', m.prenom) AS MedecinNom,
                   su.nom AS SucursaleNom
            FROM RendezVous r
            JOIN Patient p   ON p.id_nat = r.id_nat_patient
            JOIN Medecin m   ON m.id_nat = r.id_nat_medecin
            JOIN Sucursale su ON su.id_sucursale = r.id_sucursale
            WHERE r.id_rdv = @Id;";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<RendezVous>(sql, new { Id = id });
    }

    public async Task<bool> HasConflitAsync(long medecinId, DateOnly date, TimeOnly heureDebut, TimeOnly heureFin)
    {
        // Intersection d'intervalles : (debut_existant < fin_demandee) ET (fin_existante > debut_demande).
        const string sql = @"
            SELECT COUNT(*) FROM RendezVous
            WHERE id_nat_medecin = @MedecinId
              AND date_rdv = @Date
              AND heure_debut < @HeureFin
              AND heure_fin   > @HeureDebut
              AND statut NOT IN ('Annulé');";

        using var connection = CreateConnection();
        var count = await connection.ExecuteScalarAsync<int>(sql, new
        {
            MedecinId = medecinId,
            Date = date,
            HeureDebut = heureDebut,
            HeureFin = heureFin
        });
        return count > 0;
    }

    public async Task<int> CreateAsync(RendezVous rdv)
    {
        const string sql = @"
            INSERT INTO RendezVous
                (id_nat_patient, id_nat_medecin, id_sucursale,
                 date_rdv, heure_debut, heure_fin, motif, statut)
            VALUES
                (@IdNatPatient, @IdNatMedecin, @IdSucursale,
                 @DateRdv, @HeureDebut, @HeureFin, @Motif, @Statut);
            SELECT LAST_INSERT_ID();";

        return await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteScalarAsync<int>(sql, rdv, tx));
    }

    public async Task UpdateStatutAsync(int id, string statut)
    {
        const string sql = "UPDATE RendezVous SET statut = @Statut WHERE id_rdv = @Id;";

        await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteAsync(sql, new { Id = id, Statut = statut }, tx));
    }
}
