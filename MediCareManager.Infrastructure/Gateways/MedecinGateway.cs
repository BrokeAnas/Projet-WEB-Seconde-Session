using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Médecin » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class MedecinGateway : IMedecinGateway
{
    private readonly IMedecinRepository _medecinRepository;

    public MedecinGateway(IMedecinRepository medecinRepository)
    {
        _medecinRepository = medecinRepository ?? throw new ArgumentNullException(nameof(medecinRepository));
    }

    public Task<IEnumerable<Medecin>> GetAllAsync()
        => _medecinRepository.GetAllAsync();
}
