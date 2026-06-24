namespace CTF.Application.Players.Weapons;

/// <summary>
/// Provides the canonical weapon definitions used by the game.
/// </summary>
/// <remarks>
/// This class acts as the single source of truth for weapon metadata,
/// such as weapon slots and names.
/// </remarks>
public class WeaponDefinitions
{
    public static readonly IWeapon Knife = new GtaWeapon(Weapon.Knife, 1);
    public static readonly IWeapon Parachute = new GtaWeapon(Weapon.Parachute, 11);

    public static readonly IWeapon Colt45 = new GtaWeapon(Weapon.Colt45, "Colt 45", 2);
    public static readonly IWeapon Silenced = new GtaWeapon(Weapon.Silenced, 2);
    public static readonly IWeapon Deagle = new GtaWeapon(Weapon.Deagle, 2);

    public static readonly IWeapon Shotgun = new GtaWeapon(Weapon.Shotgun, 3);
    public static readonly IWeapon CombatShotgun = new GtaWeapon(Weapon.CombatShotgun, "Combat Shotgun", 3);
    public static readonly IWeapon Sawedoff = new GtaWeapon(Weapon.Sawedoff, 3);

    public static readonly IWeapon MP5 = new GtaWeapon(Weapon.MP5, 4);
    public static readonly IWeapon Uzi = new GtaWeapon(Weapon.Uzi, 4);
    public static readonly IWeapon Tec9 = new GtaWeapon(Weapon.Tec9, "Tec-9", 4);

    public static readonly IWeapon AK47 = new GtaWeapon(Weapon.AK47, "AK-47", 5);
    public static readonly IWeapon M4 = new GtaWeapon(Weapon.M4, 5);

    public static readonly IWeapon Sniper = new GtaWeapon(Weapon.Sniper, "Sniper Rifle", 6);
    public static readonly IWeapon Rifle = new GtaWeapon(Weapon.Rifle, "Country Rifle", 6);

    public static readonly IWeapon RocketLauncher = new GtaWeapon(Weapon.RocketLauncher, "Rocket Launcher", 7);
    public static readonly IWeapon Heatseeker = new GtaWeapon(Weapon.HeatSeeker, "Heatseeker Rocket", 7);
    public static readonly IWeapon Flamethrower = new GtaWeapon(Weapon.FlameThrower, "Flamethrower", 7);
    public static readonly IWeapon Minigun = new GtaWeapon(Weapon.Minigun, 7);

    public static readonly IWeapon Grenade = new GtaWeapon(Weapon.Grenade, 8);
    public static readonly IWeapon TearGas = new GtaWeapon(Weapon.Teargas, "Tear Gas", 8);
    public static readonly IWeapon Molotov = new GtaWeapon(Weapon.Moltov, "Molotov Cocktail", 8);
    public static readonly IWeapon SatchelCharge = new GtaWeapon(Weapon.SatchelCharge, "Satchel Charge", 8);

    private class GtaWeapon : IWeapon
    {
        public Weapon Id { get; }
        public string Name { get; }
        public int Slot { get; }
        public bool Is(Weapon weapon) => Id == weapon;
        public bool IsNot(Weapon weapon) => !Is(weapon);

        public GtaWeapon(Weapon id, int slot)
        {
            Id = id;
            Name = id.ToString();
            Slot = slot;
        }

        public GtaWeapon(Weapon id, string name, int slot)
        {
            Id = id;
            Name = name;
            Slot = slot;
        }
    }
}
