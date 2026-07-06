using SmolPass.Application.Common;
using SmolPass.Application.Tests.Fakes;
using SmolPass.Application.UseCases.Vault;
using SmolPass.Contracts.Vault;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.Tests.UseCases.Vault;

public sealed class UpdateVaultItemUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_ItemExistant_PreserveIdentiteEtRemplaceContenu()
    {
        //Arrange
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

        byte[] newBlob = new byte[] { 1, 2, 4 };
        UpdateVaultItemRequest updateRequest = new UpdateVaultItemRequest(newBlob);

        UpdateVaultItemUseCase vaultItemUseCase = new UpdateVaultItemUseCase(testVault);
        CancellationToken cancellationToken = new CancellationToken(default);
        //ACT
        Result<VaultItem> ResultTestUpdate = await vaultItemUseCase.ExecuteAsync(test.Id, test.UserId, updateRequest, cancellationToken);
        
        //Assert
        Assert.True(ResultTestUpdate.IsSuccess);
        VaultItem update = ResultTestUpdate.Value;
        // --- Préservés (l'identité et les faits historiques) ---
        Assert.Equal(test.Id, update.Id);
        Assert.Equal(test.UserId, update.UserId);
        Assert.Equal(test.CreatedAt, update.CreatedAt);
        // --- Changés (le contenu et l'horodatage) ---
        Assert.Equal(newBlob, update.EncryptedBlob);
        Assert.NotEqual(test.EncryptedBlob, update.EncryptedBlob);
        Assert.NotEqual(test.UpdatedAt, update.UpdatedAt);
        Assert.Equal(DateTimeKind.Utc, update.UpdatedAt.Kind);
            
    }
    [Fact]
    public async Task ExecuteAsync_ItemInexistant_RetourneFailure()
    {
        // Arrange — fake VIDE : aucun item seedé
        FakeVaultItemRepository testVault = new FakeVaultItemRepository();
        UpdateVaultItemRequest updateRequest = new(new byte[] { 1, 2, 4 });
        UpdateVaultItemUseCase useCase = new(testVault);

        // Act — un id/userId qui ne correspond à rien
        Result<VaultItem> result = await useCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), updateRequest);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public async Task ExecuteAsync_MauvaisUser_RetourneFailure()
    {
        //Arrange
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

        byte[] newBlob = new byte[] { 1, 2, 4 };
        UpdateVaultItemRequest updateRequest = new UpdateVaultItemRequest(newBlob);

        UpdateVaultItemUseCase vaultItemUseCase = new UpdateVaultItemUseCase(testVault);
        CancellationToken cancellationToken = new CancellationToken(default);
        //ACT
        Result<VaultItem> ResultTestUpdate = await vaultItemUseCase.ExecuteAsync(test.Id, Guid.NewGuid(), updateRequest, cancellationToken);


        // Assert
        Assert.True(ResultTestUpdate.IsFailure);
        Assert.Throws<InvalidOperationException>(() => ResultTestUpdate.Value);
    }
}

