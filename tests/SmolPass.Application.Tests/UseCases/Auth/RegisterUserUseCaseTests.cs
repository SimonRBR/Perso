using SmolPass.Application.Common;
using SmolPass.Application.Tests.Fakes;
using SmolPass.Application.UseCases.Auth;
using SmolPass.Contracts.Auth;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.Tests.UseCases.Auth;

public sealed class RegisterUserUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_EmailLibre_CreeEtPersisteLeUser()
    {
        // Arrange — store VIDE (Register part de rien, comme Add)
        FakeUserRepository repo = new FakeUserRepository();
        // TODO : construis un RegisterRequest (email + 3 byte[] + KdfIterations).
        //        ⚠ 3 byte[] consécutifs → NOMME les arguments (piège 5.3).
        RegisterUserUseCase useCase = new RegisterUserUseCase(repo);
        RegisterRequest request = new RegisterRequest
            (
                Email: "test@gmail.com",
                AuthSalt: new byte[1],
                AuthHash : new byte[3], 
                EncryptionSalt: new byte[6],
                KdfIterations: 2
            );

        // Act
        // TODO : Result<User> result = await ...
        Result<User> result = await useCase.ExecuteAsync(request);

        Assert.True(result.IsSuccess);
        Assert.Equal(request.Email, result.Value.Email);
        Assert.Equal(request.AuthSalt, result.Value.AuthSalt);
        Assert.Equal(request.AuthHash, result.Value.AuthHash);
        Assert.Equal(request.EncryptionSalt, result.Value.EncryptionSalt);
        Assert.Equal(request.KdfIterations, result.Value.KdfIterations);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(DateTimeKind.Utc, result.Value.CreatedAt.Kind);
        

        // Assert 2 — la PERSISTANCE (le spy : relis le store)
        // TODO : GetByEmailAsync(email) → NotNull  ← assertion PORTEUSE (attrape le false green)
        User? persiste = await repo.GetByEmailAsync(request.Email);
        Assert.NotNull(persiste);
        Assert.Equal(request.Email, persiste.Email);
    }

    [Fact]
    public async Task ExecuteAsync_EmailDejaPris_RetourneFailure()
    {
        // Arrange — un user existe DÉJÀ sous cet email ; Id fixe connu pour le distinguer
        FakeUserRepository repo = new();
        Guid idExistant = Guid.NewGuid();
        User userExistant = new User
        {
            Id = idExistant,
            Email = "La_cible@hotmail.fr",
            AuthHash = new byte[2],
            AuthSalt = new byte[2],
            CreatedAt = DateTime.UtcNow,
            EncryptionSalt = new byte[2],
            KdfIterations = 0
        };
        repo.Seed(userExistant);
        // TODO : Seed un User { Id = idExistant, Email = <cible>, ... }
        // TODO : construis un RegisterRequest AU MÊME email, avec d'AUTRES byte[]
        //        (pour qu'un éventuel écrasement soit détectable)
        RegisterUserUseCase useCase = new(repo);
        RegisterRequest register = new RegisterRequest
        (
            Email: "La_cible@hotmail.fr",
            AuthSalt: new byte[1],
            AuthHash: new byte[3],
            EncryptionSalt: new byte[6],
            KdfIterations: 2
        );

        //Act
        Result<User> result = await useCase.ExecuteAsync(register);

        //Assert
        Assert.True(result.IsFailure);
        Assert.Throws<InvalidOperationException>(() => result.Value);

        //Persistance
        User? userDejaLa = await repo.GetByEmailAsync(register.Email);
        Assert.NotNull(userDejaLa);
        Assert.Equal(userExistant.Id, userDejaLa.Id);
        Assert.Equal(userExistant.Email, userDejaLa.Email);
        Assert.Equal(userExistant.AuthSalt, userDejaLa.AuthSalt);
        Assert.Equal(userExistant.AuthHash, userDejaLa.AuthHash);

    }
}