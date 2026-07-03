using Dapper;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Repositories;

public class PaiementRepository : BaseRepository, IPaiementRepository
{
    public PaiementRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Paiement>> GetAllAsync(
        long? patientId = null, DateOnly? dateDebut = null, DateOnly? dateFin = null)
    {
        const string sql = @"
            SELECT pa.id_paiement, pa.id_nat_patient, pa.id_rdv, pa.montant,
                   pa.date_paiement, pa.mode_paiement,
                   CONCAT(p.nom, ' ', p.prenom) AS PatientNom
            FROM Paiement pa
            JOIN Patient p ON p.id_nat = pa.id_nat_patient
            WHERE (@PatientId IS NULL OR pa.id_nat_patient = @PatientId)
              AND (@DateDebut IS NULL OR pa.date_paiement >= @DateDebut)
              AND (@DateFin   IS NULL OR pa.date_paiement <= @DateFin)
            ORDER BY pa.date_paiement DESC, pa.id_paiement DESC;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<Paiement>(sql, new
        {
            PatientId = patientId,
            DateDebut = dateDebut,
            DateFin = dateFin
        });
    }

    public async Task<Paiement?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT pa.id_paiement, pa.id_nat_patient, pa.id_rdv, pa.montant,
                   pa.date_paiement, pa.mode_paiement,
                   CONCAT(p.nom, ' ', p.prenom) AS PatientNom
            FROM Paiement pa
            JOIN Patient p ON p.id_nat = pa.id_nat_patient
            WHERE pa.id_paiement = @Id;";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Paiement>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Paiement paiement)
    {
        const string sql = @"
            INSERT INTO Paiement (id_nat_patient, id_rdv, montant, date_paiement, mode_paiement)
            VALUES (@IdNatPatient, @IdRdv, @Montant, @DatePaiement, @ModePaiement);
            SELECT LAST_INSERT_ID();";

        return await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteScalarAsync<int>(sql, paiement, tx));
    }

    public async Task UpdateAsync(Paiement paiement)
    {
        // Le trigger Paiement_Update_Log enregistre automatiquement l'ancienne valeur.
        const string sql = @"
            UPDATE Paiement
            SET montant = @Montant, date_paiement = @DatePaiement, mode_paiement = @ModePaiement
            WHERE id_paiement = @IdPaiement;";

        await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteAsync(sql, paiement, tx));
    }

    public async Task DeleteAsync(int id)
    {
        // Le trigger Paiement_Delete_Log enregistre automatiquement la suppression.
        const string sql = "DELETE FROM Paiement WHERE id_paiement = @Id;";

        await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteAsync(sql, new { Id = id }, tx));
    }

    public async Task<IEnumerable<PaiementHistorique>> GetAuditLogAsync()
    {
        const string sql = @"
            SELECT h.id_historique, h.id_paiement, h.id_nat_patient, h.id_rdv, h.montant,
                   h.date_paiement, h.operation, h.date_operation,
                   CONCAT(p.nom, ' ', p.prenom) AS PatientNom
            FROM Paiement_Historique h
            LEFT JOIN Patient p ON p.id_nat = h.id_nat_patient
            ORDER BY h.date_operation DESC, h.id_historique DESC;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<PaiementHistorique>(sql);
    }
}
