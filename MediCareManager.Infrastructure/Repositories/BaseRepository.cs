using System.Data;
using MediCareManager.Infrastructure.Configuration;
using MySql.Data.MySqlClient;

namespace MediCareManager.Infrastructure.Repositories;

/// <summary>
/// Fournit la connexion MySQL et le motif transactionnel commun aux repositories Dapper.
/// </summary>
public abstract class BaseRepository
{
    protected readonly string ConnectionString;

    protected BaseRepository(string connectionString)
    {
        ConnectionString = connectionString;
    }

    protected MySqlConnection CreateConnection()
        => DatabaseConfiguration.CreateConnection(ConnectionString);

    /// <summary>
    /// Exécute une opération d'écriture dans une transaction (commit en cas de succès, rollback sinon).
    /// </summary>
    protected async Task<T> ExecuteInTransactionAsync<T>(Func<MySqlConnection, IDbTransaction, Task<T>> action)
    {
        using var connection = CreateConnection();
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();
        try
        {
            var result = await action(connection, transaction);
            await transaction.CommitAsync();
            return result;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>Numéro d'erreur MySQL pour une violation de clé unique / primaire.</summary>
    protected const int DuplicateEntry = 1062;

    /// <summary>Numéro d'erreur MySQL pour une violation de contrainte de clé étrangère.</summary>
    protected const int ForeignKeyViolation = 1452;

    /// <summary>Numéro d'erreur MySQL pour une suppression bloquée par une clé étrangère.</summary>
    protected const int RowReferenced = 1451;
}
