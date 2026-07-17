
using Microsoft.AspNetCore.Mvc;
using SmolPass.Api.Services;
using SmolPass.Application.Common;
using SmolPass.Application.UseCases.Auth;
using SmolPass.Contracts.Auth;
using SmolPass.Domain.Entities;



namespace SmolPass.Api.Controllers;


[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUserUseCase;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly LoginInitUseCase _loginInitUseCase;
    private readonly LoginUserUseCase _loginUserUseCase;

    public AuthController(RegisterUserUseCase registerUserUseCase, JwtTokenGenerator jwtTokenGenerator, LoginInitUseCase loginInitUseCase,
        LoginUserUseCase loginUserUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _jwtTokenGenerator = jwtTokenGenerator;
        _loginInitUseCase = loginInitUseCase;
        _loginUserUseCase = loginUserUseCase;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        Result<User> result = await _registerUserUseCase.ExecuteAsync(request, cancellationToken);
        if (result.IsFailure)
        {
            return Conflict(new { message = result.Error });
        }

        RegisterResponse response = new(result.Value.Id, _jwtTokenGenerator.GenerateToken(result.Value.Id, result.Value.Email)); 
        return Created($"/api/auth/users/{result.Value.Id}", response);
    }

    [HttpGet("login-init")]
    public async Task<IActionResult> LoginInit(
        [FromQuery] string email, CancellationToken cancellationToken)
    {
        Result<LoginInitResponse> result = await _loginInitUseCase.ExecuteAsync(email, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(new { message = result.Error });           
        }
        return Ok(result.Value);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        Result<User> result = await _loginUserUseCase.ExecuteAsync(request, cancellationToken);
        if (result.IsFailure)
        {
            return Unauthorized(new { message = result.Error });
        }

        LoginResponse loginResponse = new(_jwtTokenGenerator.GenerateToken(result.Value.Id, result.Value.Email));
        return Ok(loginResponse);
    }

}

