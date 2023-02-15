namespace StudentSystem.Web.Apis.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthLoginServiceResponse> Login(AuthLoginServiceRequest authLoginServiceRequest);
        Task<AuthRefreshTokenServiceResponse> RefreshToken(AuthRefreshTokenServiceRequest authRefreshTokenServiceRequest);
    }
}
