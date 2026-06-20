using System.ComponentModel.DataAnnotations;

namespace MediCareManager.Core.DTOs;

public record LoginDto(
    [Required, EmailAddress] string Email,
    [Required] string Password);

public record AuthResponseDto(string Token, string Role, string Nom, string Prenom);
