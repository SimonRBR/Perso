using SmolPass.Application.Common;
using SmolPass.Application.Interfaces;
using SmolPass.Contracts.Vault;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.UseCases.Vault
{
    public sealed class AddVaultItemUseCase
    {
        private readonly IVaultItemRepository _vaultItemRepository;

        public AddVaultItemUseCase(IVaultItemRepository vaultItemRepository)
        {
            _vaultItemRepository = vaultItemRepository;
        }

        public async Task<Result<VaultItem>> ExecuteAsync(Guid userId, CreateVaultItemRequest request, CancellationToken cancellationToken = default)
        {
            DateTime date = DateTime.UtcNow;
            VaultItem item = new VaultItem
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                EncryptedBlob = request.EncryptedBlob,
                CreatedAt = date,
                UpdatedAt = date,
            };

            await _vaultItemRepository.AddAsync(item, cancellationToken);

            return Result<VaultItem>.Success(item);
        }
    }
}
