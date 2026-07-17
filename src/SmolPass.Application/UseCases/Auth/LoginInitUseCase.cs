using SmolPass.Application.Common;
using SmolPass.Application.Interfaces;
using SmolPass.Contracts.Auth;
using SmolPass.Domain.Entities;

namespace SmolPass.Application.UseCases.Auth
{
    public sealed class LoginInitUseCase
    {
        private readonly IUserRepository _userRepository;

        public LoginInitUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<LoginInitResponse>> ExecuteAsync(string email, CancellationToken cancellationToken = default)
        {
            User? user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user == null)
            {
                return Result<LoginInitResponse>.Failure("Aucun compte pour cet email.");
            }

            LoginInitResponse response = new LoginInitResponse
            (
                AuthSalt: user.AuthSalt,
                EncryptionSalt: user.EncryptionSalt,
                KdfIterations: user.KdfIterations
            );

            return Result<LoginInitResponse>.Success(response);
        }
    
    }
}
