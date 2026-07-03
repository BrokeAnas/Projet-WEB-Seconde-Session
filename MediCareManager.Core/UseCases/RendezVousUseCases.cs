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

    public Task<IEnumerable<RendezVous>> GetAllAsync(long? medecinId, DateOnly? date)
        => _rendezVousGateway.GetAllAsync(medecinId, date);

    public async Task<RendezVous> GetByIdAsync(int id)
        => await _rendezVousGateway.GetByIdAsync(id) ?? throw new RendezVousNotFoundException(id);

    public async Task<int> CreateAsync(CreateRendezVousDto dto)
    {
        if (dto.HeureFin <= dto.HeureDebut)
            throw new DomainValidationException("L'heure de fin doit être postérieure à l'heure de début.");

        // Règle métier : détection de conflit d'agenda (intersection d'intervalles).
        if (await _rendezVousGateway.HasConflitAsync(dto.IdNatMedecin, dto.DateRdv, dto.HeureDebut, dto.HeureFin))
            throw new RendezVousConflitException(dto.IdNatMedecin, dto.DateRdv, dto.HeureDebut, dto.HeureFin);

        var rdv = new RendezVous
        {
            IdNatPatient = dto.IdNatPatient,
            IdNatMedecin = dto.IdNatMedecin,
            IdSucursale = dto.IdSucursale,
            DateRdv = dto.DateRdv,
            HeureDebut = dto.HeureDebut,
            HeureFin = dto.HeureFin,
            Motif = dto.Motif,
            Statut = "Planifié"
        };

        return await _rendezVousGateway.CreateAsync(rdv);
    }

    public async Task UpdateStatutAsync(int id, string statut)
    {
        if (!StatutsValides.Contains(statut))
            throw new DomainValidationException($"Statut invalide : « {statut} ».");

        if (await _rendezVousGateway.GetByIdAsync(id) is null)
            throw new RendezVousNotFoundException(id);

        await _rendezVousGateway.UpdateStatutAsync(id, statut);
    }
}
