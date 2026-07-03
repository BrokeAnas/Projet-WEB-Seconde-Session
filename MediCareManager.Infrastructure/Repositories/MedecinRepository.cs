using Dapper;
using MediCareManager.Core.Models;
using MediCareManager.Core.Exceptions;
using MediCareManager.Infrastructure.Repositories.Abstractions;
using MySql.Data.MySqlClient;

namespace MediCareManager.Infrastructure.Repositories;

public class MedecinRepository : BaseRepository, IMedecinRepository
{
    public MedecinRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Medecin>> GetAllAsync(string? search = null)
    {
        const string sql = @"
            SELECT m.id_nat, m.nom, m.prenom, m.email, m.id_specialisation, m.id_sucursale,
                   sp.libelle AS Specialisation, su.nom AS Sucursale
            FROM Medecin m
            JOIN SpecialisationMedecin sp ON sp.id_specialisation = m.id_specialisation
            LEFT JOIN Sucursale su ON su.id_sucursale = m.id_sucursale
            WHERE (@Search IS NULL
                   OR m.nom LIKE @SearchPattern
                   OR m.prenom LIKE @SearchPattern
                   OR sp.libelle LIKE @SearchPattern)
            ORDER BY m.nom, m.prenom;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<Medecin>(sql, new
        {
            Search = search,
            SearchPattern = $"%{search}%"
        });
    }

    public async Task<Medecin?> GetByIdAsync(long idNat)
    {
        const string sql = @"
            SELECT m.id_nat, m.nom, m.prenom, m.email, m.mot_de_passe,
                   m.id_specialisation, m.id_sucursale,
                   sp.libelle AS Specialisation, su.nom AS Sucursale
            FROM Medecin m
            JOIN SpecialisationMedecin sp ON sp.id_specialisation = m.id_specialisation
            LEFT JOIN Sucursale su ON su.id_sucursale = m.id_sucursale
            WHERE m.id_nat = @IdNat;";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Medecin>(sql, new { IdNat = idNat });
    }

    public async Task<Medecin?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT id_nat, nom, prenom, email, mot_de_passe, id_specialisation, id_sucursale
            FROM Medecin WHERE email = @Email;";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Medecin>(sql, new { Email = email });
    }

    public async Task<long> CreateAsync(Medecin medecin)
    {
        const string sql = @"
            INSERT INTO Medecin (id_nat, nom, prenom, email, mot_de_passe, id_specialisation, id_sucursale)
            VALUES (@IdNat, @Nom, @Prenom, @Email, @MotDePasse, @IdSpecialisation, @IdSucursale);";

        try
        {
            return await ExecuteInTransactionAsync(async (conn, tx) =>
            {
                await conn.ExecuteAsync(sql, medecin, tx);
                return medecin.IdNat;
            });
        }
        catch (MySqlException ex) when (ex.Number == DuplicateEntry)
        {
            if (ex.Message.Contains("email", StringComparison.OrdinalIgnoreCase))
                throw new EmailAlreadyExistsException(medecin.Email);
            throw new IdNatAlreadyExistsException(medecin.IdNat);
        }
    }

    public async Task UpdateAsync(Medecin medecin)
    {
        const string sql = @"
            UPDATE Medecin
            SET nom = @Nom, prenom = @Prenom, email = @Email,
                id_specialisation = @IdSpecialisation, id_sucursale = @IdSucursale
            WHERE id_nat = @IdNat;";

        try
        {
            await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteAsync(sql, medecin, tx));
        }
        catch (MySqlException ex) when (ex.Number == DuplicateEntry)
        {
            throw new EmailAlreadyExistsException(medecin.Email);
        }
    }

    public async Task DeleteAsync(long idNat)
    {
        const string sql = "DELETE FROM Medecin WHERE id_nat = @IdNat;";

        await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteAsync(sql, new { IdNat = idNat }, tx));
    }

    public async Task<bool> HasRendezVousFutursAsync(long idNat)
    {
        const string sql = @"
            SELECT COUNT(1) FROM RendezVous
            WHERE id_nat_medecin = @IdNat
              AND date_rdv >= CURDATE()
              AND statut IN ('Planifié', 'En cours');";

        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, new { IdNat = idNat }) > 0;
    }
}
