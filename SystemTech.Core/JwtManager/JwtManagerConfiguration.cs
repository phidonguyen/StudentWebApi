using Microsoft.Extensions.Configuration;
using SystemTech.Core.Common;

namespace SystemTech.Core.JwtManager
{
    public class JwtManagerConfiguration : IJwtManagerConfiguration
    {
        private readonly IConfiguration _configuration;
        public JwtManagerConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string JwtKey => _configuration[$"{Constants.JwtConfigRoot.JwtKey}"];
        public string JwtIssuer => _configuration[$"{Constants.JwtConfigRoot.JwtIssuer}"];
        public string JwtAudience => _configuration[$"{Constants.JwtConfigRoot.JwtAudience}"];
        public double JwtAccessTokenExpiryDuration => double.Parse(_configuration[$"{Constants.JwtConfigRoot.JwtAccessTokenExpiryDuration}"]);
        public double JwtRefreshTokenExpiryDuration => double.Parse(_configuration[$"{Constants.JwtConfigRoot.JwtRefreshTokenExpiryDuration}"]);

        public IConfigurationSection GetConfigurationSection(string key)
        {
            return _configuration.GetSection(key);
        }
    }
}