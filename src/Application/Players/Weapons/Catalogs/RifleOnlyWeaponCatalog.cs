namespace CTF.Application.Players.Weapons.Catalogs;

public class RifleOnlyWeaponCatalog : WeaponCatalog
{
    public override WeaponCatalogType Type => WeaponCatalogType.RifleOnly;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.SniperRifle,
            WeaponDefinitions.CountryRifle
        ]);
    }
}
