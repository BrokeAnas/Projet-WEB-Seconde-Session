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

        // GET /api/patients?search=&page=&pageSize=
        group.MapGet("", async (IPatientUseCases patientUseCases, string? search, int page = 1, int pageSize = 20) =>
            Results.Ok(await patientUseCases.GetAllAsync(search, page, pageSize)));

        // GET /api/patients/{idNat}
        group.MapGet("{idNat:long}", async (long idNat, IPatientUseCases patientUseCases) =>
            Results.Ok(await patientUseCases.GetByIdAsync(idNat)));

        // POST /api/patients
        group.MapPost("", async (CreatePatientDto dto, IPatientUseCases patientUseCases) =>
        {
            var idNat = await patientUseCases.CreateAsync(dto);
            var patient = await patientUseCases.GetByIdAsync(idNat);
            return Results.Created($"/api/patients/{idNat}", patient);
        });

        // PUT /api/patients/{idNat}
        group.MapPut("{idNat:long}", async (long idNat, UpdatePatientDto dto, IPatientUseCases patientUseCases) =>
        {
            await patientUseCases.UpdateAsync(idNat, dto);
            return Results.Ok(await patientUseCases.GetByIdAsync(idNat));
        });

        // DELETE /api/patients/{idNat}
        group.MapDelete("{idNat:long}", async (long idNat, IPatientUseCases patientUseCases) =>
        {
            await patientUseCases.DeleteAsync(idNat);
            return Results.NoContent();
        });

        return app;
    }
}
