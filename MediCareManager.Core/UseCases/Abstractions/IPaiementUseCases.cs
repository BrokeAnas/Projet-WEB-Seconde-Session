using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface IPaiementUseCases
{
    Task<IEnumerable<Paiement>> GetAllAsync(long? patientId, DateOnly? dateDebut, DateOnly? dateFin);
    Task<Paiement> GetByIdAsync(int id);
    Task<int> CreateAsync(CreatePaiementDto dto);
    Task UpdateAsync(int id, UpdatePaiementDto dto); // Trigger UPDATE s'active en BDD
    Task DeleteAsync(int id); // Trigger DELETE s'active en BDD
    Task<IEnumerable<PaiementHistorique>> GetAuditLogAsync();
}
