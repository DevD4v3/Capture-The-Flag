namespace CTF.Host.Extensions;

public static class SerilogExtensions
{
    public static IServiceCollection AddSerilog(this IServiceCollection services)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "logs/ctf-.txt");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override(
                $"System.Net.Http.HttpClient.{nameof(IDiscordWebhookClient)}",
                LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate: 
                    "[{Level:u3}] {Message:lj}{NewLine}{Exception}", 
                theme: ConsoleTheme.None)
            .WriteTo.File(
                path,
                rollingInterval: RollingInterval.Day,
                outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddFilter("System.Net.Http.HttpClient", LogLevel.Warning);
            loggingBuilder.AddSerilog(dispose: true);
        });

        return services;
    }
}
