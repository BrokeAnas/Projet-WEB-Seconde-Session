using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Services;

public interface IPaiementService
{
    Task<IEnumerable<Paiement>> GetAllAsync(long? patientId, DateOnly? dateDebut, DateOnly? dateFin);
    Task<Paiement> GetByIdAsync(int id);
    Task<int> CreateAsync(CreatePaiementDto dto);
    Task UpdateAsync(int id, UpdatePaiementDto dto); // Trigger UPDATE s'active en BDD
    Task DeleteAsync(int id); // Trigger DELETE s'active en BDD
    Task<IEnumerable<PaiementHistorique>> GetAuditLogAsync();
}
