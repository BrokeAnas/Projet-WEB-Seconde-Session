using Dapper;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Interfaces.Repositories;

namespace MediCareManager.Infrastructure.Repositories;

public class AdministrateurRepository : BaseRepository, IAdministrateurRepository
{
    public AdministrateurRepository(string connectionString) : base(connectionString) { }

    public async Task<Administrateur?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT id_admin, nom, prenom, email, mot_de_passe
            FROM Administrateur WHERE email = @Email;";

        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Administrateur>(sql, new { Email = email });
    }
}
