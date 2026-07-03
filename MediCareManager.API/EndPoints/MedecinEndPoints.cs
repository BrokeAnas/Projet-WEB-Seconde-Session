using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class MedecinEndPoints
{
    public static WebApplication AddMedecinEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/medecins")
            .RequireAuthorization()
            .WithTags("Medecins");

        // GET /api/medecins — liste en lecture seule (listes déroulantes de l'agenda).
        group.MapGet("", async (IMedecinUseCases medecinUseCases) =>
            Results.Ok(await medecinUseCases.GetAllAsync()));

        return app;
    }
}
