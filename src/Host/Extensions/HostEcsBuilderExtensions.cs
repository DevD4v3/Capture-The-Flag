namespace CTF.Host.Extensions;

public static class HostEcsBuilderExtensions
{
    public static IEcsBuilder EnableExceptionHandler(this IEcsBuilder builder)
    {
        string[] events =
        [
            "OnGameModeInit",
            "OnGameModeExit",
            "OnPlayerCommandText",
            "OnPlayerConnect",
            "OnPlayerDisconnect",
            "OnDialogResponse",
            "OnPlayerText",
            "OnPlayerUpdate",
            "OnPlayerDeath",
            "OnPlayerTakeDamage",
            "OnPlayerGiveDamage",
            "OnRconLoginAttempt",
            "OnRconCommand",
            "OnPlayerKeyStateChange",
            "OnPlayerPickUpPickup",
            "OnPlayerSpawn",
            "OnPlayerRequestClass",
            "OnPlayerRequestSpawn"
        ];

        foreach (string @event in events)
        {
            builder.UseMiddleware<GlobalExceptionHandler>(@event);
        }

        return builder;
    }

    public static IEcsBuilder RegisterPauseEventHandlers(this IEcsBuilder builder) 
    {
        var playerPauseSystem = builder.Services.GetRequiredService<PlayerPauseSystem>();
        var flagCarrierPauseHandler = builder.Services.GetRequiredService<FlagCarrierPauseHandler>();
        playerPauseSystem.PauseEvent += flagCarrierPauseHandler.OnPlayerPauseStateChange;
        return builder;
    }

    public static IEcsBuilder RegisterMapEventHandlers(this IEcsBuilder builder)
    {
        var mapRotationService = builder.Services.GetRequiredService<MapRotationService>();
        var rocketLauncherSystem = builder.Services.GetRequiredService<RocketLauncherSystem>();
        mapRotationService.LoadingMapEvent += rocketLauncherSystem.OnLoadingMap;
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
