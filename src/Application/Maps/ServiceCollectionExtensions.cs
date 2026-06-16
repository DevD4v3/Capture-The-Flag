namespace CTF.Application.Maps;

public static class MapServicesExtensions
{
    public static IServiceCollection AddMapServices(
        this IServiceCollection services,
        string mapsPath)
    {
        services
            .AddSingleton<MapInfoService>()
            .AddSingleton<MapRotationService>()
            .AddSingleton<MapTextDrawRenderer>()
            .AddSingleton(_ => new MapCollection(mapsPath))
            .AddSingleton(sp =>
            {
                var maps = sp.GetRequiredService<MapCollection>();
                return new MapInfoService(
                    initialMap: maps.GetById(0).Value,
                    mapsPath: mapsPath);
            }); ;

        return services;
    }
}
