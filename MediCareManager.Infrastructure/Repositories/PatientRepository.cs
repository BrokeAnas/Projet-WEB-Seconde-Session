using Dapper;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;
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
                "Impossible de supprimer ce patient : des rendez-vous y sont liés.");
        }
    }

    public async Task<bool> ExistsAsync(long idNat)
    {
        const string sql = "SELECT COUNT(1) FROM Patient WHERE id_nat = @IdNat;";

        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, new { IdNat = idNat }) > 0;
    }
}
