using MediCareManager.Core.Common;
using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Interfaces.Repositories;
using MediCareManager.Core.Interfaces.Services;

namespace MediCareManager.Core.Services;

public class SecretaireService : ISecretaireService
{
    private readonly ISecretaireRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public SecretaireService(ISecretaireRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public Task<IEnumerable<Secretaire>> GetAllAsync()
        => _repository.GetAllAsync();

    public async Task<Secretaire> GetByIdAsync(long idNat)
        => await _repository.GetByIdAsync(idNat)
           ?? throw new NotFoundException($"Secrétaire introuvable (numéro national {idNat}).");

    public async Task<long> CreateAsync(CreateSecretaireDto dto)
    {
        if (!BelgianNationalNumber.IsValid(dto.IdNat))
            throw new DomainValidationException("Numéro de Registre National invalide (contrôle modulo 97).");

        var idNat = BelgianNationalNumber.ToLong(dto.IdNat);

        if (await _repository.GetByIdAsync(idNat) is not null)
            throw new IdNatAlreadyExistsException(idNat);

        if (await _repository.GetByEmailAsync(dto.Email) is not null)
            throw new EmailAlreadyExistsException(dto.Email);

        var secretaire = new Secretaire
        {
            IdNat = idNat,
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Email = dto.Email,
            MotDePasse = _passwordHasher.Hash(dto.MotDePasse),
            IdSucursale = dto.IdSucursale
        };

        return await _repository.CreateAsync(secretaire);
    }

    public async Task UpdateAsync(long idNat, UpdateSecretaireDto dto)
    {
        var secretaire = await _repository.GetByIdAsync(idNat)
            ?? throw new NotFoundException($"Secrétaire introuvable (numéro national {idNat}).");

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            !dto.Email.Equals(secretaire.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _repository.GetByEmailAsync(dto.Email);
            if (existing is not null && existing.IdNat != idNat)
                throw new EmailAlreadyExistsException(dto.Email);
            secretaire.Email = dto.Email;
        }

        secretaire.Nom = dto.Nom ?? secretaire.Nom;
        secretaire.Prenom = dto.Prenom ?? secretaire.Prenom;
        secretaire.IdSucursale = dto.IdSucursale ?? secretaire.IdSucursale;

        await _repository.UpdateAsync(secretaire);
    }

    public async Task DeleteAsync(long idNat)
    {
        if (await _repository.GetByIdAsync(idNat) is null)
            throw new NotFoundException($"Secrétaire introuvable (numéro national {idNat}).");

        await _repository.DeleteAsync(idNat);
    }
}
