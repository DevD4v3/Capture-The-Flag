namespace CTF.Application.Players.Weapons.Catalogs;

public class HeavyWeaponCatalog : WeaponCatalog
{
    public override WeaponCatalogType Type => WeaponCatalogType.Heavy;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.RocketLauncher,
            WeaponDefinitions.Heatseeker,
            WeaponDefinitions.Minigun
        ]);
    }
}
