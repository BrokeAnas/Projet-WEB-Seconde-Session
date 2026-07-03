using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Statistiques » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class StatsGateway : IStatsGateway
{
    private readonly IStatsRepository _statsRepository;

    public StatsGateway(IStatsRepository statsRepository)
    {
        _statsRepository = statsRepository ?? throw new ArgumentNullException(nameof(statsRepository));
    }

    public Task<AdminStatsDto> GetStatsAsync()
        => _statsRepository.GetStatsAsync();
}
