using System.ComponentModel.DataAnnotations;

namespace StudentSystem.Web.Apis.Controllers.Params
{
    public class AuthLoginParams
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
    
    public class AuthRefreshTokenParams
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
    
}
