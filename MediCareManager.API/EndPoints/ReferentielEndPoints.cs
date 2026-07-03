using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

/// <summary>
/// Endpoints des référentiels : spécialisations, assurances et types de maladies.
/// Lecture pour tous les rôles authentifiés, écriture réservée à l'admin.
/// </summary>
public static class ReferentielEndPoints
{
    public static WebApplication AddReferentielEndPoints(this WebApplication app)
    {
        // ----- Spécialisations -----
        var specialisations = app.MapGroup("api/specialisations")
            .RequireAuthorization()
            .WithTags("Specialisations");

        specialisations.MapGet("", async (ISpecialisationUseCases specialisationUseCases) =>
            Results.Ok(await specialisationUseCases.GetAllAsync()));

        specialisations.MapPost("", async (CreateSpecialisationDto dto, ISpecialisationUseCases specialisationUseCases) =>
        {
            var id = await specialisationUseCases.CreateAsync(dto);
            return Results.Json(new { id_specialisation = id, libelle = dto.Libelle }, statusCode: StatusCodes.Status201Created);
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        specialisations.MapDelete("{id:int}", async (int id, ISpecialisationUseCases specialisationUseCases) =>
        {
            await specialisationUseCases.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        // ----- Assurances -----
        var assurances = app.MapGroup("api/assurances")
            .RequireAuthorization()
            .WithTags("Assurances");

        assurances.MapGet("", async (IAssuranceUseCases assuranceUseCases) =>
            Results.Ok(await assuranceUseCases.GetAllAsync()));

        assurances.MapPost("", async (CreateAssuranceDto dto, IAssuranceUseCases assuranceUseCases) =>
        {
            var id = await assuranceUseCases.CreateAsync(dto);
            return Results.Json(new { id_assurance = id, nom = dto.Nom, type = dto.Type }, statusCode: StatusCodes.Status201Created);
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        assurances.MapDelete("{id:int}", async (int id, IAssuranceUseCases assuranceUseCases) =>
        {
            await assuranceUseCases.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        // ----- Types de maladies -----
        var typesMaladies = app.MapGroup("api/typesmaladies")
            .RequireAuthorization()
            .WithTags("TypesMaladies");

        typesMaladies.MapGet("", async (ITypeMaladieUseCases typeMaladieUseCases) =>
            Results.Ok(await typeMaladieUseCases.GetAllAsync()));

        typesMaladies.MapPost("", async (CreateTypeMaladieDto dto, ITypeMaladieUseCases typeMaladieUseCases) =>
        {
            var id = await typeMaladieUseCases.CreateAsync(dto);
            return Results.Json(new { id_maladie = id, libelle = dto.Libelle, code_cim = dto.CodeCIM }, statusCode: StatusCodes.Status201Created);
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        typesMaladies.MapDelete("{id:int}", async (int id, ITypeMaladieUseCases typeMaladieUseCases) =>
        {
            await typeMaladieUseCases.DeleteAsync(id);
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        return app;
    }
}
