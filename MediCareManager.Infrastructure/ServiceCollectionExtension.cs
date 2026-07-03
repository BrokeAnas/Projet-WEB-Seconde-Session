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
        services.AddScoped<ISecretaireRepository>(_ => new SecretaireRepository(connectionString));
        services.AddScoped<IAdministrateurRepository>(_ => new AdministrateurRepository(connectionString));
        services.AddScoped<IRendezVousRepository>(_ => new RendezVousRepository(connectionString));
        services.AddScoped<IPaiementRepository>(_ => new PaiementRepository(connectionString));
        services.AddScoped<ISucursaleRepository>(_ => new SucursaleRepository(connectionString));
        services.AddScoped<ISpecialisationRepository>(_ => new SpecialisationRepository(connectionString));
        services.AddScoped<IAssuranceRepository>(_ => new AssuranceRepository(connectionString));
        services.AddScoped<ITypeMaladieRepository>(_ => new TypeMaladieRepository(connectionString));
        services.AddScoped<IStatsRepository>(_ => new StatsRepository(connectionString));

        // ----- Gateways (implémentations des IGateways du Core) -----
        services.AddScoped<IPatientGateway, PatientGateway>();
        services.AddScoped<IMedecinGateway, MedecinGateway>();
        services.AddScoped<ISecretaireGateway, SecretaireGateway>();
        services.AddScoped<IAdministrateurGateway, AdministrateurGateway>();
        services.AddScoped<IRendezVousGateway, RendezVousGateway>();
        services.AddScoped<IPaiementGateway, PaiementGateway>();
        services.AddScoped<ISucursaleGateway, SucursaleGateway>();
        services.AddScoped<ISpecialisationGateway, SpecialisationGateway>();
        services.AddScoped<IAssuranceGateway, AssuranceGateway>();
        services.AddScoped<ITypeMaladieGateway, TypeMaladieGateway>();
        services.AddScoped<IStatsGateway, StatsGateway>();

        // ----- Sécurité -----
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        return services;
    }
}
