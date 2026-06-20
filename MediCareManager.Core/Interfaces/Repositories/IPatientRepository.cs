using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface IPatientRepository
{
    Task<IEnumerable<Patient>> GetAllAsync(string? search = null, int page = 1, int pageSize = 20);
    Task<Patient?> GetByIdAsync(long idNat);
    Task<long> CreateAsync(Patient patient);
    Task UpdateAsync(Patient patient);
    Task DeleteAsync(long idNat);
    Task<bool> ExistsAsync(long idNat);
    Task<IEnumerable<PatientMaladie>> GetMaladiesAsync(long idNat);
    Task AddMaladieAsync(PatientMaladie pm);
    Task<IEnumerable<PatientAssurance>> GetAssurancesAsync(long idNat);
    Task AddAssuranceAsync(PatientAssurance pa);
    Task RemoveAssuranceAsync(long idNat, int idAssurance);
}
