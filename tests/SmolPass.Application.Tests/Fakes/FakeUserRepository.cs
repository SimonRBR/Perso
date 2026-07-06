using SmolPass.Application.Interfaces;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.Tests.Fakes;

public sealed class FakeUserRepository : IUserRepository
{
    private readonly Dictionary<string, User> _store = new();

    // Helper de TEST (hors interface) : pose un état initial pour l'Arrange,
    // sans passer par AddAsync.
    public void Seed(User user)
    {
        _store[user.Email] = user;
    }
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        User? user = _store.GetValueOrDefault(email);
        return Task.FromResult(user);
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        _store[user.Email] = user;
        return Task.CompletedTask;
    }
}