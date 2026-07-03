using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface IAssuranceGateway
{
    Task<IEnumerable<Assurance>> GetAllAsync();
    Task<int> CreateAsync(Assurance assurance);
    Task DeleteAsync(int id);
}
