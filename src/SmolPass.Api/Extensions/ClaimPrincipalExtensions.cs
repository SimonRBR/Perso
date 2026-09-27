using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmolPass.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal principal, out Guid userId)
    {
        string? sub = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(sub, out userId);
    }
}
