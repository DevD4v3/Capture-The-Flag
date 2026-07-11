namespace CTF.Application.GunGames.WeaponProgressions.Definitions;

/// <summary>
/// Defines a GunGame weapon progression featuring the most powerful weapons in GTA.
/// </summary>
public class PowerfulWeaponProgression : WeaponProgressionBase
{
    public override WeaponProgressionType Type => WeaponProgressionType.Powerful;

    protected override void Define(List<IWeapon> weapons)
    {
        weapons.AddRange(
        [
            WeaponDefinitions.MP5,
            WeaponDefinitions.AK47,
            WeaponDefinitions.M4,
            WeaponDefinitions.Sawedoff,
            WeaponDefinitions.CombatShotgun,
            WeaponDefinitions.Deagle,
            WeaponDefinitions.Flamethrower,
            WeaponDefinitions.RocketLauncher,
            WeaponDefinitions.Heatseeker,
            WeaponDefinitions.Minigun,
            WeaponDefinitions.Chainsaw,
            WeaponDefinitions.Grenade
        ]);
    }
}
