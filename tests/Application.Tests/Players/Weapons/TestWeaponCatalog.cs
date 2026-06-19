namespace CTF.Application.Tests.Players.Weapons;

public class TestWeaponCatalog : WeaponCatalogBase
{
    public override WeaponCatalogType Type => WeaponCatalogType.Mixed;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Sawedoff,
            WeaponDefinitions.Tec9,
            WeaponDefinitions.Deagle,
            WeaponDefinitions.AK47,
            WeaponDefinitions.CombatShotgun
        ]);
    }
}
