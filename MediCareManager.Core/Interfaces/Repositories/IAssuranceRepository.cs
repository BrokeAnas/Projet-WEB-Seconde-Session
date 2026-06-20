using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface IAssuranceRepository
{
    Task<IEnumerable<Assurance>> GetAllAsync();
    Task<int> CreateAsync(Assurance assurance);
    Task DeleteAsync(int id);
}
