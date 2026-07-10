namespace CTF.Host.Extensions;

public static class HostEcsBuilderExtensions
{
    public static IEcsBuilder RegisterPauseEventHandlers(this IEcsBuilder builder) 
    {
        var playerPauseSystem = builder.Services.GetRequiredService<PlayerPauseSystem>();
        var flagCarrierPauseSystem = builder.Services.GetRequiredService<FlagCarrierPauseSystem>();
        playerPauseSystem.PauseEvent += flagCarrierPauseSystem.OnPlayerPauseStateChange;
        return builder;
    }

    public static IEcsBuilder RegisterMapEventHandlers(this IEcsBuilder builder)
    {
        var mapRotationService = builder.Services.GetRequiredService<MapRotationService>();
        var rocketLauncherSystem = builder.Services.GetRequiredService<RocketLauncherSystem>();
        var gunGameSystem = builder.Services.GetRequiredService<GunGameSystem>();
        mapRotationService.LoadingMapEvent += rocketLauncherSystem.OnLoadingMap;
        mapRotationService.LoadingMapEvent += gunGameSystem.OnLoadingMap;
        return builder;
    }

    public static IEcsBuilder RegisterMiddlewares(this IEcsBuilder builder)
    {
        builder
            .UseMiddleware<PlayerCommandLockMiddleware>(name: "OnPlayerCommandText")
            .UseMiddleware<PlayerRequestSpawnMiddleware>(name: "OnPlayerRequestSpawn");

        return builder;
    }
}
