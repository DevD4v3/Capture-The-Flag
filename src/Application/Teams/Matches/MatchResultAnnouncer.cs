namespace CTF.Application.Teams.Matches;

public class MatchResultAnnouncer(IWorldService worldService)
{
    public void Announce()
    {
        MatchResult result = MatchResult.Create(Team.Alpha, Team.Beta);

        string resultMessage = result.IsTie ? 
            Messages.TiedTeams : 
            Smart.Format(
                Messages.TeamIsWinner,
                new { result.Winner.Name });

        worldService.SendClientMessage(Color.Yellow, resultMessage);
    }
}
