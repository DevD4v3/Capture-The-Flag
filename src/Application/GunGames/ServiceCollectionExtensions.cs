namespace CTF.Application.GunGames;

public static class GunGameExtensions
{
    public static IServiceCollection AddGunGameServices(this IServiceCollection services)
    {
        services
            .AddSingleton<GunGameReward>()
            .AddSingleton<GunGameSession>()
            .AddSingleton<WeaponProgression>()
            .AddSingleton<IGunGameMode>(sp => sp.GetRequiredService<GunGameSystem>());

        services
            .AddSingleton<WeaponProgressionBase, ClassicWeaponProgression>()
            .AddSingleton<WeaponProgressionBase, HardcoreWeaponProgression>()
            .AddSingleton<WeaponProgressionBase, PistolsWeaponProgression>()
            .AddSingleton<WeaponProgressionBase, ReverseClassicWeaponProgression>()
            .AddSingleton<WeaponProgressionBase, RiflesWeaponProgression>()
            .AddSingleton<WeaponProgressionBase, ShotgunsWeaponProgression>()
            .AddSingleton<WeaponProgressionBase, SmgsWeaponProgression>()
            .AddSingleton<IDictionary<WeaponProgressionType, WeaponProgressionBase>>(sp =>
            {
                var progressions = sp.GetRequiredService<IEnumerable<WeaponProgressionBase>>();
                return progressions.ToDictionary(w => w.Type);
            });

        services
            .AddSingleton<IGunGameResultHandler, PlayerLeveledDown>()
            .AddSingleton<IGunGameResultHandler, PlayerLeveledUp>()
            .AddSingleton<IGunGameResultHandler, PlayerReachedFinalLevel>()
            .AddSingleton<IGunGameResultHandler, PlayerScoredFinalKill>()
            .AddSingleton<IDictionary<GunGameResult, IGunGameResultHandler>>(sp =>
            {
                var handlers = sp.GetRequiredService<IEnumerable<IGunGameResultHandler>>();
                return handlers.ToDictionary(h => h.Result);
            });

        return services;
    }
}
