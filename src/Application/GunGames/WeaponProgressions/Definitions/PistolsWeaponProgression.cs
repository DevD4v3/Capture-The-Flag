namespace CTF.Application.GunGames.WeaponProgressions.Definitions;

/// <summary>
/// Defines a GunGame weapon progression using only pistols.
/// </summary>
public class PistolsWeaponProgression : WeaponProgression
{
    public override WeaponProgressionType Type => WeaponProgressionType.Pistols;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Silenced,
            WeaponDefinitions.Colt45,
            WeaponDefinitions.Deagle,
            WeaponDefinitions.Silenced,
            WeaponDefinitions.Colt45,
            WeaponDefinitions.Deagle,
            WeaponDefinitions.Silenced,
            WeaponDefinitions.Colt45,
            WeaponDefinitions.Deagle,
            WeaponDefinitions.Knife
        ]);
    }
}
