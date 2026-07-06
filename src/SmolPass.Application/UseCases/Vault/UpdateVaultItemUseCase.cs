using SmolPass.Application.Common;
using SmolPass.Application.Interfaces;
using SmolPass.Contracts.Vault;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.UseCases.Vault;

public sealed class UpdateVaultItemUseCase
{
    private readonly IVaultItemRepository _vaultItemRepository;

    public UpdateVaultItemUseCase(IVaultItemRepository vaultItemRepository)
    {
        _vaultItemRepository = vaultItemRepository;
    }

    public async Task<Result<VaultItem>> ExecuteAsync(Guid id, Guid userId, UpdateVaultItemRequest request, CancellationToken cancellationToken = default)
    {
        VaultItem? vault = await _vaultItemRepository.GetByIdAsync(id, userId, cancellationToken);

        if (vault == null)
        {
            return Result<VaultItem>.Failure("Element Introuvable.");
        }

        VaultItem newVault = new VaultItem
        {
            Id = id,
            UserId = userId,
            CreatedAt = vault.CreatedAt,
            UpdatedAt = DateTime.UtcNow,
            EncryptedBlob = request.EncryptedBlob
        };

        await _vaultItemRepository.UpdateAsync(newVault, cancellationToken);
        return Result<VaultItem>.Success(newVault);
    }

}
