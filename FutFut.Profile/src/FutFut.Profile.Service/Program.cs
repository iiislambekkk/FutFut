using FutFut.Common.AWS3;
using FutFut.Common.EfCore;
using FutFut.Common.Identity;
using FutFut.Profile.Service.Data;
using FutFut.Profile.Service.Entities;
using FutFut.Profile.Service.HostedServices;
using FutFut.Profile.Service.Mapping;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Network;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEfCoreDbContext<AppDbContext>()
    .AddEFCoreRepository<ProfileEntity, AppDbContext>()
    .AddEFCoreRepository<PlayedHistory, AppDbContext>()
    .AddEFCoreRepository<SystemWorks, AppDbContext>()
    .AddAWS3Storage()
    .AddJwtBearerAuthentication();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
    .WriteTo.TCPSink("tcp://localhost:5666", new CompactJsonFormatter())
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHostedService<ObjectStorageCleanupHostedService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));


builder.Services.AddControllers(opt =>
{
    opt.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(opt =>
    {
        opt.DocumentPath = "/openapi/v1.json";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();