using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface IPatientUseCases
{
    Task<IEnumerable<Patient>> GetAllAsync(string? search, int page, int pageSize);
    Task<Patient> GetByIdAsync(long idNat);
    Task<long> CreateAsync(CreatePatientDto dto);
    Task UpdateAsync(long idNat, UpdatePatientDto dto);
    Task DeleteAsync(long idNat);
}
