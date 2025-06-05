using FutFut.Common.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FutFut.Common.EfCore;

public static class Extension
{
    public static IServiceCollection AddEfCoreDbContext<TСontext>(this IServiceCollection services) where TСontext : DbContext
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>()!;
        var efcoreSettings = configuration.GetSection(nameof(EFCoreSettings)).Get<EFCoreSettings>()!;
        Console.WriteLine(efcoreSettings.ConnectionString);
        
        services.AddDbContext<TСontext>(options => options.UseNpgsql(efcoreSettings.ConnectionString));
        
        return services;
    }

    public static IServiceCollection AddEFCoreRepository<T, TContext>(this IServiceCollection services) where T : class, IEntity where TContext : DbContext
    {
        services.AddScoped<IRepository<T>, EFCoreRepository<T, TContext>>();
        
        return services;
    }
}