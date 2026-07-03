using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface IAssuranceRepository
{
    Task<IEnumerable<Assurance>> GetAllAsync();
    Task<int> CreateAsync(Assurance assurance);
    Task DeleteAsync(int id);
}
