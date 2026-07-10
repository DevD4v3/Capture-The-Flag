namespace CTF.Application.Players;

public static class PlayerExtensions
{
    /// <summary>
    /// Gets the information associated with the specified player.
    /// </summary>
    /// <param name="player">
    /// The player whose information should be retrieved.
    /// </param>
    /// <returns>
    /// The player's information.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the player does not have an attached
    /// <see cref="AccountComponent"/>.
    /// </exception>
    public static PlayerInfo GetRequiredInfo(this Player player)
    {
        AccountComponent accountComponent = player.GetComponent<AccountComponent>();
        return accountComponent?.PlayerInfo
            ?? throw new InvalidOperationException(
                $"The player is missing the required {nameof(AccountComponent)}.");
    }

    /// <summary>
    /// Determines whether the specified player is unauthenticated.
    /// </summary>
    /// <param name="player">
    /// The player whose authentication status should be checked.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the player is unauthenticated;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the player does not have an attached
    /// <see cref="AccountComponent"/>.
    /// </exception>
    public static bool IsUnauthenticated(this Player player)
    {
        AccountComponent accountComponent = player.GetComponent<AccountComponent>();
        return accountComponent?.IsUnauthenticated
            ?? throw new InvalidOperationException(
                $"The player is missing the required {nameof(AccountComponent)}.");
    }

    /// <summary>
    /// Removes the specified player from their current team.
    /// </summary>
    /// <param name="player">
    /// The player to remove from the current team.
    /// </param>
    /// <returns>
    /// The team from which the player was removed, or <see cref="Team.None"/> if the player had no team.
    /// </returns>
    public static Team RemoveFromCurrentTeam(this Player player)
    {
        if (player.Team == (int)TeamId.NoTeam)
            return Team.None;

        PlayerInfo playerInfo = player.GetRequiredInfo();
        Team currentTeam = playerInfo.Team;
        currentTeam.Members.Remove(player);
        playerInfo.SetTeam(TeamId.NoTeam);
        player.Team = (int)TeamId.NoTeam;
        player.Color = Team.None.ColorHex;
        return currentTeam;
    }
}
