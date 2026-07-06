using SmolPass.Application.Interfaces;

namespace SmolPass.Application.UseCases.Vault;

public sealed class DeleteVaultItemUseCase
{
    private readonly IVaultItemRepository _vaultItemRepository;

    public DeleteVaultItemUseCase(IVaultItemRepository vaultItemRepository)
    {
        _vaultItemRepository = vaultItemRepository;
    }

    public async Task ExecuteAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        await _vaultItemRepository.DeleteAsync(id, userId, cancellationToken);
    }
}