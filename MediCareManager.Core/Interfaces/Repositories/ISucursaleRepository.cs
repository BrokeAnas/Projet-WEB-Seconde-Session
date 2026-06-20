using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface ISucursaleRepository
{
    Task<IEnumerable<Sucursale>> GetAllAsync();
    Task<Sucursale?> GetByIdAsync(int id);
    Task<int> CreateAsync(Sucursale sucursale);
    Task UpdateAsync(Sucursale sucursale);
    Task DeleteAsync(int id);
    Task<bool> HasPersonnelAsync(int id);
}
