using MediCareManager.Core.IGateways;
using MediCareManager.Core.Security;
using MediCareManager.Infrastructure.Gateways;
using MediCareManager.Infrastructure.Repositories;
using MediCareManager.Infrastructure.Repositories.Abstractions;
using MediCareManager.Infrastructure.Security;
using Microsoft.Extensions.DependencyInjection;

namespace MediCareManager.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        // ----- Repositories (Dapper / MySQL) -----
        services.AddScoped<IPatientRepository>(_ => new PatientRepository(connectionString));
        services.AddScoped<IMedecinRepository>(_ => new MedecinRepository(connectionString));
        services.AddScoped<IAdministrateurRepository>(_ => new AdministrateurRepository(connectionString));
        services.AddScoped<IRendezVousRepository>(_ => new RendezVousRepository(connectionString));
        services.AddScoped<ISucursaleRepository>(_ => new SucursaleRepository(connectionString));

        // ----- Gateways (implémentations des IGateways du Core) -----
        services.AddScoped<IPatientGateway, PatientGateway>();
        services.AddScoped<IMedecinGateway, MedecinGateway>();
        services.AddScoped<IAdministrateurGateway, AdministrateurGateway>();
        services.AddScoped<IRendezVousGateway, RendezVousGateway>();
        services.AddScoped<ISucursaleGateway, SucursaleGateway>();

        // ----- Sécurité -----
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        return services;
    }
}
