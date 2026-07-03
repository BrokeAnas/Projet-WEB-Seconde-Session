using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface IAdministrateurRepository
{
    Task<Administrateur?> GetByEmailAsync(string email);
}
