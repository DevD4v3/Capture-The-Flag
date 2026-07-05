namespace CTF.Application.GunGames;

/// <summary>
/// Represents the current availability of GunGame mode.
/// </summary>
public interface IGunGameMode
{
    /// <summary>
    /// Gets a value indicating whether GunGame mode is currently active.
    /// </summary>
    bool IsEnabled { get; }
}
