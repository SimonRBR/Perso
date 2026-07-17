using SmolPass.Application.Common;
using SmolPass.Application.Tests.Fakes;
using SmolPass.Application.UseCases.Auth;
using SmolPass.Contracts.Auth;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.Tests.UseCases.Auth;

public sealed class LoginInitUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_EmailConnu_RestitueLesSelsEtIterations()
    {
        // Arrange — QUESTION : le succès EXIGE un user présent → on Seed (≠ Register-succès parti vide)
        FakeUserRepository repo = new FakeUserRepository();
        User user = new User()
        {
            Id = Guid.NewGuid(),
            Email = "simon@smolpass.dev",
            AuthSalt = new byte[] { 1, 1 },          // DISTINCT de EncryptionSalt (anti-inversion)
            AuthHash = new byte[] { 9, 9, 9 },        // décor : NON restitué
            EncryptionSalt = new byte[] { 2, 2, 2 },  // DISTINCT de AuthSalt
            KdfIterations = 3,
            CreatedAt = new DateTime(2019, 6, 1, 0, 0, 0, DateTimeKind.Utc)
        };
        repo.Seed(user);
        LoginInitUseCase useCase = new LoginInitUseCase(repo);

        // Act — ⚠ input = string email directement (pas de DTO request)
        Result<LoginInitResponse> result = await useCase.ExecuteAsync(user.Email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(user.AuthSalt, result.Value.AuthSalt);
        Assert.Equal(user.EncryptionSalt, result.Value.EncryptionSalt);
        Assert.Equal(user.KdfIterations, result.Value.KdfIterations);

    }

    [Fact]
    public async Task ExecuteAsync_EmailInconnu_RetourneFailure()
    {
        // Arrange — store VIDE : l'email demandé n'existe pas
        FakeUserRepository repo = new FakeUserRepository();
        LoginInitUseCase useCase = new LoginInitUseCase(repo);
        string email = "Do_Not_Exist@gmail.com";
        // Act
        // TODO : ExecuteAsync avec un email absent
        Result<LoginInitResponse> result = await useCase.ExecuteAsync(email);

        // Assert — QUESTION : IsFailure + .Value jette. AUCUNE relecture de store.
        Assert.True(result.IsFailure);
        Assert.Throws<InvalidOperationException>(()  => result.Value);
    }
}