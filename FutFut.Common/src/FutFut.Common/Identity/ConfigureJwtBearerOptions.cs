using FutFut.Common.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FutFut.Common.Identity;

public class ConfigureJwtBearerOptions(IConfiguration configuration) : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(JwtBearerOptions options)
    {
        Configure(Options.DefaultName, options);
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name == JwtBearerDefaults.AuthenticationScheme)
        {
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            
            options.Authority = serviceSettings.Authority;
            options.Audience = serviceSettings.ServiceName;
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        }
    }
}