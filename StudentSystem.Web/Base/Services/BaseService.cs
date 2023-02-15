using Microsoft.AspNetCore.Http;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.Web.Common.Helpers;

namespace StudentSystem.Web.Base.Services
{
    public class BaseService : IAsyncDisposable
    {
        protected readonly StudentSystemDbContext DbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected string CurrentUserId => GetCurrentUser();
        protected string UserAgent => GetUserAgent();
        protected string IpAddress => GetIpAddress();
        protected BaseService(IHttpContextAccessor httpContextAccessor, StudentSystemDbContextFactory dbContextFactory)
        {
            DbContext = dbContextFactory.CreateDbContext();
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUser()
        {
            string userId = null;
            if (_httpContextAccessor.HttpContext != null)
                userId = _httpContextAccessor.HttpContext.User.GetCurrentUserId();
            return userId;
        }

        private string GetUserAgent()
        {
            string userAgent = null!;
            if (_httpContextAccessor.HttpContext != null)
                userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
            return userAgent;
        }

        private string GetIpAddress()
        {
            string ipAddress = null!;
            if (_httpContextAccessor.HttpContext != null)
                ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            return ipAddress;
        }

        public ValueTask DisposeAsync()
        {
            return DbContext.DisposeAsync();
        }
    }
}
