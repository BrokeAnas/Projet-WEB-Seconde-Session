using MediCareManager.Core.Exceptions;
using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

public class SucursaleUseCases : ISucursaleUseCases
{
    private readonly ISucursaleGateway _sucursaleGateway;

    public SucursaleUseCases(ISucursaleGateway sucursaleGateway)
    {
        _sucursaleGateway = sucursaleGateway;
    }

    public Task<IEnumerable<Sucursale>> GetAllAsync()
        => _sucursaleGateway.GetAllAsync();

    public async Task<Sucursale> GetByIdAsync(int id)
        => await _sucursaleGateway.GetByIdAsync(id) ?? throw new NotFoundException($"Succursale introuvable (id {id}).");

    public async Task<int> CreateAsync(CreateSucursaleDto dto)
    {
        var sucursale = new Sucursale
        {
            Nom = dto.Nom,
            Adresse = dto.Adresse,
            Telephone = dto.Telephone,
            Email = dto.Email
        };

        return await _sucursaleGateway.CreateAsync(sucursale);
    }

    public async Task UpdateAsync(int id, UpdateSucursaleDto dto)
    {
        var sucursale = await _sucursaleGateway.GetByIdAsync(id)
            ?? throw new NotFoundException($"Succursale introuvable (id {id}).");

        sucursale.Nom = dto.Nom ?? sucursale.Nom;
        sucursale.Adresse = dto.Adresse ?? sucursale.Adresse;
        sucursale.Telephone = dto.Telephone ?? sucursale.Telephone;
        sucursale.Email = dto.Email ?? sucursale.Email;

        await _sucursaleGateway.UpdateAsync(sucursale);
    }

    public async Task DeleteAsync(int id)
    {
        if (await _sucursaleGateway.GetByIdAsync(id) is null)
            throw new NotFoundException($"Succursale introuvable (id {id}).");

        // Interdiction de suppression si du personnel (médecin/secrétaire) y est affecté.
        if (await _sucursaleGateway.HasPersonnelAsync(id))
            throw new SucursaleHasPersonnelException(id);

        await _sucursaleGateway.DeleteAsync(id);
    }
}
