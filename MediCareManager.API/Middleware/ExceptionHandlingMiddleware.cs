using System.Text.Json;
using MediCareManager.Core.Exceptions;

namespace MediCareManager.API.Middleware;

/// <summary>
/// Intercepte les exceptions métier (Core) et les traduit en codes HTTP cohérents
/// avec un corps JSON uniforme { "error": "..." }.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleAsync(context, ex);
        }
    }

    private async Task HandleAsync(HttpContext context, Exception ex)
    {
        var (status, message) = ex switch
        {
            DomainValidationException        => (StatusCodes.Status400BadRequest, ex.Message),
            NotFoundException                => (StatusCodes.Status404NotFound,   ex.Message),
            RendezVousConflitException       => (StatusCodes.Status409Conflict,   ex.Message),
            MedecinHasRendezVousException    => (StatusCodes.Status409Conflict,   ex.Message),
            SucursaleHasPersonnelException   => (StatusCodes.Status409Conflict,   ex.Message),
            IdNatAlreadyExistsException      => (StatusCodes.Status409Conflict,   ex.Message),
            EmailAlreadyExistsException      => (StatusCodes.Status409Conflict,   ex.Message),
            _                                => (StatusCodes.Status500InternalServerError, "Erreur interne du serveur")
        };

        if (status == StatusCodes.Status500InternalServerError)
            _logger.LogError(ex, "Erreur non gérée");

        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = message }));
    }
}
