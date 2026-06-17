namespace CTF.Host.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddPlayerServices()
            .AddMapServices(GameModePaths.Maps)
            .AddTeamServices();

        return services;
    }
}
