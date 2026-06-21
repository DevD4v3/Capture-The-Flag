namespace CTF.Application.Teams;

/// <summary>
/// Provides access to all players participating in the current match.
/// </summary>
public static class MatchPlayers
{
    /// <summary>
    /// Gets all players participating in the current match.
    /// </summary>
    public static IEnumerable<Player> GetAll()
    {
        foreach (Player player in Team.Alpha.Members) 
            yield return player;

        foreach (Player player in Team.Beta.Members)
            yield return player;
    }
}
