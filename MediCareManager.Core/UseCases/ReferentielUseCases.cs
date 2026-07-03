using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

public class SpecialisationUseCases : ISpecialisationUseCases
{
    private readonly ISpecialisationGateway _specialisationGateway;
    public SpecialisationUseCases(ISpecialisationGateway specialisationGateway) => _specialisationGateway = specialisationGateway;

    public Task<IEnumerable<SpecialisationMedecin>> GetAllAsync() => _specialisationGateway.GetAllAsync();

    public Task<int> CreateAsync(CreateSpecialisationDto dto)
        => _specialisationGateway.CreateAsync(new SpecialisationMedecin { Libelle = dto.Libelle });

    public Task DeleteAsync(int id) => _specialisationGateway.DeleteAsync(id);
}

public class AssuranceUseCases : IAssuranceUseCases
{
    private readonly IAssuranceGateway _assuranceGateway;
    public AssuranceUseCases(IAssuranceGateway assuranceGateway) => _assuranceGateway = assuranceGateway;

    public Task<IEnumerable<Assurance>> GetAllAsync() => _assuranceGateway.GetAllAsync();

    public Task<int> CreateAsync(CreateAssuranceDto dto)
        => _assuranceGateway.CreateAsync(new Assurance { Nom = dto.Nom, Type = dto.Type });

    public Task DeleteAsync(int id) => _assuranceGateway.DeleteAsync(id);
}

public class TypeMaladieUseCases : ITypeMaladieUseCases
{
    private readonly ITypeMaladieGateway _typeMaladieGateway;
    public TypeMaladieUseCases(ITypeMaladieGateway typeMaladieGateway) => _typeMaladieGateway = typeMaladieGateway;

    public Task<IEnumerable<TypeMaladie>> GetAllAsync() => _typeMaladieGateway.GetAllAsync();

    public Task<int> CreateAsync(CreateTypeMaladieDto dto)
        => _typeMaladieGateway.CreateAsync(new TypeMaladie { Libelle = dto.Libelle, CodeCim = dto.CodeCIM });

    public Task DeleteAsync(int id) => _typeMaladieGateway.DeleteAsync(id);
}

public class AdminUseCases : IAdminUseCases
{
    private readonly IStatsGateway _statsGateway;
    public AdminUseCases(IStatsGateway statsGateway) => _statsGateway = statsGateway;

    public Task<AdminStatsDto> GetStatsAsync() => _statsGateway.GetStatsAsync();
}
