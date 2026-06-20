using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface ITypeMaladieRepository
{
    Task<IEnumerable<TypeMaladie>> GetAllAsync();
    Task<int> CreateAsync(TypeMaladie type);
    Task DeleteAsync(int id);
}
