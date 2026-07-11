namespace CTF.Application.Players.Weapons;

public static class WeaponServicesExtensions
{
    public static IServiceCollection AddWeaponServices(this IServiceCollection services)
    {
        services
            .AddSingleton<WeaponCatalogBase, RunWeaponCatalog>()
            .AddSingleton<WeaponCatalogBase, WalkingWeaponCatalog>()
            .AddSingleton<WeaponCatalogBase, MixedWeaponCatalog>()
            .AddSingleton<WeaponCatalogBase, RifleOnlyWeaponCatalog>()
            .AddSingleton<WeaponCatalogBase, WarWeaponCatalog>()
            .AddSingleton<WeaponCatalogBase, HeavyWeaponCatalog>()
            .AddSingleton<WeaponCatalogBase, MeleeWeaponCatalog>()
            .AddSingleton<WeaponCatalog>()
            .AddSingleton<IDictionary<WeaponCatalogType, WeaponCatalogBase>>(sp =>
            {
                var catalogs = sp.GetRequiredService<IEnumerable<WeaponCatalogBase>>();
                return catalogs.ToDictionary(w => w.Type);
            });

        return services;
    }
}
