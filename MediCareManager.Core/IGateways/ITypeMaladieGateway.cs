using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface ITypeMaladieGateway
{
    Task<IEnumerable<TypeMaladie>> GetAllAsync();
    Task<int> CreateAsync(TypeMaladie type);
    Task DeleteAsync(int id);
}
