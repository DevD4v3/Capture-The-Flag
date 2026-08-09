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

        string resultSummary = result.IsTie ?
            Messages.Tie :
            Smart.Format(
                Messages.Winner,
                new
                {
                    GameTextStyle = result.Winner.GameText,
                    TeamName = result.Winner.Name
                });

        worldService.SendClientMessage(Color.Yellow, resultMessage);
        worldService.GameText(resultSummary, TimeSpan.FromSeconds(4), GameTextStyle.Style3);
    }
}
