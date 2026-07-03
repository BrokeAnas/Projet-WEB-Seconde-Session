using MediCareManager.Core.IGateways;
using MediCareManager.Core.Models;
using MediCareManager.Infrastructure.Repositories.Abstractions;

namespace MediCareManager.Infrastructure.Gateways;

/// <summary>
/// Porte d'entrée « Médecin » du Core vers l'Infrastructure : reçoit le repository par DI
/// et lui délègue les opérations, en retournant des modèles du Core.
/// </summary>
public class MedecinGateway : IMedecinGateway
{
    private readonly IMedecinRepository _medecinRepository;

    public MedecinGateway(IMedecinRepository medecinRepository)
    {
        _medecinRepository = medecinRepository ?? throw new ArgumentNullException(nameof(medecinRepository));
    }

    public Task<IEnumerable<Medecin>> GetAllAsync(string? search = null)
        => _medecinRepository.GetAllAsync(search);

    public Task<Medecin?> GetByIdAsync(long idNat)
        => _medecinRepository.GetByIdAsync(idNat);

    public Task<Medecin?> GetByEmailAsync(string email)
        => _medecinRepository.GetByEmailAsync(email);

    public Task<long> CreateAsync(Medecin medecin)
        => _medecinRepository.CreateAsync(medecin);

    public Task UpdateAsync(Medecin medecin)
        => _medecinRepository.UpdateAsync(medecin);

    public Task DeleteAsync(long idNat)
        => _medecinRepository.DeleteAsync(idNat);

    public Task<bool> HasRendezVousFutursAsync(long idNat)
        => _medecinRepository.HasRendezVousFutursAsync(idNat);
}
