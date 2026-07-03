using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class MedecinEndPoints
{
    public static WebApplication AddMedecinEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/medecins")
            .RequireAuthorization()
            .WithTags("Medecins");

        // GET /api/medecins?search=  (tous rôles)
        group.MapGet("", async (IMedecinUseCases medecinUseCases, string? search) =>
            Results.Ok(await medecinUseCases.GetAllAsync(search)));

        // GET /api/medecins/{idNat}  (tous rôles)
        group.MapGet("{idNat:long}", async (long idNat, IMedecinUseCases medecinUseCases) =>
            Results.Ok(await medecinUseCases.GetByIdAsync(idNat)));

        // POST /api/medecins  (admin)
        group.MapPost("", async (CreateMedecinDto dto, IMedecinUseCases medecinUseCases) =>
        {
            var idNat = await medecinUseCases.CreateAsync(dto);
            var medecin = await medecinUseCases.GetByIdAsync(idNat);
            return Results.Created($"/api/medecins/{idNat}", medecin);
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        // PUT /api/medecins/{idNat}  (admin)
        group.MapPut("{idNat:long}", async (long idNat, UpdateMedecinDto dto, IMedecinUseCases medecinUseCases) =>
        {
            await medecinUseCases.UpdateAsync(idNat, dto);
            return Results.Ok(await medecinUseCases.GetByIdAsync(idNat));
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        // DELETE /api/medecins/{idNat}  (admin) -> 409 si rendez-vous futurs
        group.MapDelete("{idNat:long}", async (long idNat, IMedecinUseCases medecinUseCases) =>
        {
            await medecinUseCases.DeleteAsync(idNat);
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        return app;
    }
}
