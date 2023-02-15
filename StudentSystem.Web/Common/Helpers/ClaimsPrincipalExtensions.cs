using System.Security.Claims;

namespace StudentSystem.Web.Common.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetCurrentUserId(this ClaimsPrincipal claimsPrincipal)
        {
            Claim claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
            if(!string.IsNullOrEmpty(claim?.Value)) return claim.Value;
            return null;
        }
        
        public static string GetEmailForUser(this ClaimsPrincipal claimsPrincipal)
        {
            Claim claim = claimsPrincipal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}