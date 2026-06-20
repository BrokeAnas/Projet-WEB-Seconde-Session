using MediCareManager.Core.Common;
using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Interfaces.Repositories;
using MediCareManager.Core.Interfaces.Services;

namespace MediCareManager.Core.Services;

public class MedecinService : IMedecinService
{
    private readonly IMedecinRepository _repository;
    private readonly IPasswordHasher _passwordHasher;

    public MedecinService(IMedecinRepository repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    public Task<IEnumerable<Medecin>> GetAllAsync(string? search)
        => _repository.GetAllAsync(search);

    public async Task<Medecin> GetByIdAsync(long idNat)
        => await _repository.GetByIdAsync(idNat) ?? throw new MedecinNotFoundException(idNat);

    public async Task<long> CreateAsync(CreateMedecinDto dto)
    {
        if (!BelgianNationalNumber.IsValid(dto.IdNat))
            throw new DomainValidationException("Numéro de Registre National invalide (contrôle modulo 97).");

        var idNat = BelgianNationalNumber.ToLong(dto.IdNat);

        if (await _repository.GetByIdAsync(idNat) is not null)
            throw new IdNatAlreadyExistsException(idNat);

        if (await _repository.GetByEmailAsync(dto.Email) is not null)
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

        return await _repository.CreateAsync(medecin);
    }

    public async Task UpdateAsync(long idNat, UpdateMedecinDto dto)
    {
        var medecin = await _repository.GetByIdAsync(idNat) ?? throw new MedecinNotFoundException(idNat);

        if (!string.IsNullOrWhiteSpace(dto.Email) &&
            !dto.Email.Equals(medecin.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _repository.GetByEmailAsync(dto.Email);
            if (existing is not null && existing.IdNat != idNat)
                throw new EmailAlreadyExistsException(dto.Email);
            medecin.Email = dto.Email;
        }

        medecin.Nom = dto.Nom ?? medecin.Nom;
        medecin.Prenom = dto.Prenom ?? medecin.Prenom;
        medecin.IdSpecialisation = dto.IdSpecialisation ?? medecin.IdSpecialisation;
        medecin.IdSucursale = dto.IdSucursale ?? medecin.IdSucursale;

        await _repository.UpdateAsync(medecin);
    }

    public async Task DeleteAsync(long idNat)
    {
        if (await _repository.GetByIdAsync(idNat) is null)
            throw new MedecinNotFoundException(idNat);

        // RG-06 : interdiction de suppression si rendez-vous futurs planifiés.
        if (await _repository.HasRendezVousFutursAsync(idNat))
            throw new MedecinHasRendezVousException(idNat);

        await _repository.DeleteAsync(idNat);
    }
}
