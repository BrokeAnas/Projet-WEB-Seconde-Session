using MediCareManager.Core.Exceptions;
using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

public class RendezVousUseCases : IRendezVousUseCases
{
    private static readonly string[] StatutsValides = { "Planifié", "En cours", "Terminé", "Annulé" };

    private readonly IRendezVousGateway _rendezVousGateway;

    public RendezVousUseCases(IRendezVousGateway rendezVousGateway)
    {
        _rendezVousGateway = rendezVousGateway;
    }

    public Task<IEnumerable<RendezVous>> GetAllAsync(long? medecinId, long? patientId, int? sucursaleId, DateOnly? date)
        => _rendezVousGateway.GetAllAsync(medecinId, patientId, sucursaleId, date);

    public async Task<RendezVous> GetByIdAsync(int id)
        => await _rendezVousGateway.GetByIdAsync(id) ?? throw new RendezVousNotFoundException(id);

    public async Task<int> CreateAsync(CreateRendezVousDto dto)
    {
        if (dto.HeureFin <= dto.HeureDebut)
            throw new DomainValidationException("L'heure de fin doit être postérieure à l'heure de début.");

        // RG-02 : détection de conflit d'agenda (intersection d'intervalles)
        if (await _rendezVousGateway.HasConflitAsync(dto.IdNatMedecin, dto.DateRdv, dto.HeureDebut, dto.HeureFin))
            throw new RendezVousConflitException(dto.IdNatMedecin, dto.DateRdv, dto.HeureDebut, dto.HeureFin);

        var rdv = new RendezVous
        {
            IdNatPatient = dto.IdNatPatient,
            IdNatMedecin = dto.IdNatMedecin,
            IdNatSecretaire = dto.IdNatSecretaire,
            IdSucursale = dto.IdSucursale,
            DateRdv = dto.DateRdv,
            HeureDebut = dto.HeureDebut,
            HeureFin = dto.HeureFin,
            Motif = dto.Motif,
            Statut = "Planifié"
        };

        return await _rendezVousGateway.CreateAsync(rdv);
    }

    public async Task UpdateAsync(int id, UpdateRendezVousDto dto)
    {
        var rdv = await _rendezVousGateway.GetByIdAsync(id) ?? throw new RendezVousNotFoundException(id);

        rdv.DateRdv = dto.DateRdv ?? rdv.DateRdv;
        rdv.HeureDebut = dto.HeureDebut ?? rdv.HeureDebut;
        rdv.HeureFin = dto.HeureFin ?? rdv.HeureFin;
        rdv.Motif = dto.Motif ?? rdv.Motif;

        if (!string.IsNullOrWhiteSpace(dto.Statut))
        {
            if (!StatutsValides.Contains(dto.Statut))
                throw new DomainValidationException($"Statut invalide : « {dto.Statut} ».");
            rdv.Statut = dto.Statut;
        }

        if (rdv.HeureFin <= rdv.HeureDebut)
            throw new DomainValidationException("L'heure de fin doit être postérieure à l'heure de début.");

        // Vérifie le conflit en excluant le rendez-vous lui-même.
        if (await _rendezVousGateway.HasConflitAsync(rdv.IdNatMedecin, rdv.DateRdv, rdv.HeureDebut, rdv.HeureFin, id))
            throw new RendezVousConflitException(rdv.IdNatMedecin, rdv.DateRdv, rdv.HeureDebut, rdv.HeureFin);

        await _rendezVousGateway.UpdateAsync(rdv);
    }

    public async Task UpdateStatutAsync(int id, string statut)
    {
        if (!StatutsValides.Contains(statut))
            throw new DomainValidationException($"Statut invalide : « {statut} ».");

        var rdv = await _rendezVousGateway.GetByIdAsync(id) ?? throw new RendezVousNotFoundException(id);
        rdv.Statut = statut;
        await _rendezVousGateway.UpdateAsync(rdv);
    }

    public async Task DeleteAsync(int id)
    {
        if (await _rendezVousGateway.GetByIdAsync(id) is null)
            throw new RendezVousNotFoundException(id);

        await _rendezVousGateway.DeleteAsync(id);
    }
}
