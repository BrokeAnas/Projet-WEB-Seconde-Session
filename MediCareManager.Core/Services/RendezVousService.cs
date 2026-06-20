using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Exceptions;
using MediCareManager.Core.Interfaces.Repositories;
using MediCareManager.Core.Interfaces.Services;

namespace MediCareManager.Core.Services;

public class RendezVousService : IRendezVousService
{
    private static readonly string[] StatutsValides = { "Planifié", "En cours", "Terminé", "Annulé" };

    private readonly IRendezVousRepository _repository;

    public RendezVousService(IRendezVousRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<RendezVous>> GetAllAsync(long? medecinId, long? patientId, int? sucursaleId, DateOnly? date)
        => _repository.GetAllAsync(medecinId, patientId, sucursaleId, date);

    public async Task<RendezVous> GetByIdAsync(int id)
        => await _repository.GetByIdAsync(id) ?? throw new RendezVousNotFoundException(id);

    public async Task<int> CreateAsync(CreateRendezVousDto dto)
    {
        if (dto.HeureFin <= dto.HeureDebut)
            throw new DomainValidationException("L'heure de fin doit être postérieure à l'heure de début.");

        // RG-02 : détection de conflit d'agenda (intersection d'intervalles)
        if (await _repository.HasConflitAsync(dto.IdNatMedecin, dto.DateRdv, dto.HeureDebut, dto.HeureFin))
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

        return await _repository.CreateAsync(rdv);
    }

    public async Task UpdateAsync(int id, UpdateRendezVousDto dto)
    {
        var rdv = await _repository.GetByIdAsync(id) ?? throw new RendezVousNotFoundException(id);

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
        if (await _repository.HasConflitAsync(rdv.IdNatMedecin, rdv.DateRdv, rdv.HeureDebut, rdv.HeureFin, id))
            throw new RendezVousConflitException(rdv.IdNatMedecin, rdv.DateRdv, rdv.HeureDebut, rdv.HeureFin);

        await _repository.UpdateAsync(rdv);
    }

    public async Task UpdateStatutAsync(int id, string statut)
    {
        if (!StatutsValides.Contains(statut))
            throw new DomainValidationException($"Statut invalide : « {statut} ».");

        var rdv = await _repository.GetByIdAsync(id) ?? throw new RendezVousNotFoundException(id);
        rdv.Statut = statut;
        await _repository.UpdateAsync(rdv);
    }

    public async Task DeleteAsync(int id)
    {
        if (await _repository.GetByIdAsync(id) is null)
            throw new RendezVousNotFoundException(id);

        await _repository.DeleteAsync(id);
    }
}
