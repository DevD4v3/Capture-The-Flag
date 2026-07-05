namespace CTF.Application.GunGames;

/// <summary>
/// Represents the possible results produced after processing a kill
/// according to the GunGame rules.
/// </summary>
public enum GunGameResult
{
    /// <summary>
    /// No progression-related action occurred.
    /// </summary>
    None,

    /// <summary>
    /// The killer advanced to the next weapon level.
    /// </summary>
    LeveledUp,

    /// <summary>
    /// The victim was demoted to the previous weapon level.
    /// </summary>
    LeveledDown,

    /// <summary>
    /// The killer reached the final weapon level.
    /// </summary>
    ReachedFinalLevel,

    /// <summary>
    /// The killer scored a kill while already at the final weapon level.
    /// </summary>
    ScoredFinalKill
}
