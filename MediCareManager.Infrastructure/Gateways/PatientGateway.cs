using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Patient » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class PatientGateway : IPatientGateway
{
    private readonly IPatientRepository _patientRepository;

    public PatientGateway(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository ?? throw new ArgumentNullException(nameof(patientRepository));
    }

    public Task<IEnumerable<Patient>> GetAllAsync(string? search = null, int page = 1, int pageSize = 20)
        => _patientRepository.GetAllAsync(search, page, pageSize);

    public Task<Patient?> GetByIdAsync(long idNat)
        => _patientRepository.GetByIdAsync(idNat);

    public Task<long> CreateAsync(Patient patient)
        => _patientRepository.CreateAsync(patient);

    public Task UpdateAsync(Patient patient)
        => _patientRepository.UpdateAsync(patient);

    public Task DeleteAsync(long idNat)
        => _patientRepository.DeleteAsync(idNat);

    public Task<bool> ExistsAsync(long idNat)
        => _patientRepository.ExistsAsync(idNat);

    public Task<IEnumerable<PatientMaladie>> GetMaladiesAsync(long idNat)
        => _patientRepository.GetMaladiesAsync(idNat);

    public Task AddMaladieAsync(PatientMaladie pm)
        => _patientRepository.AddMaladieAsync(pm);

    public Task<IEnumerable<PatientAssurance>> GetAssurancesAsync(long idNat)
        => _patientRepository.GetAssurancesAsync(idNat);

    public Task AddAssuranceAsync(PatientAssurance pa)
        => _patientRepository.AddAssuranceAsync(pa);

    public Task RemoveAssuranceAsync(long idNat, int idAssurance)
        => _patientRepository.RemoveAssuranceAsync(idNat, idAssurance);
}
