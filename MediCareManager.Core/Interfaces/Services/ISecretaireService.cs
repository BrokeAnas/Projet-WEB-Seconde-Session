using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Services;

public interface ISecretaireService
{
    Task<IEnumerable<Secretaire>> GetAllAsync();
    Task<Secretaire> GetByIdAsync(long idNat);
    Task<long> CreateAsync(CreateSecretaireDto dto);
    Task UpdateAsync(long idNat, UpdateSecretaireDto dto);
    Task DeleteAsync(long idNat);
}
