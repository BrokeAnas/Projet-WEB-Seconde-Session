using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Paiement » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class PaiementGateway : IPaiementGateway
{
    private readonly IPaiementRepository _paiementRepository;

    public PaiementGateway(IPaiementRepository paiementRepository)
    {
        _paiementRepository = paiementRepository ?? throw new ArgumentNullException(nameof(paiementRepository));
    }

    public Task<IEnumerable<Paiement>> GetAllAsync(long? patientId = null, DateOnly? dateDebut = null, DateOnly? dateFin = null)
        => _paiementRepository.GetAllAsync(patientId, dateDebut, dateFin);

    public Task<Paiement?> GetByIdAsync(int id)
        => _paiementRepository.GetByIdAsync(id);

    public Task<int> CreateAsync(Paiement paiement)
        => _paiementRepository.CreateAsync(paiement);

    public Task UpdateAsync(Paiement paiement)
        => _paiementRepository.UpdateAsync(paiement);

    public Task DeleteAsync(int id)
        => _paiementRepository.DeleteAsync(id);

    public Task<IEnumerable<PaiementHistorique>> GetAuditLogAsync()
        => _paiementRepository.GetAuditLogAsync();
}
