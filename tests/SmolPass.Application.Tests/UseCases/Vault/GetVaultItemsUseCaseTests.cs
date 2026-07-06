using SmolPass.Application.Tests.Fakes;
using SmolPass.Application.UseCases.Vault;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.Tests.UseCases.Vault;

public sealed class GetVaultItemsUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_AucunItem_RetourneListeVide()
    {
        // Arrange — store vide (ou uniquement des items d'un AUTRE user), le use case
        // TODO
        FakeVaultItemRepository testVault = new FakeVaultItemRepository();
        testVault.Seed(ItemPour(Guid.NewGuid()));
        Guid newUser = Guid.NewGuid();

        // Act — interroger avec un userId qui n'a aucun item
        GetVaultItemsUseCase vaultItemsUseCase = new GetVaultItemsUseCase(testVault);
        // TODO 
        IReadOnlyList<VaultItem> items = await vaultItemsUseCase.ExecuteAsync(newUser);
        // Assert — liste vide, PAS d'échec (pas de Result ici)
        Assert.Empty(items);
    }

    [Fact]
    public async Task ExecuteAsync_PlusieursUsers_NeRetourneQueLesItemsDuBonUser()
    {
        // Arrange — 2 items pour user A, 1 item pour user B
        FakeVaultItemRepository testVault = new FakeVaultItemRepository();
        Guid userA = Guid.NewGuid();
        Guid userB = Guid.NewGuid();

        testVault.Seed(ItemPour(userA));
        testVault.Seed(ItemPour(userA));
        testVault.Seed(ItemPour(userB));
        // Act — interroger avec le userId de A
        GetVaultItemsUseCase vaultItemsUseCase = new GetVaultItemsUseCase(testVault);
        IReadOnlyList<VaultItem> items = await vaultItemsUseCase.ExecuteAsync(userA);


        Assert.Equal(2, items.Count);
        Assert.All(items, i => Assert.Equal(userA, i.UserId));
    }

    private static VaultItem ItemPour(Guid userId) => new()
    {
        Id = Guid.NewGuid(),
        UserId = userId,
        CreatedAt = new DateTime(2019, 6, 1, 0, 0, 0, DateTimeKind.Utc),
        EncryptedBlob = new byte[] { 1, 2, 3 },
        UpdatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)
    };


}