using SmolPass.Domain.Entities;

namespace SmolPass.Application.Interfaces
{
    public interface IVaultItemRepository
    {  
        Task<IReadOnlyList<VaultItem>> GetAllByUserIdAsync(Guid UserId, CancellationToken cancellationToken = default);

        //GET un item, seulement s'il appartient au user (IDOR)
        Task<VaultItem?> GetByIdAsync(Guid id, Guid UserId, CancellationToken cancellationToken = default);

        //POST
        Task AddAsync(VaultItem item, CancellationToken cancellationToken = default);

        //DELETE
        Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);

        //PUT
        Task UpdateAsync(VaultItem item, CancellationToken cancellationToken = default);
    }
}
