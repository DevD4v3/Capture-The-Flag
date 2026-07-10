namespace CTF.Application.Players.Pause;

public class PlayerDataComponent : Component
{
    private readonly Player _player;
    public PlayerState State => _player.State;
    public bool IsPaused { get; set; }
    public long LastUpdateTick { get; set; }
    public PlayerDataComponent(Player player)
    {
        ArgumentNullException.ThrowIfNull(player);
        _player = player;
    }
}
