using System.ComponentModel.DataAnnotations;

namespace MediCareManager.API.Models;

/// <summary>Corps du PATCH /api/rendezvous/{id}/statut.</summary>
public record UpdateStatutDto(
    [Required] string Statut);
