using Dapper;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Interfaces.Repositories;
using MySql.Data.MySqlClient;

namespace MediCareManager.Infrastructure.Repositories;

public class PatientRepository : BaseRepository, IPatientRepository
{
    public PatientRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Patient>> GetAllAsync(string? search = null, int page = 1, int pageSize = 20)
    {
        const string sql = @"
            SELECT id_nat, nom, prenom, date_naissance, adresse, telephone, email
            FROM Patient
            WHERE (@Search IS NULL
                   OR nom LIKE @SearchPattern
                   OR prenom LIKE @SearchPattern
                   OR CAST(id_nat AS CHAR) LIKE @SearchPattern)
            ORDER BY nom, prenom
            LIMIT @PageSize OFFSET @Offset;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<Patient>(sql, new
        {
            Search = search,
            SearchPattern = $"%{search}%",
            PageSize = pageSize,
            Offset = (page - 1) * pageSize
        });
    }

    public async Task<Patient?> GetByIdAsync(long idNat)
    {
        const string sql = @"
            SELECT id_nat, nom, prenom, date_naissance, adresse, telephone, email
            FROM Patient WHERE id_nat = @IdNat;";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Patient>(sql, new { IdNat = idNat });
    }

    public async Task<long> CreateAsync(Patient patient)
    {
        const string sql = @"
            INSERT INTO Patient (id_nat, nom, prenom, date_naissance, adresse, telephone, email)
            VALUES (@IdNat, @Nom, @Prenom, @DateNaissance, @Adresse, @Telephone, @Email);";

        try
        {
            return await ExecuteInTransactionAsync(async (conn, tx) =>
            {
                await conn.ExecuteAsync(sql, patient, tx);
                return patient.IdNat;
            });
        }
        catch (MySqlException ex) when (ex.Number == DuplicateEntry)
        {
            throw new IdNatAlreadyExistsException(patient.IdNat);
        }
    }

    public async Task UpdateAsync(Patient patient)
    {
        const string sql = @"
            UPDATE Patient
            SET nom = @Nom, prenom = @Prenom, date_naissance = @DateNaissance,
                adresse = @Adresse, telephone = @Telephone, email = @Email
            WHERE id_nat = @IdNat;";

        await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteAsync(sql, patient, tx));
    }

    public async Task DeleteAsync(long idNat)
    {
        const string sql = "DELETE FROM Patient WHERE id_nat = @IdNat;";

        try
        {
            await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteAsync(sql, new { IdNat = idNat }, tx));
        }
        catch (MySqlException ex) when (ex.Number == RowReferenced)
        {
            throw new DomainValidationException(
                "Impossible de supprimer ce patient : des rendez-vous ou des paiements y sont liés.");
        }
    }

    public async Task<bool> ExistsAsync(long idNat)
    {
        const string sql = "SELECT COUNT(1) FROM Patient WHERE id_nat = @IdNat;";

        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, new { IdNat = idNat }) > 0;
    }

    public async Task<IEnumerable<PatientMaladie>> GetMaladiesAsync(long idNat)
    {
        const string sql = @"
            SELECT pm.id_nat_patient, pm.id_maladie, pm.id_nat_medecin,
                   pm.date_diagnostic, pm.observations,
                   tm.libelle, tm.code_CIM,
                   CONCAT(m.nom, ' ', m.prenom) AS MedecinNom
            FROM PatientMaladie pm
            JOIN TypeMaladie tm ON tm.id_maladie = pm.id_maladie
            LEFT JOIN Medecin m ON m.id_nat = pm.id_nat_medecin
            WHERE pm.id_nat_patient = @IdNat
            ORDER BY pm.date_diagnostic DESC;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<PatientMaladie>(sql, new { IdNat = idNat });
    }

    public async Task AddMaladieAsync(PatientMaladie pm)
    {
        const string sql = @"
            INSERT INTO PatientMaladie (id_nat_patient, id_maladie, id_nat_medecin, date_diagnostic, observations)
            VALUES (@IdNatPatient, @IdMaladie, @IdNatMedecin, @DateDiagnostic, @Observations);";

        try
        {
            await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteAsync(sql, pm, tx));
        }
        catch (MySqlException ex) when (ex.Number == DuplicateEntry)
        {
            throw new DomainValidationException("Ce diagnostic est déjà enregistré pour ce patient.");
        }
    }

    public async Task<IEnumerable<PatientAssurance>> GetAssurancesAsync(long idNat)
    {
        const string sql = @"
            SELECT pa.id_nat_patient, pa.id_assurance, pa.numero_affiliation,
                   pa.date_debut, pa.date_fin,
                   a.nom, a.type
            FROM PatientAssurance pa
            JOIN Assurance a ON a.id_assurance = pa.id_assurance
            WHERE pa.id_nat_patient = @IdNat;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<PatientAssurance>(sql, new { IdNat = idNat });
    }

    public async Task AddAssuranceAsync(PatientAssurance pa)
    {
        const string sql = @"
            INSERT INTO PatientAssurance (id_nat_patient, id_assurance, numero_affiliation, date_debut, date_fin)
            VALUES (@IdNatPatient, @IdAssurance, @NumeroAffiliation, @DateDebut, @DateFin);";

        try
        {
            await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteAsync(sql, pa, tx));
        }
        catch (MySqlException ex) when (ex.Number == DuplicateEntry)
        {
            throw new DomainValidationException("Cette assurance est déjà associée au patient.");
        }
    }

    public async Task RemoveAssuranceAsync(long idNat, int idAssurance)
    {
        const string sql = @"
            DELETE FROM PatientAssurance
            WHERE id_nat_patient = @IdNat AND id_assurance = @IdAssurance;";

        await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteAsync(sql, new { IdNat = idNat, IdAssurance = idAssurance }, tx));
    }
}
