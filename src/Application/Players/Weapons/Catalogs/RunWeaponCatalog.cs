namespace CTF.Application.Players.Weapons.Catalogs;

/// <summary>
/// Defines a weapon catalog that allows players to remain mobile while fighting.
/// </summary>
/// <remarks>
/// These weapons support the classic Run Weapons (RW) gameplay style,
/// where players can move quickly while attacking.
/// </remarks>
public class RunWeaponCatalog : WeaponCatalog
{
    public override WeaponCatalogType Type => WeaponCatalogType.Run;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Colt45,
            WeaponDefinitions.Sawedoff,
            WeaponDefinitions.Uzi,
            WeaponDefinitions.Tec9
        ]);
    }
}
