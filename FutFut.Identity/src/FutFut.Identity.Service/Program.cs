using FutFut.Common.MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FutFut.Identity.Service.Areas.Identity.Data;
using FutFut.Identity.Service.Entities;
using FutFut.Identity.Service.HostedServices;
using FutFut.Identity.Service.Settings;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder
    .Configuration.GetConnectionString("ApplicationDbContextConnection");

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<IdentitySettings>(builder.Configuration.GetSection(nameof(IdentitySettings)));

builder.Services.AddRazorPages(); 
builder.Services
    .AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    })
    .AddMassTransitWithRabbitMQ();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;           
        options.Password.RequireUppercase = false;      
        options.Password.RequireLowercase = false;     
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6; 
    }
)
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var identityServerSettings =
    builder.Configuration.GetSection(nameof(IdentityServerSettings)).Get<IdentityServerSettings>()!;

builder.Services.AddIdentityServer(opt =>
    {
        opt.Events.RaiseErrorEvents = true;
        opt.Events.RaiseInformationEvents = true;
        opt.Events.RaiseFailureEvents = true;
        opt.Events.RaiseSuccessEvents = true;
    })
    .AddAspNetIdentity<ApplicationUser>()
    .AddInMemoryApiScopes(identityServerSettings.ApiScopes)
    .AddInMemoryClients(identityServerSettings.Clients)
    .AddInMemoryApiResources(identityServerSettings.ApiResources)
    .AddInMemoryIdentityResources(identityServerSettings.IdentityResources);

builder.Services.AddLocalApiAuthentication();

builder.Services.AddControllers();
builder.Services.AddHostedService<IdentitySeedHostedService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(c =>
    {
        c.DocumentPath = "/openapi/v1.json";
    });
    
    app.UseCors(b =>
    {
        b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
