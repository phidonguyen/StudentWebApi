using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.Web.Common.Helpers;
using StudentSystem.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

AppSettings appSettings = builder.Configuration.ConfigureAndGet<AppSettings>(builder.Services);

builder.Services.AddPooledDbContextFactory<StudentSystemDbContext>(option =>
{
    string connection = builder.Configuration.GetConnectionString("DefaultConnection");
    option.UseSqlServer(connection, x => x.EnableRetryOnFailure());
});

builder.Services.AddTransient<StudentSystemDbContextFactory>();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    
builder.Services.AddCors(options =>
{
    options.AddPolicy("APIPublicCors",
        _ => _.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

builder.Services.AddHttpClient();

// Auto mapping
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new ObjectMapping());
    cfg.AddProfile(new ModelMapping());
}).CreateMapper());

builder.Services.AddAuthentication(appSettings);

// DI settings
builder.Services.RegisterServices(appSettings);

// Swagger config
builder.Services.AddSwagger(appSettings);

builder.Services.AddHealthChecks()
    .AddSqlServer(appSettings.ConnectionStrings.DefaultConnection);

var app = builder.Build();

app.UseForwardedHeaders();

app.UseSwagger();

app.UseSwaggerUI();

app.UseDeveloperExceptionPage();

app.UseRouting();

app.UseCors("APIPublicCors");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
});

app.Run();