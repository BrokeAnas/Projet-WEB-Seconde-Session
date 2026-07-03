using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class PaiementEndPoints
{
    public static WebApplication AddPaiementEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/paiements")
            .RequireAuthorization()
            .WithTags("Paiements");

        // GET /api/paiements?patientId=&dateDebut=&dateFin=  (secrétaire, admin)
        group.MapGet("", async (IPaiementUseCases paiementUseCases, long? patientId, DateOnly? dateDebut, DateOnly? dateFin) =>
            Results.Ok(await paiementUseCases.GetAllAsync(patientId, dateDebut, dateFin)))
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        // GET /api/paiements/audit  (admin) — journal alimenté par les triggers SQL.
        group.MapGet("audit", async (IPaiementUseCases paiementUseCases) =>
            Results.Ok(await paiementUseCases.GetAuditLogAsync()))
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        // GET /api/paiements/{id}  (secrétaire, admin)
        group.MapGet("{id:int}", async (int id, IPaiementUseCases paiementUseCases) =>
            Results.Ok(await paiementUseCases.GetByIdAsync(id)))
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        // POST /api/paiements  (secrétaire, admin) -> 201
        group.MapPost("", async (CreatePaiementDto dto, IPaiementUseCases paiementUseCases) =>
        {
            var id = await paiementUseCases.CreateAsync(dto);
            var paiement = await paiementUseCases.GetByIdAsync(id);
            return Results.Created($"/api/paiements/{id}", paiement);
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        // PUT /api/paiements/{id}  (secrétaire, admin) — déclenche le trigger UPDATE
        group.MapPut("{id:int}", async (int id, UpdatePaiementDto dto, IPaiementUseCases paiementUseCases) =>
        {
            await paiementUseCases.UpdateAsync(id, dto);
            return Results.Ok(await paiementUseCases.GetByIdAsync(id));
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        // DELETE /api/paiements/{id}  (admin) — déclenche le trigger DELETE
        group.MapDelete("{id:int}", async (int id, IPaiementUseCases paiementUseCases) =>
        {
            await paiementUseCases.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        return app;
    }
}
