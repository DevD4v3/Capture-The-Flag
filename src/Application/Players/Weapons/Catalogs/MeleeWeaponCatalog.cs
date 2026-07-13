namespace CTF.Application.Players.Weapons.Catalogs;

public class MeleeWeaponCatalog : WeaponCatalogBase
{
    public override WeaponCatalogType Type => WeaponCatalogType.Melee;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Chainsaw,
            WeaponDefinitions.GolfClub,
            WeaponDefinitions.Nitestick,
            WeaponDefinitions.BaseballBat,
            WeaponDefinitions.Shovel,
            WeaponDefinitions.Poolstick,
            WeaponDefinitions.Katana,
            WeaponDefinitions.Dildo,
            WeaponDefinitions.PurpleDildo,
            WeaponDefinitions.Vibrator,
            WeaponDefinitions.SilverVibrator,
            WeaponDefinitions.Cane,
            WeaponDefinitions.Flower,
            WeaponDefinitions.Spraycan,
            WeaponDefinitions.FireExtinguisher
        ]);
    }
}
