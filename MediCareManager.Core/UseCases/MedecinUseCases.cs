using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Core.UseCases.Abstractions;

namespace MediCareManager.Core.UseCases;

public class MedecinUseCases : IMedecinUseCases
{
    private readonly IMedecinGateway _medecinGateway;

    public MedecinUseCases(IMedecinGateway medecinGateway)
    {
        _medecinGateway = medecinGateway;
    }

    public Task<IEnumerable<Medecin>> GetAllAsync()
        => _medecinGateway.GetAllAsync();
}
