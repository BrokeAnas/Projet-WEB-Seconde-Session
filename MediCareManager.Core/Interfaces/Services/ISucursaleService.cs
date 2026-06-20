using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Services;

public interface ISucursaleService
{
    Task<IEnumerable<Sucursale>> GetAllAsync();
    Task<Sucursale> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateSucursaleDto dto);
    Task UpdateAsync(int id, UpdateSucursaleDto dto);
    Task DeleteAsync(int id); // Lève SucursaleHasPersonnelException si personnel affecté
}
