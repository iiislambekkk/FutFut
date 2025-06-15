using FutFut.Identity.Contracts;
using FutFut.Identity.Service.Entities;
using FutFut.Identity.Service.Settings;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FutFut.Identity.Service.HostedServices;

public class IdentitySeedHostedService : IHostedService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IdentitySettings _settings;

    public IdentitySeedHostedService(IServiceScopeFactory serviceScopeFactory, IOptions<IdentitySettings> options)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _settings = options.Value;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Seeding...");
        Console.ResetColor();
        
        using var scope = _serviceScopeFactory.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        
        await CreateRoleIfNotExistAsync(Roles.Admin, roleManager);
        await CreateRoleIfNotExistAsync(Roles.User, roleManager);

        var adminUser = await userManager.FindByEmailAsync(_settings.AdminUserEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser()
            {
                UserName = _settings.AdminUserEmail,
                Email = _settings.AdminUserEmail,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true
            };

            var isSuccess = await userManager.CreateAsync(adminUser, _settings.AdminUserPassword);
            
               
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(isSuccess.Succeeded);
            foreach (var error in isSuccess.Errors)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {error.Code} - {error.Description}");
            }
            Console.ResetColor();
            Console.ResetColor();
            await userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }
        
        
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Seeding Finished!");
        
        await publishEndpoint.Publish<UserCreated>(new UserCreated(Guid.Parse(adminUser.Id), adminUser.Email), cancellationToken);
        
        Console.ResetColor();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    public static async Task CreateRoleIfNotExistAsync(string role, RoleManager<ApplicationRole> roleManager)
    {
        var roleExist = await roleManager.RoleExistsAsync(role);

        if (!roleExist)
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = role });
        }
    }
}