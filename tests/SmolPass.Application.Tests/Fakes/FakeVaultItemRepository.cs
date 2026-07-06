using SmolPass.Application.Interfaces;
using SmolPass.Domain.Entities;


namespace SmolPass.Application.Tests.Fakes;
public sealed class FakeVaultItemRepository : IVaultItemRepository
{
    // Le "magasin" en mémoire, indexé par Id. Fausse table SQL.
    private readonly Dictionary<Guid, VaultItem> _store = new();

    // Helper de TEST (pas dans l'interface) : pose un état initial pour l'Arrange,
    // sans passer par AddAsync — on ne veut pas qu'un test d'Update dépende d'Add.
    public void Seed(VaultItem item)
    {
        _store[item.Id] = item;
    }

    public Task<IReadOnlyList<VaultItem>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<VaultItem> items = _store.Values.Where(v => v.UserId == userId).ToList();
        return Task.FromResult(items);
    }

    public Task<VaultItem?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        VaultItem? item = _store.Values.FirstOrDefault(v => v.Id == id && v.UserId == userId);
        return Task.FromResult(item);
    }

    public Task AddAsync(VaultItem item, CancellationToken cancellationToken = default)
    {
        _store[item.Id] = item;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(VaultItem item, CancellationToken cancellationToken = default)
    {
        _store[item.Id] = item;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        // TODO : retirer SI (Id == id ET UserId == userId) ; idempotent (ne PAS jeter si absent)
        VaultItem? item = _store.Values.FirstOrDefault(v => v.Id == id && v.UserId == userId);
        if (item != null)
        {
            _store.Remove(item.Id);
        }

        return Task.CompletedTask;
    }
}

