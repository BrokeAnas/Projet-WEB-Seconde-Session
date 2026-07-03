using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface IMedecinRepository
{
    Task<IEnumerable<Medecin>> GetAllAsync();
}
