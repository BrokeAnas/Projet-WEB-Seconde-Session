using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface IPatientGateway
{
    Task<IEnumerable<Patient>> GetAllAsync(string? search = null, int page = 1, int pageSize = 20);
    Task<Patient?> GetByIdAsync(long idNat);
    Task<long> CreateAsync(Patient patient);
    Task UpdateAsync(Patient patient);
    Task DeleteAsync(long idNat);
    Task<bool> ExistsAsync(long idNat);
}
