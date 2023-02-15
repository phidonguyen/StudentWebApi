using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentSystem.Web.Apis.Controllers.Params;
using StudentSystem.Web.Apis.Services.Auth;
using StudentSystem.Web.Base.Controllers;

namespace StudentSystem.Web.Apis.Controllers
{
    [ApiController]
    [Route("api/v{version:ApiVersion}/auth")]
    [ApiVersion("1.0")]
    public class AuthController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IMapper mapper, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
        
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] AuthLoginParams authLoginParams)
        {
            AuthLoginServiceFields authLoginServiceFields = _mapper.Map<AuthLoginServiceFields>(authLoginParams);
            AuthLoginServiceRequest authLoginServiceRequest = new AuthLoginServiceRequest(authLoginServiceFields);
            AuthLoginServiceResponse authLoginServiceResponse = await _authService.Login(authLoginServiceRequest);
            
            return ResponseItem(authLoginServiceResponse);
        }
        
        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshToken([FromBody] AuthRefreshTokenParams authRefreshTokenParams)
        {
            AuthRefreshTokenServiceFields authRefreshTokenServiceFields = _mapper.Map<AuthRefreshTokenServiceFields>(authRefreshTokenParams);
            AuthRefreshTokenServiceRequest authRefreshTokenServiceRequest = new AuthRefreshTokenServiceRequest(authRefreshTokenServiceFields);
            AuthRefreshTokenServiceResponse authLoginServiceResponse = await _authService.RefreshToken(authRefreshTokenServiceRequest);
            
            return ResponseItem(authLoginServiceResponse);
        }

    }
}