namespace CTF.Application.Teams.Matches;

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

    public static MatchResult Create(Team firstTeam, Team secondTeam)
    {
        if (firstTeam.IsWinner())
            return new MatchResult(
                firstTeam, 
                Smart.Format(Messages.TeamIsWinner, new { firstTeam.Name })
            );

        if (secondTeam.IsWinner())
            return new MatchResult(
                secondTeam, 
                Smart.Format(Messages.TeamIsWinner, new { secondTeam.Name })
            );

        return new MatchResult(Team.None, Messages.TiedTeams);
    }
}
