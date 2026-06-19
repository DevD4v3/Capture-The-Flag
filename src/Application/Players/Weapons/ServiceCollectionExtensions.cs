namespace CTF.Application.Players.Weapons;

public static class WeaponServicesExtensions
{
    public static IServiceCollection AddWeaponServices(this IServiceCollection services)
    {
        services
            .AddSingleton<WeaponCatalogBase, RunWeaponCatalog>()
            .AddSingleton<WeaponCatalogBase, WalkingWeaponCatalog>()
            .AddSingleton<WeaponCatalogBase, MixedWeaponCatalog>()
            .AddSingleton<WeaponCatalog>();

        return services;
    }
}
