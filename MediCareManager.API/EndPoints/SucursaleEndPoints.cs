using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class SucursaleEndPoints
{
    public static WebApplication AddSucursaleEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/sucursales")
            .RequireAuthorization()
            .WithTags("Sucursales");

        // GET /api/sucursales  (tous rôles)
        group.MapGet("", async (ISucursaleUseCases sucursaleUseCases) =>
            Results.Ok(await sucursaleUseCases.GetAllAsync()));

        // GET /api/sucursales/{id}  (tous rôles)
        group.MapGet("{id:int}", async (int id, ISucursaleUseCases sucursaleUseCases) =>
            Results.Ok(await sucursaleUseCases.GetByIdAsync(id)));

        // POST /api/sucursales  (admin)
        group.MapPost("", async (CreateSucursaleDto dto, ISucursaleUseCases sucursaleUseCases) =>
        {
            var id = await sucursaleUseCases.CreateAsync(dto);
            var sucursale = await sucursaleUseCases.GetByIdAsync(id);
            return Results.Created($"/api/sucursales/{id}", sucursale);
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        // PUT /api/sucursales/{id}  (admin)
        group.MapPut("{id:int}", async (int id, UpdateSucursaleDto dto, ISucursaleUseCases sucursaleUseCases) =>
        {
            await sucursaleUseCases.UpdateAsync(id, dto);
            return Results.Ok(await sucursaleUseCases.GetByIdAsync(id));
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        // DELETE /api/sucursales/{id}  (admin) -> 409 si personnel affecté
        group.MapDelete("{id:int}", async (int id, ISucursaleUseCases sucursaleUseCases) =>
        {
            await sucursaleUseCases.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        return app;
    }
}
