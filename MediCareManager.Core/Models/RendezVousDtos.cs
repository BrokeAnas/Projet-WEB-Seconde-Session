using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.Models;

public record CreateRendezVousDto(
    [Required] long IdNatPatient,
    [Required] long IdNatMedecin,
    [Required] int IdSucursale,
    [Required] DateOnly DateRdv,
    [Required] TimeOnly HeureDebut,
    [Required] TimeOnly HeureFin,
    string? Motif);
