namespace CTF.Application.Players;

public static class PlayerServicesExtensions
{
    public static IServiceCollection AddPlayerServices(this IServiceCollection services)
    {
        services
            .AddSingleton<PlayerRankUpdater>()
            .AddSingleton<PlayerKillingSpreeUpdater>()
            .AddSingleton<PlayerStatsRenderer>()
            .AddSingleton<AuthenticationDialog>()
            .AddSingleton<AccountAuthenticator>()
            .AddComboServices()
            .AddChatServices()
            .AddWeaponServices();

        return services;
    }
}
