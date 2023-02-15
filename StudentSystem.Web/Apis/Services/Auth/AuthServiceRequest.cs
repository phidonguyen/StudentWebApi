using SystemTech.Core.Messages;

namespace StudentSystem.Web.Apis.Services.Auth
{
    public class AuthLoginServiceRequest : BaseRequest<AuthLoginServiceFields>
    {
        public AuthLoginServiceRequest(AuthLoginServiceFields authLoginServiceFields) : base(authLoginServiceFields) { }
    }
    
    public class AuthRefreshTokenServiceRequest : BaseRequest<AuthRefreshTokenServiceFields>
    {
        public AuthRefreshTokenServiceRequest(AuthRefreshTokenServiceFields authRefreshTokenServiceFields) : base(authRefreshTokenServiceFields) { }
    }
}

