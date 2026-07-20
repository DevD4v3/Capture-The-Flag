namespace CTF.Host;

public class Startup : IEcsStartup
{
    public void Initialize(IStartupContext context)
    {
        context.UseEntities()
            .UseConsoleCommands()
            .UsePlayerCommands(opts => opts.UsageMessageColor = Color.Red);
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration _)
    {
        new EnvLoader()
            #if DEBUG
            .EnableFileNotFoundException()
            #endif
            .AddEnvFile(".env")
            .Load();

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        services.ChooseDatabaseProvider(configuration);
        services
            .AddSerilog()
            .AddApplicationServices()
            .AddSettings(configuration)
            .AddSingleton<IPasswordHasher, PasswordHasherBcrypt>()
            .AddSingleton(configuration)
            .AddSingleton(TimeProvider.System)
            .AddSingleton<UnixTimeSeconds>()
            .AddStreamer()
            .AddMapObjects();

        services.RemoveAll<ICommandTextFormatter>();
        services.RemoveAll<IPermissionChecker>();
        services.AddSingleton<ICommandTextFormatter, CommandUsageFormatter>();
        services.AddSingleton<IPermissionChecker, PlayerRoleChecker>();

        services.AddHttpClient<IDiscordWebhookClient, DiscordWebhookClient>(httpClient =>
        {
            httpClient.Timeout = TimeSpan.FromSeconds(5);
        });

        // Add systems to the services collection
        services
            .AddSystemsInAssembly(typeof(CurrentMap).Assembly)
            .AddSystemsInAssembly(typeof(Startup).Assembly);
    }

    public void Configure(IEcsBuilder builder)
    {
        // TODO: Enable desired ECS system features
        builder
            .RegisterMiddlewares()
            .RegisterPauseEventHandlers()
            .RegisterMapEventHandlers()
            .RegisterTeamEventHandlers()
            .EnableStreamerEvents();
    }
}
