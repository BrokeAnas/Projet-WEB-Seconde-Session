using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Type de maladie » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class TypeMaladieGateway : ITypeMaladieGateway
{
    private readonly ITypeMaladieRepository _typeMaladieRepository;

    public TypeMaladieGateway(ITypeMaladieRepository typeMaladieRepository)
    {
        _typeMaladieRepository = typeMaladieRepository ?? throw new ArgumentNullException(nameof(typeMaladieRepository));
    }

    public Task<IEnumerable<TypeMaladie>> GetAllAsync()
        => _typeMaladieRepository.GetAllAsync();

    public Task<int> CreateAsync(TypeMaladie type)
        => _typeMaladieRepository.CreateAsync(type);

    public Task DeleteAsync(int id)
        => _typeMaladieRepository.DeleteAsync(id);
}
