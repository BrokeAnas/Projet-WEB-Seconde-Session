using System.Security.Claims;
using MediCareManager.API.Models;
using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class RendezVousEndPoints
{
    private static string? GetSub(ClaimsPrincipal user) => user.FindFirst("sub")?.Value;

    private static int? GetSucursaleClaim(ClaimsPrincipal user)
        => int.TryParse(user.FindFirst("sucursale")?.Value, out var s) ? s : null;

    public static WebApplication AddRendezVousEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/rendezvous")
            .RequireAuthorization()
            .WithTags("RendezVous");

        // GET /api/rendezvous?medecinId=&patientId=&sucursaleId=&date=  (tous rôles)
        group.MapGet("", async (
            IRendezVousUseCases rendezVousUseCases,
            ClaimsPrincipal user,
            long? medecinId,
            long? patientId,
            int? sucursaleId,
            DateOnly? date) =>
        {
            // RG-04 : une secrétaire ne voit que la succursale à laquelle elle est rattachée.
            if (user.IsInRole("secretaire") && !user.IsInRole("admin"))
            {
                var sucursale = GetSucursaleClaim(user);
                if (sucursale.HasValue) sucursaleId = sucursale.Value;
            }

            return Results.Ok(await rendezVousUseCases.GetAllAsync(medecinId, patientId, sucursaleId, date));
        });

        // GET /api/rendezvous/{id}  (tous rôles)
        group.MapGet("{id:int}", async (int id, IRendezVousUseCases rendezVousUseCases) =>
            Results.Ok(await rendezVousUseCases.GetByIdAsync(id)));

        // POST /api/rendezvous  (secrétaire, admin) -> 201 ou 409 si conflit
        group.MapPost("", async (CreateRendezVousDto dto, IRendezVousUseCases rendezVousUseCases) =>
        {
            var id = await rendezVousUseCases.CreateAsync(dto);
            var rdv = await rendezVousUseCases.GetByIdAsync(id);
            return Results.Created($"/api/rendezvous/{id}", rdv);
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        // PUT /api/rendezvous/{id}  (secrétaire, admin)
        group.MapPut("{id:int}", async (int id, UpdateRendezVousDto dto, IRendezVousUseCases rendezVousUseCases) =>
        {
            await rendezVousUseCases.UpdateAsync(id, dto);
            return Results.Ok(await rendezVousUseCases.GetByIdAsync(id));
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        // PATCH /api/rendezvous/{id}/statut  (secrétaire, médecin, admin)
        group.MapPatch("{id:int}/statut", async (int id, UpdateStatutDto dto, ClaimsPrincipal user, IRendezVousUseCases rendezVousUseCases) =>
        {
            // RG-12 : un médecin (qui n'est ni admin ni secrétaire) ne peut modifier que SES propres rendez-vous.
            if (user.IsInRole("medecin") && !user.IsInRole("admin") && !user.IsInRole("secretaire"))
            {
                var rdv = await rendezVousUseCases.GetByIdAsync(id);
                if (rdv.IdNatMedecin.ToString() != GetSub(user))
                    return Results.Forbid();
            }

            await rendezVousUseCases.UpdateStatutAsync(id, dto.Statut);
            return Results.Ok(await rendezVousUseCases.GetByIdAsync(id));
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "medecin", "admin"));

        // DELETE /api/rendezvous/{id}  (secrétaire, admin)
        group.MapDelete("{id:int}", async (int id, IRendezVousUseCases rendezVousUseCases) =>
        {
            await rendezVousUseCases.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        return app;
    }
}
