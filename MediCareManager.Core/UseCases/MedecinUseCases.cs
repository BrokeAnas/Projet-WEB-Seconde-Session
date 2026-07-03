using MediCareManager.Core.Common;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Core.Security;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

public class MedecinUseCases : IMedecinUseCases
{
    private readonly IMedecinGateway _medecinGateway;
    private readonly IPasswordHasher _passwordHasher;

    public MedecinUseCases(IMedecinGateway medecinGateway, IPasswordHasher passwordHasher)
    {
        _medecinGateway = medecinGateway;
        _passwordHasher = passwordHasher;
    }

    public Task<IEnumerable<Medecin>> GetAllAsync(string? search)
        => _medecinGateway.GetAllAsync(search);

    public async Task<Medecin> GetByIdAsync(long idNat)
        => await _medecinGateway.GetByIdAsync(idNat) ?? throw new MedecinNotFoundException(idNat);

    public async Task<long> CreateAsync(CreateMedecinDto dto)
    {
        if (!BelgianNationalNumber.IsValid(dto.IdNat))
            throw new DomainValidationException("Numéro de Registre National invalide (contrôle modulo 97).");

        if (dto.MotDePasse.Length < 8)
            throw new DomainValidationException("Le mot de passe doit contenir au moins 8 caractères.");

        var idNat = BelgianNationalNumber.ToLong(dto.IdNat);

        if (await _medecinGateway.GetByIdAsync(idNat) is not null)
            throw new IdNatAlreadyExistsException(idNat);

        if (await _medecinGateway.GetByEmailAsync(dto.Email) is not null)
            throw new EmailAlreadyExistsException(dto.Email);

        var medecin = new Medecin
        {
            IdNat = idNat,
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            Email = dto.Email,
            MotDePasse = _passwordHasher.Hash(dto.MotDePasse),
            IdSpecialisation = dto.IdSpecialisation,
            IdSucursale = dto.IdSucursale
        };

        return await _medecinGateway.CreateAsync(medecin);
    }

    public async Task UpdateAsync(long idNat, UpdateMedecinDto dto)
    {
        var medecin = await _medecinGateway.GetByIdAsync(idNat) ?? throw new MedecinNotFoundException(idNat);

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            !dto.Email.Equals(medecin.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _medecinGateway.GetByEmailAsync(dto.Email);
            if (existing is not null && existing.IdNat != idNat)
                throw new EmailAlreadyExistsException(dto.Email);
            medecin.Email = dto.Email;
        }

        medecin.Nom = dto.Nom ?? medecin.Nom;
        medecin.Prenom = dto.Prenom ?? medecin.Prenom;
        medecin.IdSpecialisation = dto.IdSpecialisation ?? medecin.IdSpecialisation;
        medecin.IdSucursale = dto.IdSucursale ?? medecin.IdSucursale;

        await _medecinGateway.UpdateAsync(medecin);
    }

    public async Task DeleteAsync(long idNat)
    {
        if (await _medecinGateway.GetByIdAsync(idNat) is null)
            throw new MedecinNotFoundException(idNat);

        // RG-06 : interdiction de suppression si rendez-vous futurs planifiés.
        if (await _medecinGateway.HasRendezVousFutursAsync(idNat))
            throw new MedecinHasRendezVousException(idNat);

        await _medecinGateway.DeleteAsync(idNat);
    }
}
