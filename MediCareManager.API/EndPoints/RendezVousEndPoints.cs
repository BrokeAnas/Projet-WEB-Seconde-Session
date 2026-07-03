using MediCareManager.API.Models;
using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class RendezVousEndPoints
{
    public static WebApplication AddRendezVousEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/rendezvous")
            .RequireAuthorization()
            .WithTags("RendezVous");

        // GET /api/rendezvous?medecinId=&date=
        group.MapGet("", async (IRendezVousUseCases rendezVousUseCases, long? medecinId, DateOnly? date) =>
            Results.Ok(await rendezVousUseCases.GetAllAsync(medecinId, date)));

        // GET /api/rendezvous/{id}
        group.MapGet("{id:int}", async (int id, IRendezVousUseCases rendezVousUseCases) =>
            Results.Ok(await rendezVousUseCases.GetByIdAsync(id)));

        // POST /api/rendezvous -> 201 ou 409 si conflit d'agenda
        group.MapPost("", async (CreateRendezVousDto dto, IRendezVousUseCases rendezVousUseCases) =>
        {
            var id = await rendezVousUseCases.CreateAsync(dto);
            var rdv = await rendezVousUseCases.GetByIdAsync(id);
            return Results.Created($"/api/rendezvous/{id}", rdv);
        });

        // PATCH /api/rendezvous/{id}/statut
        group.MapPatch("{id:int}/statut", async (int id, UpdateStatutDto dto, IRendezVousUseCases rendezVousUseCases) =>
        {
            await rendezVousUseCases.UpdateStatutAsync(id, dto.Statut);
            return Results.Ok(await rendezVousUseCases.GetByIdAsync(id));
        });

        return app;
    }
}
