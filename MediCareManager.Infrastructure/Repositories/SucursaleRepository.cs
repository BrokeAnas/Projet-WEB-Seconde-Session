using Dapper;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Interfaces.Repositories;
using MySql.Data.MySqlClient;

namespace MediCareManager.Infrastructure.Repositories;

public class SucursaleRepository : BaseRepository, ISucursaleRepository
{
    public SucursaleRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Sucursale>> GetAllAsync()
    {
        const string sql = @"
            SELECT id_sucursale, nom, adresse, telephone, email
            FROM Sucursale ORDER BY nom;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<Sucursale>(sql);
    }

    public async Task<Sucursale?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT id_sucursale, nom, adresse, telephone, email
            FROM Sucursale WHERE id_sucursale = @Id;";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Sucursale>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Sucursale sucursale)
    {
        const string sql = @"
            INSERT INTO Sucursale (nom, adresse, telephone, email)
            VALUES (@Nom, @Adresse, @Telephone, @Email);
            SELECT LAST_INSERT_ID();";

        return await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteScalarAsync<int>(sql, sucursale, tx));
    }

    public async Task UpdateAsync(Sucursale sucursale)
    {
        const string sql = @"
            UPDATE Sucursale
            SET nom = @Nom, adresse = @Adresse, telephone = @Telephone, email = @Email
            WHERE id_sucursale = @IdSucursale;";

        await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteAsync(sql, sucursale, tx));
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Sucursale WHERE id_sucursale = @Id;";

        try
        {
            await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteAsync(sql, new { Id = id }, tx));
        }
        catch (MySqlException ex) when (ex.Number == RowReferenced)
        {
            throw new SucursaleHasPersonnelException(id);
        }
    }

    public async Task<bool> HasPersonnelAsync(int id)
    {
        const string sql = @"
            SELECT
                (SELECT COUNT(*) FROM Medecin   WHERE id_sucursale = @Id) +
                (SELECT COUNT(*) FROM Secretaire WHERE id_sucursale = @Id);";

        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(sql, new { Id = id }) > 0;
    }
}
