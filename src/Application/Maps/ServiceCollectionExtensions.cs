namespace CTF.Application.Maps;

public static class MapServicesExtensions
{
    public static IServiceCollection AddMapServices(this IServiceCollection services)
    {
        string mapsPath = GameModePaths.Maps;

        services
            .AddSingleton<MapInfoService>()
            .AddSingleton<MapRotationService>()
            .AddSingleton<MapTextDrawRenderer>()
            .AddSingleton(sp => new MapInfoService(sp.GetRequiredService<MapCollection>(), mapsPath))
            .AddSingleton(_ => new MapCollection(mapsPath));

        return services;
    }
}
