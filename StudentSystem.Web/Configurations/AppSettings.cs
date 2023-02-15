namespace StudentSystem.Web.Configurations
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; } = null!;
        public SwaggerSettings Swagger { get; set; } = null!;
        public JwtSettings Jwt { get; set; } = null!;
        public IdentityServerSettings IdentityServer { get; set; } = null!;
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; } = null!;
    }
    
    public class SwaggerSettings
    {
        public string ClientId { get; set; } = null!;

        public string Title { get; set; } = null!;

        public Dictionary<string, string> Scopes { get; set; } = new();
    }
    
    public class JwtSettings
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public double AccessTokenExpiryDuration { get; set; }
        public double RefreshTokenExpiryDuration { get; set; }
    }

    public class IdentityServerSettings
    {
        public string BaseUrl { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public bool RequireHttps { get; set; }
    }
}