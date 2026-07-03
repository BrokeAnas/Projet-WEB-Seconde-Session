using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Assurance » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class AssuranceGateway : IAssuranceGateway
{
    private readonly IAssuranceRepository _assuranceRepository;

    public AssuranceGateway(IAssuranceRepository assuranceRepository)
    {
        _assuranceRepository = assuranceRepository ?? throw new ArgumentNullException(nameof(assuranceRepository));
    }

    public Task<IEnumerable<Assurance>> GetAllAsync()
        => _assuranceRepository.GetAllAsync();

    public Task<int> CreateAsync(Assurance assurance)
        => _assuranceRepository.CreateAsync(assurance);

    public Task DeleteAsync(int id)
        => _assuranceRepository.DeleteAsync(id);
}
