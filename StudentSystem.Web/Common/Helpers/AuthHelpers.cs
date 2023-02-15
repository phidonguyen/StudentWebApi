using System.Security.Claims;
using StudentSystem.DataAccess.EntityFramework.Entities;

namespace StudentSystem.Web.Common.Helpers
{
    public class AuthHelpers
    {
        public static ClaimsIdentity ArchiveCurrentUser(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            return new ClaimsIdentity(claims);
        }

        public static User ExtractCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal == null) return null;
            string name = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            string guid = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            return new User
            {
                Name = name,
                Id = guid,
            };
        }
    }
}
