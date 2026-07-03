using Dapper;
using MediCareManager.Core.Models;
using MediCareManager.Core.Exceptions;
using MediCareManager.Infrastructure.Repositories.Abstractions;
using MySql.Data.MySqlClient;

namespace MediCareManager.Infrastructure.Repositories;

public class SpecialisationRepository : BaseRepository, ISpecialisationRepository
{
    public SpecialisationRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<SpecialisationMedecin>> GetAllAsync()
    {
        const string sql = "SELECT id_specialisation, libelle FROM SpecialisationMedecin ORDER BY libelle;";
        using var connection = CreateConnection();
        return await connection.QueryAsync<SpecialisationMedecin>(sql);
    }

    public async Task<int> CreateAsync(SpecialisationMedecin spec)
    {
        const string sql = @"
            INSERT INTO SpecialisationMedecin (libelle) VALUES (@Libelle);
            SELECT LAST_INSERT_ID();";
        try
        {
            return await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteScalarAsync<int>(sql, spec, tx));
        }
        catch (MySqlException ex) when (ex.Number == DuplicateEntry)
        {
            throw new DomainValidationException($"La spécialisation « {spec.Libelle} » existe déjà.");
        }
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM SpecialisationMedecin WHERE id_specialisation = @Id;";
        try
        {
            await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteAsync(sql, new { Id = id }, tx));
        }
        catch (MySqlException ex) when (ex.Number == RowReferenced)
        {
            throw new DomainValidationException("Cette spécialisation est utilisée par un médecin : suppression impossible.");
        }
    }
}

public class AssuranceRepository : BaseRepository, IAssuranceRepository
{
    public AssuranceRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<Assurance>> GetAllAsync()
    {
        const string sql = "SELECT id_assurance, nom, type FROM Assurance ORDER BY nom;";
        using var connection = CreateConnection();
        return await connection.QueryAsync<Assurance>(sql);
    }

    public async Task<int> CreateAsync(Assurance assurance)
    {
        const string sql = @"
            INSERT INTO Assurance (nom, type) VALUES (@Nom, @Type);
            SELECT LAST_INSERT_ID();";
        return await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteScalarAsync<int>(sql, assurance, tx));
    }

    public async Task DeleteAsync(int id)
    {
        // PatientAssurance est en ON DELETE CASCADE : la suppression est toujours possible.
        const string sql = "DELETE FROM Assurance WHERE id_assurance = @Id;";
        await ExecuteInTransactionAsync(async (conn, tx) =>
            await conn.ExecuteAsync(sql, new { Id = id }, tx));
    }
}

public class TypeMaladieRepository : BaseRepository, ITypeMaladieRepository
{
    public TypeMaladieRepository(string connectionString) : base(connectionString) { }

    public async Task<IEnumerable<TypeMaladie>> GetAllAsync()
    {
        const string sql = "SELECT id_maladie, libelle, code_CIM FROM TypeMaladie ORDER BY libelle;";
        using var connection = CreateConnection();
        return await connection.QueryAsync<TypeMaladie>(sql);
    }

    public async Task<int> CreateAsync(TypeMaladie type)
    {
        const string sql = @"
            INSERT INTO TypeMaladie (libelle, code_CIM) VALUES (@Libelle, @CodeCim);
            SELECT LAST_INSERT_ID();";
        try
        {
            return await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteScalarAsync<int>(sql, type, tx));
        }
        catch (MySqlException ex) when (ex.Number == DuplicateEntry)
        {
            throw new DomainValidationException($"Le type de maladie « {type.Libelle} » existe déjà.");
        }
    }

    public async Task DeleteAsync(int id)
    {
        const string sql = "DELETE FROM TypeMaladie WHERE id_maladie = @Id;";
        try
        {
            await ExecuteInTransactionAsync(async (conn, tx) =>
                await conn.ExecuteAsync(sql, new { Id = id }, tx));
        }
        catch (MySqlException ex) when (ex.Number == RowReferenced)
        {
            throw new DomainValidationException("Ce type de maladie est utilisé dans un dossier : suppression impossible.");
        }
    }
}
