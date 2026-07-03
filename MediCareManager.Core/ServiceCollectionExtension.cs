using MediCareManager.Core.UseCases;
using MediCareManager.Core.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace MediCareManager.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthUseCases, AuthUseCases>();
        services.AddScoped<IPatientUseCases, PatientUseCases>();
        services.AddScoped<IMedecinUseCases, MedecinUseCases>();
        services.AddScoped<ISecretaireUseCases, SecretaireUseCases>();
        services.AddScoped<IRendezVousUseCases, RendezVousUseCases>();
        services.AddScoped<IPaiementUseCases, PaiementUseCases>();
        services.AddScoped<ISucursaleUseCases, SucursaleUseCases>();
        services.AddScoped<ISpecialisationUseCases, SpecialisationUseCases>();
        services.AddScoped<IAssuranceUseCases, AssuranceUseCases>();
        services.AddScoped<ITypeMaladieUseCases, TypeMaladieUseCases>();
        services.AddScoped<IAdminUseCases, AdminUseCases>();

        return services;
    }
}
