using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

public class SucursaleUseCases : ISucursaleUseCases
{
    private readonly ISucursaleGateway _sucursaleGateway;

    public SucursaleUseCases(ISucursaleGateway sucursaleGateway)
    {
        _sucursaleGateway = sucursaleGateway;
    }

    public Task<IEnumerable<Sucursale>> GetAllAsync()
        => _sucursaleGateway.GetAllAsync();
}
