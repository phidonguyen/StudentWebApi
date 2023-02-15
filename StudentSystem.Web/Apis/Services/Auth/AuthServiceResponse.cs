using SystemTech.Core.Messages;
using StudentSystem.Web.Apis.Models;

namespace StudentSystem.Web.Apis.Services.Auth
{
    public class AuthLoginServiceResponse : BaseResponse<TokenModel, AuthLoginServiceFields>
    {
        public AuthLoginServiceResponse(AuthLoginServiceRequest authLoginServiceRequest) : base(authLoginServiceRequest) { }
    }
    
    public class AuthRefreshTokenServiceResponse : BaseResponse<TokenModel, AuthRefreshTokenServiceFields>
    {
        public AuthRefreshTokenServiceResponse(AuthRefreshTokenServiceRequest authRefreshTokenServiceRequest) : base(authRefreshTokenServiceRequest) { }
    }
}

