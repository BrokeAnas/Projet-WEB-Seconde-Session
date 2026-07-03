using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface ISucursaleGateway
{
    Task<IEnumerable<Sucursale>> GetAllAsync();
}
