using FluentValidation;
using Forum.Persistence.ConfigurationsAppsettingJson;
using Forum.Persistence.Context;
using Forum.Persistence.Seed;
using Forum.Web.Infrastructure.Extentions;
using Forum.Web.Infrastructure.MapsterConfig;
using Forum.Web.Infrastructure.Middlewares;
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
builder.Services.AddControllersWithViews();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection(nameof(ConnectionStrings)));
builder.Services.Configure<LimitConfiguration>(builder.Configuration.GetSection(nameof(LimitConfiguration)));
builder.Services.Configure<ImagesAddress>(builder.Configuration.GetSection(nameof(ImagesAddress)));
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString(nameof(ConnectionStrings.DefaultConnection))));

builder.Services.AddIdentity();

builder.Services.AddCustomCookieAuthentication();

builder.Services.AddServices();
builder.Services.RegisterMaps();

builder.Services.AddHealthChecksUI(setup =>
{
    setup.AddHealthCheckEndpoint("endpoint1", "http://localhost:8001/healthz");
})
    .AddSqlServerStorage(builder.Configuration.GetConnectionString(nameof(ConnectionStrings.DefaultConnection)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHealthChecksUI(config =>
   config.UIPath = "/hc-ui"
   );
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();



app.UseMiddleware<RequestResponseMiddleware>();
app.UseMiddleware<ErrorHandilngMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//AccountSeed.Initialize(app.Services);

app.Run();
