using System.Security.Authentication;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.Base.Services;
using StudentSystem.Web.Common.Constants;
using StudentSystem.Web.Common.Helpers;
using StudentSystem.Web.Configurations;
using SystemTech.Core.Exceptions;
using SystemTech.Core.JwtManager;
using SystemTech.Core.Messages;
using SystemTech.Core.Utils;

namespace StudentSystem.Web.Apis.Services.Auth
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly Merger _merger;
        private readonly IMapper _mapper;
        private readonly IJwtManagerService _jwtManagerService;
        private readonly double _refreshTokenExpiryDuration;
        private readonly double _accessTokenExpiryDuration;

        public AuthService(IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IJwtManagerService jwtManagerService,
            IConfiguration configuration,
            StudentSystemDbContextFactory dbContextFactory) : base(httpContextAccessor, dbContextFactory)
        {
            var appSettings = configuration.Get<AppSettings>();
            _mapper = mapper;
            _merger = new Merger(mapper.ConfigurationProvider);
            _jwtManagerService = jwtManagerService;
            _refreshTokenExpiryDuration = appSettings.Jwt.RefreshTokenExpiryDuration;
            _accessTokenExpiryDuration = appSettings.Jwt.AccessTokenExpiryDuration;
        }

        #region Login

        public async Task<AuthLoginServiceResponse> Login(AuthLoginServiceRequest authLoginServiceRequest)
        {
            AuthLoginServiceResponse authLoginServiceResponse =
                new AuthLoginServiceResponse(authLoginServiceRequest);
            
            try
            {
                AuthLoginServiceFields authLoginServiceFields = authLoginServiceRequest.Fields;
                
                User user = await DbContext.Users.FirstOrDefaultAsync(_ => _.Email == authLoginServiceFields.Email);
                if (user == null)
                {
                    authLoginServiceResponse.AddException(
                        new RecordNotFoundException($"Email [{authLoginServiceFields.Email}] does not exist."));
                    return authLoginServiceResponse;
                }

                await ValidateLoggedUser(user, authLoginServiceFields, authLoginServiceResponse);
                if (authLoginServiceResponse.HasErrors())
                    return authLoginServiceResponse;
                
                var token = await CreateTokenForUser(user);
                
                // merging
                _merger.MergeData(authLoginServiceResponse, token);
            }
            catch (Exception ex)
            {
                authLoginServiceResponse.AddException(ex);
            }

            return authLoginServiceResponse;
        }

        #endregion

        #region RefreshToken

        public async Task<AuthRefreshTokenServiceResponse> RefreshToken(AuthRefreshTokenServiceRequest authRefreshTokenServiceRequest)
        {
            AuthRefreshTokenServiceResponse authRefreshTokenServiceResponse =
                new AuthRefreshTokenServiceResponse(authRefreshTokenServiceRequest);
            
            try
            {
                AuthRefreshTokenServiceFields authRefreshTokenServiceFields = authRefreshTokenServiceRequest.Fields;
                
                ClaimsPrincipal claimsPrincipal = _jwtManagerService.GetPrincipalFromExpiredToken(authRefreshTokenServiceFields.AccessToken);
                string userId = claimsPrincipal.GetCurrentUserId();

                Token token = await DbContext.Tokens.FirstOrDefaultAsync(_ =>
                        _.RefreshToken == authRefreshTokenServiceFields.RefreshToken &&
                        _.UserId == userId);
                
                if (token == null || token.RefreshTokenExpired <= DateTime.Now)
                {
                    var message = "Refresh token expired.";
                    authRefreshTokenServiceResponse.AddException(new AuthenticationException(message));
                    return authRefreshTokenServiceResponse;
                }

                User user = AuthHelpers.ExtractCurrentUser(claimsPrincipal);
                ClaimsIdentity claimsIdentity = AuthHelpers.ArchiveCurrentUser(user);

                token.AccessToken = _jwtManagerService.GenerateAccessToken(claimsIdentity);
                token.AccessTokenExpired = DateTime.Now.AddMinutes(_accessTokenExpiryDuration);
                token.RefreshTokenExpired = DateTime.Now.AddMinutes(_refreshTokenExpiryDuration);

                var tokenEntry = DbContext.Update(token);

                await DbContext.SaveChangesAsync();
                
                // merging
                _merger.MergeData(authRefreshTokenServiceResponse, tokenEntry.Entity);
            }
            catch (Exception ex)
            {
                authRefreshTokenServiceResponse.AddException(ex);
            }
            
            return authRefreshTokenServiceResponse;
        }

        #endregion

        #region Helper

        private async Task ValidateLoggedUser(User userDb, AuthLoginServiceFields authLoginServiceFields,
            AuthLoginServiceResponse authLoginServiceResponse)
        {
            if (userDb.Status == (int)UserStatusEnum.Banned)
            {
                string message = "Your account is locked. Contact admin.";
                authLoginServiceResponse.AddException(new UnauthorizedAccessException(message));   
            }

            if (!CryptoHelpers.VerifyPassword(authLoginServiceFields.Password, userDb.Password))
            {
                var numberLoginAttempt = await LoginFailedHandler(userDb);
                int remainingAttemptTimes = GeneralConstants.MaxLoginAttempt - numberLoginAttempt;
                string message = "The password is incorrect.";

                if (numberLoginAttempt > 0 && remainingAttemptTimes > 0)
                    message = $"The password is incorrect. You have {remainingAttemptTimes} attempts remaining.";
                
                if (numberLoginAttempt > 0 && remainingAttemptTimes == 0)
                    message = "Your account is locked.";

                authLoginServiceResponse.AddException(new AuthenticationException(message));
            }
        }

        private async Task<int> LoginFailedHandler(User user)
        {
            // tracking user login failed
            HistoryLogin historyLogin = new()
            {
                Ip = IpAddress,
                UserAgent = UserAgent,
                UserId = user.Id,
                IsSuccess = false
            };
            DbContext.Add(historyLogin);
            await DbContext.SaveChangesAsync();

            // checking user login attempt
            var loginAttempts = DbContext.HistoryLogins
                .Where(_ => _.UserId == user.Id)
                .OrderByDescending(_ => _.CreatedDate)
                .AsEnumerable()
                .TakeWhile(_ => !_.IsSuccess)
                .Take(GeneralConstants.MaxLoginAttempt);

            int numberLoginAttempt = loginAttempts.Count();

            if (numberLoginAttempt == GeneralConstants.MaxLoginAttempt)
            {
                user.Status = (int)UserStatusEnum.Banned;
                DbContext.Update(user);
            }

            return numberLoginAttempt;
        }

        private async Task<Token> CreateTokenForUser(User user)
        {
            ClaimsIdentity claimsIdentity = AuthHelpers.ArchiveCurrentUser(user);
            string accessToken = _jwtManagerService.GenerateAccessToken(claimsIdentity);
            string refreshToken = _jwtManagerService.GenerateRefreshToken();
            
            Token token = new()
            {
                UserId = user.Id,
                AccessToken = accessToken,
                AccessTokenExpired = DateTime.Now.AddMinutes(_accessTokenExpiryDuration),
                RefreshToken = refreshToken,
                RefreshTokenExpired = DateTime.Now.AddMinutes(_refreshTokenExpiryDuration)
            };
            var tokenEntry = DbContext.Add(token);

            // tracking login
            var historyLogin = new HistoryLogin
            {
                Ip = IpAddress,
                UserAgent = UserAgent,
                UserId = user.Id,
                TokenId = tokenEntry.Entity.Id,
                IsSuccess = true
            };
            DbContext.Add(historyLogin);

            await DbContext.SaveChangesAsync();
            return tokenEntry.Entity;
        }

        #endregion
    }
}