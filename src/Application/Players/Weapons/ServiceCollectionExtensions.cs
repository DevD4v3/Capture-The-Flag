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
            .AddSingleton<WeaponCatalog>()
            .AddSingleton<IDictionary<WeaponCatalogType, WeaponCatalogBase>>(sp =>
            {
                var catalogs = sp.GetRequiredService<IEnumerable<WeaponCatalogBase>>();
                return catalogs.ToDictionary(w => w.Type);
            });

        return services;
    }

    private static IServiceCollection AddWeaponCatalog<T>(this IServiceCollection services)
        where T : WeaponCatalogBase
    {
        services.AddSingleton<WeaponCatalogBase, T>();
        return services;
    }
}
