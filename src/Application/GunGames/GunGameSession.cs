namespace CTF.Application.GunGames;

/// <summary>
/// Represents the current GunGame session.
/// </summary>
public class GunGameSession
{
    /// <summary>
    /// Gets or sets the weapon progression used by the current GunGame session.
    /// </summary>
    public WeaponProgressionType WeaponProgressionType { get; set; } = WeaponProgressionType.Classic;

    /// <summary>
    /// Gets or sets the number of kills required to advance to the next weapon level.
    /// </summary>
    public KillsRequiredPerLevel KillsRequiredPerLevel { get; set; } = new(1);
}
