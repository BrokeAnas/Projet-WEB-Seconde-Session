using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface IAdministrateurRepository
{
    Task<Administrateur?> GetByEmailAsync(string email);
}
