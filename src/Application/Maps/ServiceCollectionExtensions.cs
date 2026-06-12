namespace CTF.Application.Maps;

public static class MapServicesExtensions
{
    public static IServiceCollection AddMapServices(this IServiceCollection services)
    {
        services
            .AddSingleton<MapInfoService>()
            .AddSingleton<MapRotationService>()
            .AddSingleton<MapTextDrawRenderer>()
            .AddSingleton(_ => new MapCollection(GameModePaths.Maps));

        return services;
    }
}
