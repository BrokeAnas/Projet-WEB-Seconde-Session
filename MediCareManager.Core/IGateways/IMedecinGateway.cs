using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface IMedecinGateway
{
    Task<IEnumerable<Medecin>> GetAllAsync();
}
