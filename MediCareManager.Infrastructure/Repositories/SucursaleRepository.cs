using Dapper;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

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
}
