using SmolPass.Application.Common;
using SmolPass.Application.Interfaces;
using SmolPass.Contracts.Vault;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.UseCases.Vault;

public sealed class GetVaultItemByIdUseCase
{
    private readonly IVaultItemRepository _vaultItemRepository;

    public GetVaultItemByIdUseCase(IVaultItemRepository vaultItemRepository)
    {
        _vaultItemRepository = vaultItemRepository;
    }

    public async Task<Result<VaultItem>> ExecuteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        VaultItem? vault = await _vaultItemRepository.GetByIdAsync(id, userId, cancellationToken);

        if (vault == null)
        {
            return Result<VaultItem>.Failure("Element introuvable.");
        }
        else
        {
            return Result<VaultItem>.Success(vault);
        }

    }
}

