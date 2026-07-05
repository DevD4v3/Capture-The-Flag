namespace CTF.Application.GunGames.WeaponProgressions.Definitions;

/// <summary>
/// Defines a GunGame weapon progression using only high-skill weapons.
/// </summary>
public class HardcoreWeaponProgression : WeaponProgressionBase
{
    public override WeaponProgressionType Type => WeaponProgressionType.Hardcore;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Deagle,
            WeaponDefinitions.CountryRifle,
            WeaponDefinitions.SniperRifle,
            WeaponDefinitions.Deagle,
            WeaponDefinitions.CountryRifle,
            WeaponDefinitions.SniperRifle,
            WeaponDefinitions.Knife
        ]);
    }
}
