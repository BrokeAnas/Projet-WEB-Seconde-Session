using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Services;

public interface IPatientService
{
    Task<IEnumerable<Patient>> GetAllAsync(string? search, int page, int pageSize);
    Task<Patient> GetByIdAsync(long idNat);
    Task<long> CreateAsync(CreatePatientDto dto);
    Task UpdateAsync(long idNat, UpdatePatientDto dto);
    Task DeleteAsync(long idNat);
    Task<IEnumerable<PatientMaladie>> GetMaladiesAsync(long idNat);
    Task AddMaladieAsync(long idNat, AddMaladieDto dto);
    Task<IEnumerable<PatientAssurance>> GetAssurancesAsync(long idNat);
    Task AddAssuranceAsync(long idNat, AddAssuranceDto dto);
    Task RemoveAssuranceAsync(long idNat, int idAssurance);
}
