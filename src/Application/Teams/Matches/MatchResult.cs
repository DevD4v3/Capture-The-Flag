namespace CTF.Application.Teams.Matches;

public class MatchResult
{
    public Team Winner { get; }
    public bool IsTie => Winner == Team.None;

    private MatchResult(Team winner)
        => Winner = winner;

    public static MatchResult Create(Team firstTeam, Team secondTeam)
    {
        if (firstTeam.IsWinner())
            return new MatchResult(firstTeam);

        if (secondTeam.IsWinner())
            return new MatchResult(secondTeam);

        return new MatchResult(Team.None);
    }
}
