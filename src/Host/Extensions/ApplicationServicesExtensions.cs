namespace CTF.Host.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddSingleton(TimeProvider.System)
            .AddSingleton<UnixTimeSeconds>()
            .AddPlayerServices()
            .AddMapServices()
            .AddTeamServices();

        return services;
    }
}
