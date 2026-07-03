using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface IAdministrateurGateway
{
    Task<Administrateur?> GetByEmailAsync(string email);
}
