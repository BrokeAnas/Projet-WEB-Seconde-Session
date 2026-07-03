using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface IMedecinUseCases
{
    Task<IEnumerable<Medecin>> GetAllAsync();
}
