using System.Security.Claims;

namespace SystemTech.Core.JwtManager
{
    public interface IJwtManagerService
    {
        string GenerateAccessToken(ClaimsIdentity claimsIdentity);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken();
    }
}