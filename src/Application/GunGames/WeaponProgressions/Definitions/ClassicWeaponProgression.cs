namespace CTF.Application.GunGames.WeaponProgressions.Definitions;

/// <summary>
/// Defines the default GunGame weapon progression,
/// where players advance through increasingly challenging
/// weapons until reaching the final knife level.
/// </summary>
public class ClassicWeaponProgression : WeaponProgressionBase
{
    public override WeaponProgressionType Type => WeaponProgressionType.Classic;

    protected override void Define(List<IWeapon> weapons)
    {
        // Ordered from the first weapon level to the final weapon level.
        weapons.AddRange(
        [
            WeaponDefinitions.Silenced,
            WeaponDefinitions.Colt45,
            WeaponDefinitions.Shotgun,
            WeaponDefinitions.Sawedoff,
            WeaponDefinitions.CombatShotgun,
            WeaponDefinitions.Tec9,
            WeaponDefinitions.Uzi,
            WeaponDefinitions.MP5,
            WeaponDefinitions.AK47,
            WeaponDefinitions.M4,
            WeaponDefinitions.Deagle,
            WeaponDefinitions.CountryRifle,
            WeaponDefinitions.SniperRifle,
            WeaponDefinitions.Knife
        ]);
    }
}
