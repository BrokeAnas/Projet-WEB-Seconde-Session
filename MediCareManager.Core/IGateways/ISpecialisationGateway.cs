using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface ISpecialisationGateway
{
    Task<IEnumerable<SpecialisationMedecin>> GetAllAsync();
    Task<int> CreateAsync(SpecialisationMedecin spec);
    Task DeleteAsync(int id);
}
