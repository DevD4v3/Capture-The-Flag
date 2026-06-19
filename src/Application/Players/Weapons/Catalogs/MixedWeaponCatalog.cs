namespace CTF.Application.Players.Weapons.Catalogs;

/// <summary>
/// Provides a combination of walking and run weapons.
/// </summary>
/// <remarks>
/// This catalog contains all weapons available from both categories.
/// </remarks>
public class MixedWeaponCatalog : WeaponCatalogBase
{
    public override WeaponCatalogType Type => WeaponCatalogType.Mixed;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Colt45,
            WeaponDefinitions.Silenced,
            WeaponDefinitions.Deagle,
            WeaponDefinitions.Shotgun,
            WeaponDefinitions.CombatShotgun,
            WeaponDefinitions.Sawedoff,
            WeaponDefinitions.MP5,
            WeaponDefinitions.Uzi,
            WeaponDefinitions.Tec9,
            WeaponDefinitions.AK47,
            WeaponDefinitions.M4,
            WeaponDefinitions.Sniper,
            WeaponDefinitions.Rifle
        ]);
    }
}
