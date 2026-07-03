using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface ISucursaleUseCases
{
    Task<IEnumerable<Sucursale>> GetAllAsync();
    Task<Sucursale> GetByIdAsync(int id);
    Task<int> CreateAsync(CreateSucursaleDto dto);
    Task UpdateAsync(int id, UpdateSucursaleDto dto);
    Task DeleteAsync(int id); // Lève SucursaleHasPersonnelException si personnel affecté
}
