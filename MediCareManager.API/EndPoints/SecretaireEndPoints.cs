using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class SecretaireEndPoints
{
    public static WebApplication AddSecretaireEndPoints(this WebApplication app)
    {
        // Tout le groupe est réservé au rôle admin.
        var group = app.MapGroup("api/secretaires")
            .RequireAuthorization(policy => policy.RequireRole("admin"))
            .WithTags("Secretaires");

        // GET /api/secretaires  (admin)
        group.MapGet("", async (ISecretaireUseCases secretaireUseCases) =>
            Results.Ok(await secretaireUseCases.GetAllAsync()));

        // GET /api/secretaires/{idNat}  (admin)
        group.MapGet("{idNat:long}", async (long idNat, ISecretaireUseCases secretaireUseCases) =>
            Results.Ok(await secretaireUseCases.GetByIdAsync(idNat)));

        // POST /api/secretaires  (admin)
        group.MapPost("", async (CreateSecretaireDto dto, ISecretaireUseCases secretaireUseCases) =>
        {
            var idNat = await secretaireUseCases.CreateAsync(dto);
            var secretaire = await secretaireUseCases.GetByIdAsync(idNat);
            return Results.Created($"/api/secretaires/{idNat}", secretaire);
        });

        // PUT /api/secretaires/{idNat}  (admin)
        group.MapPut("{idNat:long}", async (long idNat, UpdateSecretaireDto dto, ISecretaireUseCases secretaireUseCases) =>
        {
            await secretaireUseCases.UpdateAsync(idNat, dto);
            return Results.Ok(await secretaireUseCases.GetByIdAsync(idNat));
        });

        // DELETE /api/secretaires/{idNat}  (admin)
        group.MapDelete("{idNat:long}", async (long idNat, ISecretaireUseCases secretaireUseCases) =>
        {
            await secretaireUseCases.DeleteAsync(idNat);
            return Results.NoContent();
        });

        return app;
    }
}
