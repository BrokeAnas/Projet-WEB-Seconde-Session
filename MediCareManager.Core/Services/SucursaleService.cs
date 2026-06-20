using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Interfaces.Repositories;
using MediCareManager.Core.Interfaces.Services;

namespace MediCareManager.Core.Services;

public class SucursaleService : ISucursaleService
{
    private readonly ISucursaleRepository _repository;

    public SucursaleService(ISucursaleRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Sucursale>> GetAllAsync()
        => _repository.GetAllAsync();

    public async Task<Sucursale> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id) ?? throw new NotFoundException($"Succursale introuvable (id {id}).");

    public async Task<int> CreateAsync(CreateSucursaleDto dto)
    {
        var sucursale = new Sucursale
        {
            Nom = dto.Nom,
            Adresse = dto.Adresse,
            Telephone = dto.Telephone,
            Email = dto.Email
        };

        return await _repository.CreateAsync(sucursale);
    }

    public async Task UpdateAsync(int id, UpdateSucursaleDto dto)
    {
        var sucursale = await _repository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Succursale introuvable (id {id}).");

        sucursale.Nom = dto.Nom ?? sucursale.Nom;
        sucursale.Adresse = dto.Adresse ?? sucursale.Adresse;
        sucursale.Telephone = dto.Telephone ?? sucursale.Telephone;
        sucursale.Email = dto.Email ?? sucursale.Email;

        await _repository.UpdateAsync(sucursale);
    }

    public async Task DeleteAsync(int id)
    {
        if (await _repository.GetByIdAsync(id) is null)
            throw new NotFoundException($"Succursale introuvable (id {id}).");

        // Interdiction de suppression si du personnel (médecin/secrétaire) y est affecté.
        if (await _repository.HasPersonnelAsync(id))
            throw new SucursaleHasPersonnelException(id);

        await _repository.DeleteAsync(id);
    }
}
