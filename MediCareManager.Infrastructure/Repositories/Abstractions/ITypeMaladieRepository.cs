using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface ITypeMaladieRepository
{
    Task<IEnumerable<TypeMaladie>> GetAllAsync();
    Task<int> CreateAsync(TypeMaladie type);
    Task DeleteAsync(int id);
}
