using SmolPass.Application.Common;
using SmolPass.Application.Interfaces;
using SmolPass.Contracts.Auth;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.UseCases.Auth;

public sealed class RegisterUserUseCase
{
    private readonly IUserRepository _userRepository;

    public RegisterUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> ExecuteAsync(
        RegisterRequest request,
        CancellationToken cancellationToken = default)
    {
        if (await _userRepository.GetByEmailAsync(request.Email, cancellationToken) != null)
        {
            return Result<User>.Failure("Un compte existe deja avec cet email.");
        }


        User nouvelUser = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            AuthSalt = request.AuthSalt,
            AuthHash = request.AuthHash,
            EncryptionSalt = request.EncryptionSalt,
            KdfIterations = request.KdfIterations,
            CreatedAt = DateTime.UtcNow
        };
        await _userRepository.AddAsync(nouvelUser, cancellationToken);

        return Result<User>.Success(nouvelUser); 
        
    }
}