using MediCareManager.Core.Common;
using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Interfaces.Repositories;
using MediCareManager.Core.Interfaces.Services;

namespace MediCareManager.Core.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;

    public PatientService(IPatientRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Patient>> GetAllAsync(string? search, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 20;
        return _repository.GetAllAsync(search, page, pageSize);
    }

    public async Task<Patient> GetByIdAsync(long idNat)
        => await _repository.GetByIdAsync(idNat) ?? throw new PatientNotFoundException(idNat);

    public async Task<long> CreateAsync(CreatePatientDto dto)
    {
        if (!BelgianNationalNumber.IsValid(dto.IdNat))
            throw new DomainValidationException("Numéro de Registre National invalide (contrôle modulo 97).");

        var idNat = BelgianNationalNumber.ToLong(dto.IdNat);

        if (await _repository.ExistsAsync(idNat))
            throw new IdNatAlreadyExistsException(idNat);

        var patient = new Patient
        {
            IdNat = idNat,
            Nom = dto.Nom,
            Prenom = dto.Prenom,
            DateNaissance = dto.DateNaissance,
            Adresse = dto.Adresse,
            Telephone = dto.Telephone,
            Email = dto.Email
        };

        return await _repository.CreateAsync(patient);
    }

    public async Task UpdateAsync(long idNat, UpdatePatientDto dto)
    {
        var patient = await _repository.GetByIdAsync(idNat) ?? throw new PatientNotFoundException(idNat);

        patient.Nom = dto.Nom ?? patient.Nom;
        patient.Prenom = dto.Prenom ?? patient.Prenom;
        patient.DateNaissance = dto.DateNaissance ?? patient.DateNaissance;
        patient.Adresse = dto.Adresse ?? patient.Adresse;
        patient.Telephone = dto.Telephone ?? patient.Telephone;
        patient.Email = dto.Email ?? patient.Email;

        await _repository.UpdateAsync(patient);
    }

    public async Task DeleteAsync(long idNat)
    {
        if (!await _repository.ExistsAsync(idNat))
            throw new PatientNotFoundException(idNat);

        await _repository.DeleteAsync(idNat);
    }

    public async Task<IEnumerable<PatientMaladie>> GetMaladiesAsync(long idNat)
    {
        if (!await _repository.ExistsAsync(idNat))
            throw new PatientNotFoundException(idNat);

        return await _repository.GetMaladiesAsync(idNat);
    }

    public async Task AddMaladieAsync(long idNat, AddMaladieDto dto)
    {
        if (!await _repository.ExistsAsync(idNat))
            throw new PatientNotFoundException(idNat);

        var pm = new PatientMaladie
        {
            IdNatPatient = idNat,
            IdMaladie = dto.IdMaladie,
            DateDiagnostic = dto.DateDiagnostic,
            Observations = dto.Observations
        };

        await _repository.AddMaladieAsync(pm);
    }

    public async Task<IEnumerable<PatientAssurance>> GetAssurancesAsync(long idNat)
    {
        if (!await _repository.ExistsAsync(idNat))
            throw new PatientNotFoundException(idNat);

        return await _repository.GetAssurancesAsync(idNat);
    }

    public async Task AddAssuranceAsync(long idNat, AddAssuranceDto dto)
    {
        if (!await _repository.ExistsAsync(idNat))
            throw new PatientNotFoundException(idNat);

        var pa = new PatientAssurance
        {
            IdNatPatient = idNat,
            IdAssurance = dto.IdAssurance,
            NumeroAffiliation = dto.NumeroAffiliation,
            DateDebut = dto.DateDebut,
            DateFin = dto.DateFin
        };

        await _repository.AddAssuranceAsync(pa);
    }

    public async Task RemoveAssuranceAsync(long idNat, int idAssurance)
    {
        if (!await _repository.ExistsAsync(idNat))
            throw new PatientNotFoundException(idNat);

        await _repository.RemoveAssuranceAsync(idNat, idAssurance);
    }
}
