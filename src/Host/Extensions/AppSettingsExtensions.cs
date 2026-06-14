namespace CTF.Host.Extensions;

public static class AppSettingsExtensions
{
    public static IServiceCollection AddSettings(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var serverSettings = configuration
            .GetRequiredSection("ServerInfo")
            .Get<ServerSettings>();

        var commandCooldowns = configuration
            .GetRequiredSection("CommandCooldowns")
            .Get<CommandCooldowns>();

        var topPlayersSettings = configuration
            .GetRequiredSection("TopPlayers")
            .Get<TopPlayersSettings>();

        var serverOwnerSettings = configuration
            .GetRequiredSection("ServerOwner")
            .Get<ServerOwnerSettings>();

        var flagCarrierSettings = configuration
            .GetRequiredSection("FlagCarrier")
            .Get<FlagCarrierSettings>();

        var flagAutoReturnSettings = configuration
            .GetRequiredSection("FlagAutoReturn")
            .Get<FlagAutoReturnSettings>();

        var antiCBugSettings = configuration
            .GetRequiredSection("AntiCBug")
            .Get<AntiCBugSettings>();

        var headshotSettings = configuration
            .GetRequiredSection("Headshot")
            .Get<HeadshotSettings>();

        var classSelectionSettings = configuration
            .GetRequiredSection("ClassSelection")
            .Get<ClassSelectionSettings>();

        services
            .AddSingleton(serverSettings)
            .AddSingleton(commandCooldowns)
            .AddSingleton(topPlayersSettings)
            .AddSingleton(serverOwnerSettings)
            .AddSingleton(flagCarrierSettings)
            .AddSingleton(flagAutoReturnSettings)
            .AddSingleton(antiCBugSettings)
            .AddSingleton(headshotSettings)
            .AddSingleton(classSelectionSettings);

        return services;
    }
}
