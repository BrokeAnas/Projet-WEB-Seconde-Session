using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Spécialisation » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class SpecialisationGateway : ISpecialisationGateway
{
    private readonly ISpecialisationRepository _specialisationRepository;

    public SpecialisationGateway(ISpecialisationRepository specialisationRepository)
    {
        _specialisationRepository = specialisationRepository ?? throw new ArgumentNullException(nameof(specialisationRepository));
    }

    public Task<IEnumerable<SpecialisationMedecin>> GetAllAsync()
        => _specialisationRepository.GetAllAsync();

    public Task<int> CreateAsync(SpecialisationMedecin spec)
        => _specialisationRepository.CreateAsync(spec);

    public Task DeleteAsync(int id)
        => _specialisationRepository.DeleteAsync(id);
}
