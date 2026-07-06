using SmolPass.Application.Interfaces;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.UseCases.Vault
{
    public sealed class GetVaultItemsUseCase
    {
        private readonly IVaultItemRepository _vaultItemRepository;

        public GetVaultItemsUseCase(IVaultItemRepository vaultItemRepository)
        {
            _vaultItemRepository = vaultItemRepository;
        }

        public async Task<IReadOnlyList<VaultItem>> ExecuteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            IReadOnlyList<VaultItem> listeVault = await _vaultItemRepository.GetAllByUserIdAsync(userId, cancellationToken);
            return listeVault;
        }
    }
}
