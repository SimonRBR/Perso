using SmolPass.Application.Common;
using SmolPass.Application.Tests.Fakes;
using SmolPass.Application.UseCases.Vault;
using SmolPass.Contracts.Vault;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.Tests.UseCases.Vault;

public sealed class AddVaultItemUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_RequeteValid_ConstruitEtPersisteLItem()
    {
        //ARRANGE
        FakeVaultItemRepository testVault = new FakeVaultItemRepository();

        Guid userId = Guid.NewGuid();
        byte[] newBlob = new byte[] { 1, 2, 3 };
        CreateVaultItemRequest createRequest = new CreateVaultItemRequest(newBlob);
        AddVaultItemUseCase useCase = new AddVaultItemUseCase(testVault);

        //ACT
        Result<VaultItem> resultTestCreate = await useCase.ExecuteAsync(userId, createRequest);

        //ASSERT
        Assert.True(resultTestCreate.IsSuccess);
        VaultItem create = resultTestCreate.Value;
        Assert.NotEqual(create.Id, Guid.Empty);
        Assert.Equal(create.UserId, userId);
        Assert.Equal(create.CreatedAt, create.UpdatedAt);
        Assert.Equal(newBlob, create.EncryptedBlob);
        Assert.Equal(DateTimeKind.Utc, create.UpdatedAt.Kind);

        //ASSERT SPY
        VaultItem? persiste = await testVault.GetByIdAsync(create.Id, create.UserId);
        Assert.NotNull(persiste);
        Assert.Equal(create.EncryptedBlob, persiste.EncryptedBlob);
    }
 
}

