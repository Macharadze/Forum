using FluentValidation;
using Forum.Api.Infrastructure.Auth;
using Forum.Api.Infrastructure.Extensions;
using Forum.Api.Infrastructure.MapsterConfiguration;
using Forum.Persistence.ConfigurationsAppsettingJson;
using Forum.Persistence.Context;
using Forum.Persistence.Seed;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.
builder.Services.RegisterMaps();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.ConfigureApiVersioning();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureHealthChecks(builder.Configuration.GetSection(nameof(ConnectionStrings)).GetSection(nameof(ConnectionStrings.DefaultConnection)).Value);



builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ConnectionStrings.DefaultConnection))));
builder.Services.AddHttpContextAccessor();
builder.Services.RegisterMaps();

builder.Services.ConfigureIdentity();

builder.Services.ConfigureSwagger();

builder.Services.AddTokenAuth(builder.Configuration.GetSection(nameof(JWTConfiguration)).GetSection(nameof(JWTConfiguration.Secret)).Value);
builder.Services.AddServices();


builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection(nameof(JWTConfiguration)));
builder.Services.Configure<LimitConfiguration>(builder.Configuration.GetSection(nameof(LimitConfiguration)));
builder.Services.Configure<ImagesAddress>(builder.Configuration.GetSection(nameof(ImagesAddress)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseHandlerMiddleware();
app.UseHttpsRedirection();

app.MapHealthChecks("/api/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCultureMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//await AccountSeed.Initialize(app.Services);

app.Run();
