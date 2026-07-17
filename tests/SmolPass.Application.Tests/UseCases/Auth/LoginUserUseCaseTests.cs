using SmolPass.Application.Common;
using SmolPass.Application.Tests.Fakes;
using SmolPass.Application.UseCases.Auth;
using SmolPass.Contracts.Auth;
using SmolPass.Domain.Entities;


namespace SmolPass.Application.Tests.UseCases.Auth;

public class LoginUserUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_BonHash_RetourneSuccess()
    {
        // Arrange
        FakeUserRepository repo = new FakeUserRepository();
        LoginUserUseCase useCase = new LoginUserUseCase(repo);

        byte[] authHash = Enumerable.Repeat((byte)0xCD, 32).ToArray();

        User user = new()
        {
            Id = Guid.NewGuid(),
            Email = "simon@smolpass.dev",
            AuthHash = authHash,
            AuthSalt = Enumerable.Repeat((byte)0xEF, 32).ToArray(),
            EncryptionSalt = Enumerable.Repeat((byte)0x0A, 32).ToArray(),
            KdfIterations = 600_000,
            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        repo.Seed(user);

        LoginRequest request = new LoginRequest
            (
                Email: "simon@smolpass.dev",
                AuthHash: (byte[])authHash.Clone()
            );

        // Act
        Result<User> result = await useCase.ExecuteAsync(request);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(user.Id, result.Value.Id);
        Assert.Equal(request.Email, result.Value.Email);
    }
    [Fact]
    public async Task ExecuteAsync_MauvaisHash_RetourneFailure()
    {
        // Arrange
        FakeUserRepository repo = new FakeUserRepository();
        LoginUserUseCase useCase = new LoginUserUseCase(repo);

        byte[] mauvaisHash = Enumerable.Repeat((byte)0xAB, 32).ToArray();

        User user = new()
        {
            Id = Guid.NewGuid(),
            Email = "simon@smolpass.dev",
            AuthHash = mauvaisHash,
            AuthSalt = Enumerable.Repeat((byte)0xB2, 32).ToArray(),
            EncryptionSalt = Enumerable.Repeat((byte)0x0A, 32).ToArray(),
            KdfIterations = 600_000,
            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        repo.Seed(user);

        LoginRequest request = new LoginRequest
            (
                Email: "simon@smolpass.dev",
                AuthHash: Enumerable.Repeat((byte)0xEF, 32).ToArray()
            );

        // Act
        Result<User> result = await useCase.ExecuteAsync(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Identifiants invalides.", result.Error);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }

    [Fact]
    public async Task ExecuteAsync_EmailInconnu_RetourneFailure()
    {
        // Arrange
        FakeUserRepository repo = new FakeUserRepository();
        LoginUserUseCase useCase = new LoginUserUseCase(repo);

        LoginRequest request = new LoginRequest
            (
                Email: "simon@smolpass.dev",
                AuthHash: Enumerable.Repeat((byte)0xEF, 32).ToArray()
            );

        // Act
        Result<User> result = await useCase.ExecuteAsync(request);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Identifiants invalides.", result.Error);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}

