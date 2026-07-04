namespace CTF.Application.Players.Weapons.Catalogs;

/// <summary>
/// Provides weapons that restrict player mobility while fighting.
/// </summary>
/// <remarks>
/// Players can only walk while using most weapons in this catalog.
/// </remarks>
public class WalkingWeaponCatalog : WeaponCatalogBase
{
    public override WeaponCatalogType Type => WeaponCatalogType.Walking;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Silenced,
            WeaponDefinitions.Deagle,
            WeaponDefinitions.Shotgun,
            WeaponDefinitions.CombatShotgun,
            WeaponDefinitions.MP5,
            WeaponDefinitions.AK47,
            WeaponDefinitions.M4,
            WeaponDefinitions.SniperRifle,
            WeaponDefinitions.CountryRifle
        ]);
    }
}
