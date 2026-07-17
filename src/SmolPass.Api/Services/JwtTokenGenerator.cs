using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace SmolPass.Api.Services;

public class JwtTokenGenerator
{
    private readonly byte[] _secret;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryMinutes;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        string secret = configuration["Jwt:Secret"]?? throw new InvalidOperationException("Jwt:Secret manquant. En dev : dotnet user-secrets set \"Jwt:Secret\" \"<clé-base64>\" dans SmolPass.Api.");
        _secret = Convert.FromBase64String(secret);
        _issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer manquant. Regarde le fichier de config.");    
        _audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience manquant. Regarde le fichier de config.");
        string expiryMinutes = configuration["Jwt:ExpiryMinutes"] ?? throw new InvalidOperationException("Jwt:ExpiryMinutes manquant. Regarde le fichier de config.");
        _expiryMinutes = int.Parse(expiryMinutes);
    }

    public string GenerateToken(Guid userId, string email)
    {
        SymmetricSecurityKey key = new(_secret);
        SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);
        Claim[] claims = new Claim[] {new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                                      new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                      new(JwtRegisteredClaimNames.Email, email)};
        JwtSecurityToken securityToken = new(
        issuer: _issuer,
        audience: _audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
        signingCredentials: signingCredentials);
        JwtSecurityTokenHandler tokenHandler = new();

        return tokenHandler.WriteToken(securityToken); //serialize
    }
}



