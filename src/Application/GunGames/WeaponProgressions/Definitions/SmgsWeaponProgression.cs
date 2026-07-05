namespace CTF.Application.GunGames.WeaponProgressions.Definitions;

/// <summary>
/// Defines a GunGame weapon progression using only submachine guns.
/// </summary>
public class SmgsWeaponProgression : WeaponProgressionBase
{
    public override WeaponProgressionType Type => WeaponProgressionType.SMGs;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Tec9,
            WeaponDefinitions.Uzi,
            WeaponDefinitions.MP5,
            WeaponDefinitions.Tec9,
            WeaponDefinitions.Uzi,
            WeaponDefinitions.MP5,
            WeaponDefinitions.Tec9,
            WeaponDefinitions.Uzi,
            WeaponDefinitions.MP5,
            WeaponDefinitions.Knife
        ]);
    }
}
