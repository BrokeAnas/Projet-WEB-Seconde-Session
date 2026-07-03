using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface ISucursaleUseCases
{
    Task<IEnumerable<Sucursale>> GetAllAsync();
}
