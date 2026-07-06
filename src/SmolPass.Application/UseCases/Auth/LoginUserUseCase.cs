using SmolPass.Application.Common;
using SmolPass.Application.Interfaces;
using SmolPass.Contracts.Auth;
using SmolPass.Domain.Entities;
using System.Security.Cryptography;


namespace SmolPass.Application.UseCases.Auth
{
    public class LoginUserUseCase
    {
        private readonly IUserRepository _userRepository;

        //Aligner sur la confog EF
        private static readonly byte[] _dummyHash = new byte[32];
        public LoginUserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<User>> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            //ligne imparfaite; a compléter avec du rate limiting et une latence plafonnée coté API
            User? user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            //Eviter les tentatives d'un hacker de deviner ce qui serait KO/OK en normalisant le test via un dummyHash si 
            //user n'est pas récupéré.
            byte[] storedHash = user.AuthHash ?? _dummyHash;

            bool isValid = CryptographicOperations.FixedTimeEquals(storedHash, request.AuthHash);

            if(user == null ||!isValid)
            {
                return Result<User>.Failure("Identifiants inconnus.");
            }

            return Result<User>.Success(user);
        }
    }
}
