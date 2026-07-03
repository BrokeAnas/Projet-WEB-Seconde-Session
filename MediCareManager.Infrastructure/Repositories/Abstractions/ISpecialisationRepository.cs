using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface ISpecialisationRepository
{
    Task<IEnumerable<SpecialisationMedecin>> GetAllAsync();
    Task<int> CreateAsync(SpecialisationMedecin spec);
    Task DeleteAsync(int id);
}
