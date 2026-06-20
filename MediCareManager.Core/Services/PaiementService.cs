using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Interfaces.Repositories;
using MediCareManager.Core.Interfaces.Services;

namespace MediCareManager.Core.Services;

public class PaiementService : IPaiementService
{
    private readonly IPaiementRepository _repository;

    public PaiementService(IPaiementRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Paiement>> GetAllAsync(long? patientId, DateOnly? dateDebut, DateOnly? dateFin)
        => _repository.GetAllAsync(patientId, dateDebut, dateFin);

    public async Task<Paiement> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id) ?? throw new PaiementNotFoundException(id);

    public async Task<int> CreateAsync(CreatePaiementDto dto)
    {
        var paiement = new Paiement
        {
            IdNatPatient = dto.IdNatPatient,
            IdRdv = dto.IdRdv,
            Montant = dto.Montant,
            DatePaiement = dto.DatePaiement,
            ModePaiement = dto.ModePaiement
        };

        return await _repository.CreateAsync(paiement);
    }

    public async Task UpdateAsync(int id, UpdatePaiementDto dto)
    {
        var paiement = await _repository.GetByIdAsync(id) ?? throw new PaiementNotFoundException(id);

        paiement.Montant = dto.Montant ?? paiement.Montant;
        paiement.DatePaiement = dto.DatePaiement ?? paiement.DatePaiement;
        paiement.ModePaiement = dto.ModePaiement ?? paiement.ModePaiement;

        // RG-03 : le trigger Paiement_Update_Log s'active automatiquement en BDD.
        await _repository.UpdateAsync(paiement);
    }

    public async Task DeleteAsync(int id)
    {
        if (await _repository.GetByIdAsync(id) is null)
            throw new PaiementNotFoundException(id);

        // RG-03 : le trigger Paiement_Delete_Log s'active automatiquement en BDD.
        await _repository.DeleteAsync(id);
    }

    public Task<IEnumerable<PaiementHistorique>> GetAuditLogAsync()
        => _repository.GetAuditLogAsync();
}
