namespace CTF.Application.Players.Weapons;

public static class WeaponServicesExtensions
{
    public static IServiceCollection AddWeaponServices(this IServiceCollection services)
    {
        services
            .AddWeaponCatalog<RunWeaponCatalog>()
            .AddWeaponCatalog<WalkingWeaponCatalog>()
            .AddWeaponCatalog<MixedWeaponCatalog>()
            .AddWeaponCatalog<RifleOnlyWeaponCatalog>()
            .AddWeaponCatalog<WarWeaponCatalog>()
            .AddWeaponCatalog<HeavyWeaponCatalog>()
            .AddWeaponCatalog<MeleeWeaponCatalog>()
            .AddSingleton<ActiveWeaponCatalog>()
            .AddSingleton(sp =>
            {
                var catalogs = sp.GetRequiredService<IEnumerable<WeaponCatalog>>();
                return catalogs.ToFrozenDictionary(w => w.Type);
            });

        return services;
    }

    private static IServiceCollection AddWeaponCatalog<T>(this IServiceCollection services)
        where T : WeaponCatalog
    {
        services.AddSingleton<WeaponCatalog, T>();
        return services;
    }
}
