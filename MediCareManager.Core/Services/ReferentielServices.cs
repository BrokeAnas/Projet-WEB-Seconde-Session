using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;
using MediCareManager.Core.Interfaces.Repositories;
using MediCareManager.Core.Interfaces.Services;

namespace MediCareManager.Core.Services;

public class SpecialisationService : ISpecialisationService
{
    private readonly ISpecialisationRepository _repository;
    public SpecialisationService(ISpecialisationRepository repository) => _repository = repository;

    public Task<IEnumerable<SpecialisationMedecin>> GetAllAsync() => _repository.GetAllAsync();

    public Task<int> CreateAsync(CreateSpecialisationDto dto)
        => _repository.CreateAsync(new SpecialisationMedecin { Libelle = dto.Libelle });

    public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
}

public class AssuranceService : IAssuranceService
{
    private readonly IAssuranceRepository _repository;
    public AssuranceService(IAssuranceRepository repository) => _repository = repository;

    public Task<IEnumerable<Assurance>> GetAllAsync() => _repository.GetAllAsync();

    public Task<int> CreateAsync(CreateAssuranceDto dto)
        => _repository.CreateAsync(new Assurance { Nom = dto.Nom, Type = dto.Type });

    public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
}

public class TypeMaladieService : ITypeMaladieService
{
    private readonly ITypeMaladieRepository _repository;
    public TypeMaladieService(ITypeMaladieRepository repository) => _repository = repository;

    public Task<IEnumerable<TypeMaladie>> GetAllAsync() => _repository.GetAllAsync();

    public Task<int> CreateAsync(CreateTypeMaladieDto dto)
        => _repository.CreateAsync(new TypeMaladie { Libelle = dto.Libelle, CodeCim = dto.CodeCIM });

    public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
}

public class AdminService : IAdminService
{
    private readonly IStatsRepository _repository;
    public AdminService(IStatsRepository repository) => _repository = repository;

    public Task<AdminStatsDto> GetStatsAsync() => _repository.GetStatsAsync();
}
