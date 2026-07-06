using SmolPass.Application.Common;
using SmolPass.Application.Tests.Fakes;
using SmolPass.Application.UseCases.Vault;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.Tests.UseCases.Vault;
public sealed class GetVaultItemByIdUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ItemExistant_RetourneSuccessAvecItem()
    {
        //ARRANGE
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
        GetVaultItemByIdUseCase vaultItemUseCase = new GetVaultItemByIdUseCase(testVault);

        //ACT
        Result<VaultItem> resultTestGet = await vaultItemUseCase.ExecuteAsync(test.Id, test.UserId);

        //ASSERT
        Assert.True(resultTestGet.IsSuccess);
        VaultItem get = resultTestGet.Value;
        Assert.Equal(test.Id, get.Id);
        Assert.Equal(test.UserId, get.UserId);
        Assert.Equal(test.EncryptedBlob, get.EncryptedBlob);
    }
    [Fact]
    public async Task ExecuteAsync_ItemInexistant_RetourneFailure()
    {
        // Arrange — fake VIDE
        FakeVaultItemRepository testVault = new FakeVaultItemRepository();
        GetVaultItemByIdUseCase vaultItemUseCase = new GetVaultItemByIdUseCase(testVault);
        // Act — id/userId qui ne matchent rien
        Result<VaultItem> resultTestGet = await vaultItemUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid());

        // Assert — Failure + .Value jette
        Assert.True(resultTestGet.IsFailure);
        Assert.Throws<InvalidOperationException>(() => resultTestGet.Value);
    }

    [Fact]
    public async Task ExecuteAsync_MauvaisUser_RetourneFailure()
    {
        //ARRANGE
        FakeVaultItemRepository testVault = new FakeVaultItemRepository();
        VaultItem test = new VaultItem
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            EncryptedBlob = new byte[] { 1, 2, 3 },
            UpdatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        testVault.Seed(test);
        GetVaultItemByIdUseCase vaultItemUseCase = new GetVaultItemByIdUseCase(testVault);

        //ACT
        Result<VaultItem> resultTestGet = await vaultItemUseCase.ExecuteAsync(test.Id, Guid.NewGuid());

        // Assert — le scoping ferme l'IDOR
        Assert.True(resultTestGet.IsFailure);
        Assert.Throws<InvalidOperationException>(() => resultTestGet.Value);
    }
}

