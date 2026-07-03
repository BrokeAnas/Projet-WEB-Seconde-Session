using MediCareManager.Core.Exceptions;
using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

public class PaiementUseCases : IPaiementUseCases
{
    private readonly IPaiementGateway _paiementGateway;

    public PaiementUseCases(IPaiementGateway paiementGateway)
    {
        _paiementGateway = paiementGateway;
    }

    public Task<IEnumerable<Paiement>> GetAllAsync(long? patientId, DateOnly? dateDebut, DateOnly? dateFin)
        => _paiementGateway.GetAllAsync(patientId, dateDebut, dateFin);

    public async Task<Paiement> GetByIdAsync(int id)
        => await _paiementGateway.GetByIdAsync(id) ?? throw new PaiementNotFoundException(id);

    public async Task<int> CreateAsync(CreatePaiementDto dto)
    {
        ValiderMontant(dto.Montant);

        var paiement = new Paiement
        {
            IdNatPatient = dto.IdNatPatient,
            IdRdv = dto.IdRdv,
            Montant = dto.Montant,
            DatePaiement = dto.DatePaiement,
            ModePaiement = dto.ModePaiement
        };

        return await _paiementGateway.CreateAsync(paiement);
    }

    public async Task UpdateAsync(int id, UpdatePaiementDto dto)
    {
        if (dto.Montant.HasValue) ValiderMontant(dto.Montant.Value);

        var paiement = await _paiementGateway.GetByIdAsync(id) ?? throw new PaiementNotFoundException(id);

        paiement.Montant = dto.Montant ?? paiement.Montant;
        paiement.DatePaiement = dto.DatePaiement ?? paiement.DatePaiement;
        paiement.ModePaiement = dto.ModePaiement ?? paiement.ModePaiement;

        // RG-03 : le trigger Paiement_Update_Log s'active automatiquement en BDD.
        await _paiementGateway.UpdateAsync(paiement);
    }

    public async Task DeleteAsync(int id)
    {
        if (await _paiementGateway.GetByIdAsync(id) is null)
            throw new PaiementNotFoundException(id);

        // RG-03 : le trigger Paiement_Delete_Log s'active automatiquement en BDD.
        await _paiementGateway.DeleteAsync(id);
    }

    public Task<IEnumerable<PaiementHistorique>> GetAuditLogAsync()
        => _paiementGateway.GetAuditLogAsync();

    private static void ValiderMontant(decimal montant)
    {
        if (montant is < 0.01m or > 9999.99m)
            throw new DomainValidationException("Le montant doit être compris entre 0,01 et 9 999,99 €.");
    }
}
