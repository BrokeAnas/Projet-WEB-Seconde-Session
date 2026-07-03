using Dapper;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Repositories;

public class MedecinRepository : BaseRepository, IMedecinRepository
{
    public MedecinRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Medecin>> GetAllAsync()
    {
        const string sql = @"
            SELECT m.id_nat, m.nom, m.prenom,
                   sp.libelle AS Specialisation, su.nom AS Sucursale
            FROM Medecin m
            JOIN SpecialisationMedecin sp ON sp.id_specialisation = m.id_specialisation
            LEFT JOIN Sucursale su ON su.id_sucursale = m.id_sucursale
            ORDER BY m.nom, m.prenom;";

        using var connection = CreateConnection();
        return await connection.QueryAsync<Medecin>(sql);
    }
}
