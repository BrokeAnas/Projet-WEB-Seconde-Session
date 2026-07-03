using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class PatientEndPoints
{
    public static WebApplication AddPatientEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/patients")
            .RequireAuthorization()
            .WithTags("Patients");

        // GET /api/patients?search=&page=&pageSize=  (tous rôles)
        group.MapGet("", async (IPatientUseCases patientUseCases, string? search, int page = 1, int pageSize = 20) =>
            Results.Ok(await patientUseCases.GetAllAsync(search, page, pageSize)));

        // GET /api/patients/{idNat}  (tous rôles)
        group.MapGet("{idNat:long}", async (long idNat, IPatientUseCases patientUseCases) =>
            Results.Ok(await patientUseCases.GetByIdAsync(idNat)));

        // POST /api/patients  (secrétaire, admin)
        group.MapPost("", async (CreatePatientDto dto, IPatientUseCases patientUseCases) =>
        {
            var idNat = await patientUseCases.CreateAsync(dto);
            var patient = await patientUseCases.GetByIdAsync(idNat);
            return Results.Created($"/api/patients/{idNat}", patient);
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        // PUT /api/patients/{idNat}  (secrétaire, admin)
        group.MapPut("{idNat:long}", async (long idNat, UpdatePatientDto dto, IPatientUseCases patientUseCases) =>
        {
            await patientUseCases.UpdateAsync(idNat, dto);
            return Results.Ok(await patientUseCases.GetByIdAsync(idNat));
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        // DELETE /api/patients/{idNat}  (admin)
        group.MapDelete("{idNat:long}", async (long idNat, IPatientUseCases patientUseCases) =>
        {
            await patientUseCases.DeleteAsync(idNat);
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("admin"));

        // ----- Dossier médical (secret médical : médecin/admin uniquement) -----

        // GET /api/patients/{idNat}/maladies  (médecin, admin)
        group.MapGet("{idNat:long}/maladies", async (long idNat, IPatientUseCases patientUseCases) =>
            Results.Ok(await patientUseCases.GetMaladiesAsync(idNat)))
        .RequireAuthorization(policy => policy.RequireRole("medecin", "admin"));

        // POST /api/patients/{idNat}/maladies  (médecin, admin)
        group.MapPost("{idNat:long}/maladies", async (long idNat, AddMaladieDto dto, IPatientUseCases patientUseCases) =>
        {
            await patientUseCases.AddMaladieAsync(idNat, dto);
            return Results.Json(await patientUseCases.GetMaladiesAsync(idNat), statusCode: StatusCodes.Status201Created);
        })
        .RequireAuthorization(policy => policy.RequireRole("medecin", "admin"));

        // ----- Assurances -----

        // GET /api/patients/{idNat}/assurances  (tous rôles)
        group.MapGet("{idNat:long}/assurances", async (long idNat, IPatientUseCases patientUseCases) =>
            Results.Ok(await patientUseCases.GetAssurancesAsync(idNat)));

        // POST /api/patients/{idNat}/assurances  (secrétaire, admin)
        group.MapPost("{idNat:long}/assurances", async (long idNat, AddAssuranceDto dto, IPatientUseCases patientUseCases) =>
        {
            await patientUseCases.AddAssuranceAsync(idNat, dto);
            return Results.Json(await patientUseCases.GetAssurancesAsync(idNat), statusCode: StatusCodes.Status201Created);
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        // DELETE /api/patients/{idNat}/assurances/{idAssurance}  (secrétaire, admin)
        group.MapDelete("{idNat:long}/assurances/{idAssurance:int}", async (long idNat, int idAssurance, IPatientUseCases patientUseCases) =>
        {
            await patientUseCases.RemoveAssuranceAsync(idNat, idAssurance);
            return Results.NoContent();
        })
        .RequireAuthorization(policy => policy.RequireRole("secretaire", "admin"));

        return app;
    }
}
