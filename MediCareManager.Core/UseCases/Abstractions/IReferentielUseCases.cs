using MediCareManager.Core.Models;

namespace MediCareManager.Core.UseCases.Abstractions;

public interface ISpecialisationUseCases
{
    Task<IEnumerable<SpecialisationMedecin>> GetAllAsync();
    Task<int> CreateAsync(CreateSpecialisationDto dto);
    Task DeleteAsync(int id);
}

public interface IAssuranceUseCases
{
    Task<IEnumerable<Assurance>> GetAllAsync();
    Task<int> CreateAsync(CreateAssuranceDto dto);
    Task DeleteAsync(int id);
}

public interface ITypeMaladieUseCases
{
    Task<IEnumerable<TypeMaladie>> GetAllAsync();
    Task<int> CreateAsync(CreateTypeMaladieDto dto);
    Task DeleteAsync(int id);
}

public interface IAdminUseCases
{
    Task<AdminStatsDto> GetStatsAsync();
}
