using MediCareManager.Core.DTOs;

namespace MediCareManager.Core.Interfaces.Repositories;

public interface IStatsRepository
{
    Task<AdminStatsDto> GetStatsAsync();
}
