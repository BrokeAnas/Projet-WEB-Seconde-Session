using System.IdentityModel.Tokens.Jwt;
using MediCareManager.API.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class AuthEndPoints
{
    public static WebApplication AddAuthEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/auth")
            .WithTags("Auth");

        // POST /api/auth/login — renvoie le token JWT ou HTTP 401.
        group.MapPost("login", async (LoginDto dto, IAuthUseCases authUseCases) =>
        {
            var token = await authUseCases.LoginAsync(dto.Email, dto.Password);
            if (token is null)
                return Results.Json(
                    new { error = "Adresse e-mail ou mot de passe incorrect." },
                    statusCode: StatusCodes.Status401Unauthorized);

            // On décode le JWT pour extraire les claims et les renvoyer dans la réponse.
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            string Claim(string type) => jwt.Claims.FirstOrDefault(c => c.Type == type)?.Value ?? string.Empty;

            var response = new AuthResponseDto(
                Token: token,
                Role: Claim("role"),
                Nom: Claim("family_name"),
                Prenom: Claim("given_name"));

            return Results.Ok(response);
        })
        .AllowAnonymous();

        return app;
    }
}
