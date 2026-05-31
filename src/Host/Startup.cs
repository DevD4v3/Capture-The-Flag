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
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

        services.ChooseDatabaseProvider(configuration);
        services
            .AddSerilog()
            .AddApplicationServices()
            .AddSettings(configuration)
            .AddSingleton<IPasswordHasher, PasswordHasherBcrypt>()
            .AddSingleton<IStreamerService, StreamerService>()
            .AddSingleton(configuration);

        // Add systems to the services collection
        services
            .AddSystemsInAssembly(typeof(ApplicationServicesExtensions).Assembly)
            .AddSystemsInAssembly(typeof(Startup).Assembly);
    }

    public void Configure(IEcsBuilder builder)
    {
        // TODO: Enable desired ECS system features
        builder
            .EnableExceptionHandler()
            .RegisterMiddlewares()
            .RegisterPauseEventHandlers()
            .RegisterMapEventHandlers();
    }
}
