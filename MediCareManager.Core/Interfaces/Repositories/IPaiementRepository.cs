using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface IPaiementRepository
{
    Task<IEnumerable<Paiement>> GetAllAsync(long? patientId = null, DateOnly? dateDebut = null, DateOnly? dateFin = null);
    Task<Paiement?> GetByIdAsync(int id);
    Task<int> CreateAsync(Paiement paiement);
    Task UpdateAsync(Paiement paiement);
    Task DeleteAsync(int id);
    Task<IEnumerable<PaiementHistorique>> GetAuditLogAsync();
}
