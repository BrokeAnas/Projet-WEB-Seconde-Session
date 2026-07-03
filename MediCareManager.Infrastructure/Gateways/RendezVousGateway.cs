using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Rendez-vous » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class RendezVousGateway : IRendezVousGateway
{
    private readonly IRendezVousRepository _rendezVousRepository;

    public RendezVousGateway(IRendezVousRepository rendezVousRepository)
    {
        _rendezVousRepository = rendezVousRepository ?? throw new ArgumentNullException(nameof(rendezVousRepository));
    }

    public Task<IEnumerable<RendezVous>> GetAllAsync(long? medecinId = null, long? patientId = null, int? sucursaleId = null, DateOnly? date = null)
        => _rendezVousRepository.GetAllAsync(medecinId, patientId, sucursaleId, date);

    public Task<RendezVous?> GetByIdAsync(int id)
        => _rendezVousRepository.GetByIdAsync(id);

    public Task<bool> HasConflitAsync(long medecinId, DateOnly date, TimeOnly heureDebut, TimeOnly heureFin, int? excludeId = null)
        => _rendezVousRepository.HasConflitAsync(medecinId, date, heureDebut, heureFin, excludeId);

    public Task<int> CreateAsync(RendezVous rdv)
        => _rendezVousRepository.CreateAsync(rdv);

    public Task UpdateAsync(RendezVous rdv)
        => _rendezVousRepository.UpdateAsync(rdv);

    public Task DeleteAsync(int id)
        => _rendezVousRepository.DeleteAsync(id);
}
