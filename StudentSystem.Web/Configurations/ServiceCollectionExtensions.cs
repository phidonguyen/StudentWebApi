using SystemTech.Core.HelperService.Auth;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.Web.Apis.Services.Auth;
using StudentSystem.Web.Apis.Services.Students;
using StudentSystem.Web.Common.Helpers;

namespace StudentSystem.Web.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthentication(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = appSettings.Jwt.Issuer,
                        ValidAudience = appSettings.Jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.Jwt.Key))
                    };
                    
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var userid = context.Principal.GetCurrentUserId();
                            var dbContextFactory = context.HttpContext.RequestServices.GetRequiredService<StudentSystemDbContextFactory>();
                            using var dbContext = dbContextFactory.CreateDbContext();
                            var user = dbContext.Users.Find(userid);
                            
                            if (user == null)
                            {
                                context.Fail("Invalid token");
                            }
                            return Task.CompletedTask;
                        },
                    };
                });
            
            services.AddAuthorization();
        }
        
        public static void AddSwagger(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentSystem", Version = "v1" });
                c.OperationFilter<SwaggerAuthorizeOperationFilter>();
                
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "`Token only!!!` - without `Bearer_` prefix",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Scheme = "bearer"
                    });
            });

            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                // options.ReportApiVersions = true;
                // Specify the default API Version as 1.0
                options.DefaultApiVersion = new ApiVersion(1, 0);
                // If the client hasn't specified the API version in the request, use the default API version number
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";
                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });
        }

        public static void RegisterServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddSingleton<IJwtManagerConfiguration, JwtManagerConfiguration>();
            services.AddTransient<IJwtManagerService, JwtManagerService>();
            services.AddTransient<IStudentService, StudentService>();
        }
        
        public static T ConfigureAndGet<T>(
            this IConfiguration configuration, IServiceCollection services) where T: class
        {
            var appSettings = configuration.Get<T>();
            return appSettings;
        }
    }
}