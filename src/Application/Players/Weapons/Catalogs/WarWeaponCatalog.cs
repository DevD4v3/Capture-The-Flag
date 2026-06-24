namespace CTF.Application.Players.Weapons.Catalogs;

public class WarWeaponCatalog : WeaponCatalogBase
{
    public override WeaponCatalogType Type => WeaponCatalogType.War;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Deagle,
            WeaponDefinitions.CombatShotgun,
            WeaponDefinitions.M4,
            WeaponDefinitions.Sniper,
            WeaponDefinitions.Grenade,
            WeaponDefinitions.Molotov,
            WeaponDefinitions.TearGas,
            WeaponDefinitions.Flamethrower,
            WeaponDefinitions.SatchelCharge
        ]);
    }
}
