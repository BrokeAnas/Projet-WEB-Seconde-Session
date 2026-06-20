using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.DTOs;

public record CreatePaiementDto(
    [Required] long IdNatPatient,
    int? IdRdv,
    [Required, Range(0.01, 9999.99)] decimal Montant,
    [Required] DateOnly DatePaiement,
    string? ModePaiement);

public record UpdatePaiementDto(
    [Range(0.01, 9999.99)] decimal? Montant,
    DateOnly? DatePaiement,
    string? ModePaiement);

public record PaiementResponseDto(
    int IdPaiement,
    long IdNatPatient,
    string PatientNom,
    int? IdRdv,
    decimal Montant,
    DateOnly DatePaiement,
    string? ModePaiement);

public record AuditLogResponseDto(
    int IdHistorique,
    int IdPaiement,
    long IdNatPatient,
    decimal Montant,
    DateOnly DatePaiement,
    string Operation,
    DateTime DateOperation);
