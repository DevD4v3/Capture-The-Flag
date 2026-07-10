namespace CTF.Application.Players.Pause;

/// <summary>
/// Provides extension methods for working with the player's paused state.
/// </summary>
public static class PlayerPauseExtensions
{
    /// <summary>
    /// Determines whether the specified player is currently paused.
    /// </summary>
    /// <param name="player">
    /// The player whose paused state should be checked.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the player is currently paused;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsPaused(this Player player)
        => player.GetComponent<PlayerDataComponent>().IsPaused;
}
