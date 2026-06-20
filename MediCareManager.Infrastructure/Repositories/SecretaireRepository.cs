using Dapper;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Interfaces.Repositories;
using MySql.Data.MySqlClient;

namespace MediCareManager.Infrastructure.Repositories;

public class SecretaireRepository : BaseRepository, ISecretaireRepository
{
    public SecretaireRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Secretaire>> GetAllAsync()
    {
        const string sql = @"
            SELECT s.id_nat, s.nom, s.prenom, s.email, s.id_sucursale, su.nom AS Sucursale
            FROM Secretaire s
            JOIN Sucursale su ON su.id_sucursale = s.id_sucursale
            ORDER BY s.nom, s.prenom;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<Secretaire>(sql);
    }

    public async Task<Secretaire?> GetByIdAsync(long idNat)
    {
        const string sql = @"
            SELECT s.id_nat, s.nom, s.prenom, s.email, s.mot_de_passe, s.id_sucursale,
                   su.nom AS Sucursale
            FROM Secretaire s
            JOIN Sucursale su ON su.id_sucursale = s.id_sucursale
            WHERE s.id_nat = @IdNat;";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Secretaire>(sql, new { IdNat = idNat });
    }

    public async Task<Secretaire?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT id_nat, nom, prenom, email, mot_de_passe, id_sucursale
            FROM Secretaire WHERE email = @Email;";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Secretaire>(sql, new { Email = email });
    }

    public async Task<long> CreateAsync(Secretaire secretaire)
    {
        const string sql = @"
            INSERT INTO Secretaire (id_nat, nom, prenom, email, mot_de_passe, id_sucursale)
            VALUES (@IdNat, @Nom, @Prenom, @Email, @MotDePasse, @IdSucursale);";

        try
        {
            return await ExecuteInTransactionAsync(async (conn, tx) =>
            {
                await conn.ExecuteAsync(sql, secretaire, tx);
                return secretaire.IdNat;
            });
        }
        catch (MySqlException ex) when (ex.Number == DuplicateEntry)
        {
            if (ex.Message.Contains("email", StringComparison.OrdinalIgnoreCase))
                throw new EmailAlreadyExistsException(secretaire.Email);
            throw new IdNatAlreadyExistsException(secretaire.IdNat);
        }
    }

    public async Task UpdateAsync(Secretaire secretaire)
    {
        const string sql = @"
            UPDATE Secretaire
            SET nom = @Nom, prenom = @Prenom, email = @Email, id_sucursale = @IdSucursale
            WHERE id_nat = @IdNat;";

        try
        {
            await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteAsync(sql, secretaire, tx));
        }
        catch (MySqlException ex) when (ex.Number == DuplicateEntry)
        {
            throw new EmailAlreadyExistsException(secretaire.Email);
        }
    }

    public async Task DeleteAsync(long idNat)
    {
        const string sql = "DELETE FROM Secretaire WHERE id_nat = @IdNat;";

        await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteAsync(sql, new { IdNat = idNat }, tx));
    }
}
