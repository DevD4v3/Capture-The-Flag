namespace CTF.Application.Tests.GunGames;

public class TestWeaponProgression : WeaponProgressionBase
{
    public override WeaponProgressionType Type => WeaponProgressionType.Classic;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Colt45,
            WeaponDefinitions.Shotgun,
            WeaponDefinitions.AK47,
            WeaponDefinitions.Knife
        ]);
    }
}

public class NonKnifeFinalWeaponProgression : WeaponProgressionBase
{
    public override WeaponProgressionType Type => WeaponProgressionType.Classic;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.Colt45,
            WeaponDefinitions.MP5,
            WeaponDefinitions.Knife,
            WeaponDefinitions.Minigun
        ]);
    }
}
