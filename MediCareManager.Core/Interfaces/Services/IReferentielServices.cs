using MediCareManager.Core.DTOs;
using MediCareManager.Core.Entities;

namespace MediCareManager.Core.Interfaces.Services;

public interface ISpecialisationService
{
    Task<IEnumerable<SpecialisationMedecin>> GetAllAsync();
    Task<int> CreateAsync(CreateSpecialisationDto dto);
    Task DeleteAsync(int id);
}

public interface IAssuranceService
{
    Task<IEnumerable<Assurance>> GetAllAsync();
    Task<int> CreateAsync(CreateAssuranceDto dto);
    Task DeleteAsync(int id);
}

public interface ITypeMaladieService
{
    Task<IEnumerable<TypeMaladie>> GetAllAsync();
    Task<int> CreateAsync(CreateTypeMaladieDto dto);
    Task DeleteAsync(int id);
}

public interface IAdminService
{
    Task<AdminStatsDto> GetStatsAsync();
}
