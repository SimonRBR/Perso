using Microsoft.EntityFrameworkCore;
using SmolPass.Application.Interfaces;
using SmolPass.Domain.Entities;
using SmolPass.Infrastructure.Persistence;


namespace SmolPass.Infrastructure.Repositories
{
    public class VaultItemRepository : IVaultItemRepository
    {
        private readonly AppDbContext _context;

        public VaultItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<VaultItem>> GetAllByUserIdAsync (Guid userId, CancellationToken cancellationToken)
        {
            return await _context.VaultItems
                .AsNoTracking()
                .Where(v => v.UserId == userId)
                .ToListAsync(cancellationToken);
        }
        
        public async Task<VaultItem?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            return await _context.VaultItems.AsNoTracking().SingleOrDefaultAsync(v => v.Id == id && v.UserId == userId, cancellationToken);
        }

        public async Task AddAsync(VaultItem vaultItem, CancellationToken cancellationToken)
        {
            _context.VaultItems.Add(vaultItem);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(VaultItem vaultItem, CancellationToken cancellationToken)
        {
            await _context.VaultItems
            .Where(v => v.Id == vaultItem.Id && v.UserId == vaultItem.UserId)  // scoping userId, comme Delete
            .ExecuteUpdateAsync(s => s
            .SetProperty(v => v.EncryptedBlob, vaultItem.EncryptedBlob)
            .SetProperty(v => v.UpdatedAt, vaultItem.UpdatedAt),
            cancellationToken);
        }

        public async Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            await _context.VaultItems.Where(v => v.Id == id && v.UserId == userId).ExecuteDeleteAsync(cancellationToken);
        }
    }
}
