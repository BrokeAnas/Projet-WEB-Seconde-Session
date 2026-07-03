using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Administrateur » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class AdministrateurGateway : IAdministrateurGateway
{
    private readonly IAdministrateurRepository _administrateurRepository;

    public AdministrateurGateway(IAdministrateurRepository administrateurRepository)
    {
        _administrateurRepository = administrateurRepository ?? throw new ArgumentNullException(nameof(administrateurRepository));
    }

    public Task<Administrateur?> GetByEmailAsync(string email)
        => _administrateurRepository.GetByEmailAsync(email);
}
