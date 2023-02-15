namespace StudentSystem.Web.Apis.Services.Auth
{
    public class AuthLoginServiceFields
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthRefreshTokenServiceFields
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
