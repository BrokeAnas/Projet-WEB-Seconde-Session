using MediCareManager.Core.Common;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

public class PatientUseCases : IPatientUseCases
{
    private readonly IPatientGateway _patientGateway;

    public PatientUseCases(IPatientGateway patientGateway)
    {
        _patientGateway = patientGateway;
    }

    public Task<IEnumerable<Patient>> GetAllAsync(string? search, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 20;
        return _patientGateway.GetAllAsync(search, page, pageSize);
    }

    public async Task<Patient> GetByIdAsync(long idNat)
        => await _patientGateway.GetByIdAsync(idNat) ?? throw new PatientNotFoundException(idNat);

    public async Task<long> CreateAsync(CreatePatientDto dto)
    {
        if (!BelgianNationalNumber.IsValid(dto.IdNat))
            throw new DomainValidationException("Numéro de Registre National invalide (contrôle modulo 97).");

        var idNat = BelgianNationalNumber.ToLong(dto.IdNat);

        if (await _patientGateway.ExistsAsync(idNat))
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

        return await _patientGateway.CreateAsync(patient);
    }

    public async Task UpdateAsync(long idNat, UpdatePatientDto dto)
    {
        var patient = await _patientGateway.GetByIdAsync(idNat) ?? throw new PatientNotFoundException(idNat);

        patient.Nom = dto.Nom ?? patient.Nom;
        patient.Prenom = dto.Prenom ?? patient.Prenom;
        patient.DateNaissance = dto.DateNaissance ?? patient.DateNaissance;
        patient.Adresse = dto.Adresse ?? patient.Adresse;
        patient.Telephone = dto.Telephone ?? patient.Telephone;
        patient.Email = dto.Email ?? patient.Email;

        await _patientGateway.UpdateAsync(patient);
    }

    public async Task DeleteAsync(long idNat)
    {
        if (!await _patientGateway.ExistsAsync(idNat))
            throw new PatientNotFoundException(idNat);

        await _patientGateway.DeleteAsync(idNat);
    }
}
