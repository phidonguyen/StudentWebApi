using Microsoft.Extensions.Configuration;

namespace SystemTech.Core.JwtManager
{
    public interface IJwtManagerConfiguration
    {
        public string JwtKey { get; }
        public string JwtIssuer { get; }
        public string JwtAudience { get; }
        public double JwtAccessTokenExpiryDuration { get; }
        public double JwtRefreshTokenExpiryDuration { get; }
        IConfigurationSection GetConfigurationSection(string key);
    }
}