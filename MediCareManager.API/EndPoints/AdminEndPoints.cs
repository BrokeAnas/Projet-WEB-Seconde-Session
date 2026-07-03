using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.API.EndPoints;

public static class AdminEndPoints
{
    public static WebApplication AddAdminEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("api/admin")
            .RequireAuthorization(policy => policy.RequireRole("admin"))
            .WithTags("Admin");

        // GET /api/admin/stats  (admin)
        group.MapGet("stats", async (IAdminUseCases adminUseCases) =>
            Results.Ok(await adminUseCases.GetStatsAsync()));

        return app;
    }
}
