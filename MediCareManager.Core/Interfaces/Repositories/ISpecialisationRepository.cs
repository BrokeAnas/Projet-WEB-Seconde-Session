using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface ISpecialisationRepository
{
    Task<IEnumerable<SpecialisationMedecin>> GetAllAsync();
    Task<int> CreateAsync(SpecialisationMedecin spec);
    Task DeleteAsync(int id);
}
