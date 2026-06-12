namespace CTF.Host;

public class Startup : IEcsStartup
{
    public void Initialize(IStartupContext context)
    {
        context.UseEntities()
            .UseCommands();
    }

    public void ConfigureServices(IServiceCollection services, IConfiguration _)
    {
        var envVars = new EnvLoader()
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
            .AddStreamer();

        services.RemoveAll<ICommandTextFormatter>();
        services.AddSingleton<ICommandTextFormatter, CommandUsageFormatter>();

        // Add systems to the services collection
        services
            .AddSystemsInAssembly(typeof(CurrentMap).Assembly)
            .AddSystemsInAssembly(typeof(Startup).Assembly);
    }

    public void Configure(IEcsBuilder builder)
    {
        // TODO: Enable desired ECS system features
        builder
            .EnableExceptionHandler()
            .RegisterMiddlewares()
            .RegisterPauseEventHandlers()
            .RegisterMapEventHandlers()
            .EnableStreamerEvents();
    }
}
