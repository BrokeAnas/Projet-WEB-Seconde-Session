using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Succursale » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class SucursaleGateway : ISucursaleGateway
{
    private readonly ISucursaleRepository _sucursaleRepository;

    public SucursaleGateway(ISucursaleRepository sucursaleRepository)
    {
        _sucursaleRepository = sucursaleRepository ?? throw new ArgumentNullException(nameof(sucursaleRepository));
    }

    public Task<IEnumerable<Sucursale>> GetAllAsync()
        => _sucursaleRepository.GetAllAsync();
}
