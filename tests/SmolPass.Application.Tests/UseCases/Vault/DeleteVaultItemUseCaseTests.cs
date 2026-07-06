using SmolPass.Application.Tests.Fakes;
using SmolPass.Application.UseCases.Vault;
using SmolPass.Domain.Entities;


namespace SmolPass.Application.Tests.UseCases.Vault;

public sealed class DeleteVaultItemUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ItemExistant_SupprimeLItem()
    {
        // Arrange — seed un item, bon propriétaire
        FakeVaultItemRepository testVault = new FakeVaultItemRepository();
        VaultItem test = new VaultItem
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreatedAt = new DateTime(2019, 6, 1, 0, 0, 0, DateTimeKind.Utc),
            EncryptedBlob = new byte[] { 1, 2, 3 },
            UpdatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        testVault.Seed(test);
        DeleteVaultItemUseCase vaultItemUseCase = new DeleteVaultItemUseCase(testVault);

        // Act — supprimer avec le bon id + userId
        await vaultItemUseCase.ExecuteAsync(test.Id, test.UserId);

        // Assert — le spy prouve l'ABSENCE : GetByIdAsync → null
        VaultItem? persisteDelete = await testVault.GetByIdAsync(test.Id, test.UserId);
        Assert.Null(persisteDelete);

    }



    [Fact]
    public async Task ExecuteAsync_ItemInexistant_NeJettePasEtResteCoherent()
    {
        // Arrange — store VIDE
        FakeVaultItemRepository testVault = new FakeVaultItemRepository();
        DeleteVaultItemUseCase vaultItemUseCase = new DeleteVaultItemUseCase(testVault);
        Guid id = Guid.NewGuid();
        Guid userId = Guid.NewGuid();
        // Act — supprimer un id qui ne matche rien
        //       (si ça jette, le test crashe tout seul = échec — pas besoin de DoesNotThrow)
        await vaultItemUseCase.ExecuteAsync(id, userId);

        // Assert — cohérence : le store est resté vide
        IReadOnlyList<VaultItem> testDeleteInexistant = await testVault.GetAllByUserIdAsync(userId);
        Assert.Empty(testDeleteInexistant);
    }

    [Fact]
    public async Task ExecuteAsync_MauvaisUser_NeSupprimePasLItemDAutrui()
    {
        // Arrange — item de user A
        FakeVaultItemRepository testVault = new FakeVaultItemRepository();
        VaultItem test = new VaultItem
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreatedAt = new DateTime(2019, 6, 1, 0, 0, 0, DateTimeKind.Utc),
            EncryptedBlob = new byte[] { 1, 2, 3 },
            UpdatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        testVault.Seed(test);

        // Act — tenter la suppression avec le bon id mais un AUTRE userId
        DeleteVaultItemUseCase vaultItemUseCase = new DeleteVaultItemUseCase(testVault);
        Guid userId = Guid.NewGuid();
        await vaultItemUseCase.ExecuteAsync(test.Id, userId);


        // Assert — l'item de A survit : relire avec le userId DE A → NotNull
        VaultItem? persistePasSupprime = await testVault.GetByIdAsync(test.Id, test.UserId);
        Assert.NotNull(persistePasSupprime);
    }
}

