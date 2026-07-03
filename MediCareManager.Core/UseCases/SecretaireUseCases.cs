using MediCareManager.Core.Common;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Core.Security;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

public class SecretaireUseCases : ISecretaireUseCases
{
    private readonly ISecretaireGateway _secretaireGateway;
    private readonly IPasswordHasher _passwordHasher;

    public SecretaireUseCases(ISecretaireGateway secretaireGateway, IPasswordHasher passwordHasher)
    {
        _secretaireGateway = secretaireGateway;
        _passwordHasher = passwordHasher;
    }

    public Task<IEnumerable<Secretaire>> GetAllAsync()
        => _secretaireGateway.GetAllAsync();

    public async Task<Secretaire> GetByIdAsync(long idNat)
        => await _secretaireGateway.GetByIdAsync(idNat)
           ?? throw new NotFoundException($"Secrétaire introuvable (numéro national {idNat}).");

    public async Task<long> CreateAsync(CreateSecretaireDto dto)
    {
        if (!BelgianNationalNumber.IsValid(dto.IdNat))
            throw new DomainValidationException("Numéro de Registre National invalide (contrôle modulo 97).");

        if (dto.MotDePasse.Length < 8)
            throw new DomainValidationException("Le mot de passe doit contenir au moins 8 caractères.");

        var idNat = BelgianNationalNumber.ToLong(dto.IdNat);

        if (await _secretaireGateway.GetByIdAsync(idNat) is not null)
            throw new IdNatAlreadyExistsException(idNat);

        if (await _secretaireGateway.GetByEmailAsync(dto.Email) is not null)
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

        return await _secretaireGateway.CreateAsync(secretaire);
    }

    public async Task UpdateAsync(long idNat, UpdateSecretaireDto dto)
    {
        var secretaire = await _secretaireGateway.GetByIdAsync(idNat)
            ?? throw new NotFoundException($"Secrétaire introuvable (numéro national {idNat}).");

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            !dto.Email.Equals(secretaire.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _secretaireGateway.GetByEmailAsync(dto.Email);
            if (existing is not null && existing.IdNat != idNat)
                throw new EmailAlreadyExistsException(dto.Email);
            secretaire.Email = dto.Email;
        }

        secretaire.Nom = dto.Nom ?? secretaire.Nom;
        secretaire.Prenom = dto.Prenom ?? secretaire.Prenom;
        secretaire.IdSucursale = dto.IdSucursale ?? secretaire.IdSucursale;

        await _secretaireGateway.UpdateAsync(secretaire);
    }

    public async Task DeleteAsync(long idNat)
    {
        if (await _secretaireGateway.GetByIdAsync(idNat) is null)
            throw new NotFoundException($"Secrétaire introuvable (numéro national {idNat}).");

        await _secretaireGateway.DeleteAsync(idNat);
    }
}
