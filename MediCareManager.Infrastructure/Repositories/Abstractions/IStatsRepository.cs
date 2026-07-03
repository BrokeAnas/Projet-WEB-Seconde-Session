using MediCareManager.Core.Models;

namespace MediCareManager.Infrastructure.Repositories.Abstractions;

public interface IStatsRepository
{
    Task<AdminStatsDto> GetStatsAsync();
}
