using FutFut.Common.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Network;
using Serilog.Sinks.SystemConsole.Themes;

namespace FutFut.Common.Logging;

public static class Extension
{
    public static WebApplicationBuilder AddLoggingWithSeqAndLogstash(this WebApplicationBuilder builder)
    {
        var loggingConfig = builder.Configuration.GetSection(nameof(LoggingSettings)).Get<LoggingSettings>();
        var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();

        if (loggingConfig is null)
        {
            throw new Exception(
                $"Can't find logging configuration. Make sure that there is '{nameof(LoggingSettings)}' section with a field '{nameof(LoggingSettings.LogStashPort)}' in the configuration.");
        }
        
        if (serviceSettings is null)
        {
            throw new Exception(
                $"Can't find service configuration. Make sure that there is '{nameof(serviceSettings)}' section with a field '{nameof(serviceSettings.ServiceName)}' in the configuration.");
        }
        
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithProperty("source", serviceSettings.ServiceName)
            .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
            .WriteTo.TCPSink($"tcp://localhost:{loggingConfig.LogStashPort}", new CompactJsonFormatter())
            .CreateLogger();

        builder.Host.UseSerilog();
        
        Log.Logger.Information("Logger is enabled successfully!");

        return builder;
    }
}