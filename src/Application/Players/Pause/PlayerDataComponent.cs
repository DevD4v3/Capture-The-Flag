namespace CTF.Application.Players.Pause;

/// <summary>
/// Stores the runtime state required for player pause detection.
/// </summary>
public class PlayerDataComponent : Component
{
    private readonly Player _player;

    /// <summary>
    /// Gets the player's current state.
    /// </summary>
    public PlayerState State => _player.State;

    /// <summary>
    /// Gets or sets whether the player is currently paused.
    /// </summary>
    public bool IsPaused { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the last received
    /// <c>OnPlayerUpdate</c> callback.
    /// </summary>
    public long LastUpdateTick { get; set; }

    public PlayerDataComponent(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        _player = player;
    }
}
