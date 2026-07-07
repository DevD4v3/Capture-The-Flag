namespace CTF.Application.Players.TopPlayers;

public class TopPlayersSettings
{
    /// <summary>
    /// Gets the required total kills for a player to be considered in the top players.
    /// </summary>
    public int RequiredTotalKills { get; init; } = 150;

    /// <summary>
    /// Gets the required maximum killing spree for a player to be considered in the top players.
    /// </summary>
    public int RequiredMaxKillingSpree { get; init; } = 10;
}
