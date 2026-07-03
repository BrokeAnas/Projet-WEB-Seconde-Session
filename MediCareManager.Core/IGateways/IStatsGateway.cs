using MediCareManager.Core.Models;

namespace MediCareManager.Core.IGateways;

public interface IStatsGateway
{
    Task<AdminStatsDto> GetStatsAsync();
}
