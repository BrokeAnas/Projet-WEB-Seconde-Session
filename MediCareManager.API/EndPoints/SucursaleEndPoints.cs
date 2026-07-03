using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class SucursaleEndPoints
{
    public static WebApplication AddSucursaleEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/sucursales")
            .RequireAuthorization()
            .WithTags("Sucursales");

        // GET /api/sucursales — liste en lecture seule (listes déroulantes de l'agenda).
        group.MapGet("", async (ISucursaleUseCases sucursaleUseCases) =>
            Results.Ok(await sucursaleUseCases.GetAllAsync()));

        return app;
    }
}
