using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface ISucursaleRepository
{
    Task<IEnumerable<Sucursale>> GetAllAsync();
}
