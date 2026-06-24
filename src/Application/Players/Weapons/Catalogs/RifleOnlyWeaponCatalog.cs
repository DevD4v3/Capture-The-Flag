namespace CTF.Application.Players.Weapons.Catalogs;

public class RifleOnlyWeaponCatalog : WeaponCatalogBase
{
    public override WeaponCatalogType Type => WeaponCatalogType.RifleOnly;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Sniper,
            WeaponDefinitions.Rifle
        ]);
    }
}
