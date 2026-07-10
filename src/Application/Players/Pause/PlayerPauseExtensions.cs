namespace CTF.Application.Players.Pause;

public static class PlayerPauseExtensions
{
    public static bool IsPaused(this Player player)
        => player.GetComponent<PlayerDataComponent>().IsPaused;
}
