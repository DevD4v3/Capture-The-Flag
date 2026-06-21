namespace CTF.Application.Teams;

public class MatchResult
{
    public Team Winner { get; }
    public string Announcement { get; }
    public bool IsTie => Winner == Team.None;

    private MatchResult(Team winner, string announcement)
    {
        Winner = winner;
        Announcement = announcement;
    }

    public static MatchResult Create()
    {
        if (Team.Alpha.IsWinner())
            return new MatchResult(Team.Alpha, Messages.AlphaIsWinner);

        if (Team.Beta.IsWinner())
            return new MatchResult(Team.Beta, Messages.BetaIsWinner);

        return new MatchResult(Team.None, Messages.TiedTeams);
    }
}
