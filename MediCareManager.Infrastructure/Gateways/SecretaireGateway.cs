using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Secrétaire » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class SecretaireGateway : ISecretaireGateway
{
    private readonly ISecretaireRepository _secretaireRepository;

    public SecretaireGateway(ISecretaireRepository secretaireRepository)
    {
        _secretaireRepository = secretaireRepository ?? throw new ArgumentNullException(nameof(secretaireRepository));
    }

    public Task<IEnumerable<Secretaire>> GetAllAsync()
        => _secretaireRepository.GetAllAsync();

    public Task<Secretaire?> GetByIdAsync(long idNat)
        => _secretaireRepository.GetByIdAsync(idNat);

    public Task<Secretaire?> GetByEmailAsync(string email)
        => _secretaireRepository.GetByEmailAsync(email);

    public Task<long> CreateAsync(Secretaire secretaire)
        => _secretaireRepository.CreateAsync(secretaire);

    public Task UpdateAsync(Secretaire secretaire)
        => _secretaireRepository.UpdateAsync(secretaire);

    public Task DeleteAsync(long idNat)
        => _secretaireRepository.DeleteAsync(idNat);
}
