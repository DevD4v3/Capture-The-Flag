namespace CTF.Application.Teams.Services;

/// <summary>
/// Balances players between the Alpha and Beta teams based on their score.
/// </summary>
public class TeamBalancer(TeamTextDrawRenderer teamTextDrawRenderer)
{
    /// <summary>
    /// Reassigns players to teams and executes the specified action
    /// after each player has been assigned.
    /// </summary>
    /// <param name="action">
    /// The action to perform for each player after being assigned to a team.
    /// </param>
    /// <remarks>
    /// Players are sorted by score in descending order and then
    /// alternately assigned to the Alpha and Beta teams.
    /// </remarks>
    public void Balance(Action<Player, PlayerInfo> action)
    {
        Player[] players = AlphaBetaTeamPlayers.GetAll()
            .OrderByDescending(player => player.Score)
            .ToArray();

        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        alphaTeam.Reset();
        betaTeam.Reset();

        for (int index = 0; index < players.Length; index++)
        {
            Player player = players[index];
            PlayerInfo playerInfo = player.GetInfo();
            if (player.IsPaused())
            {
                playerInfo.StatsPerRound.ResetStats();
                playerInfo.SetTeam(TeamId.NoTeam);
                player.Team = (int)TeamId.NoTeam;
                player.Color = Team.None.ColorHex;
                player.SetScore(0);
                player.RedirectToClassSelection();
                continue;
            }

            if (int.IsEvenInteger(index))
            {
                alphaTeam.Members.Add(player);
                playerInfo.SetTeam(alphaTeam.Id);
                player.SendClientMessage(alphaTeam.ColorHex, Messages.AssignedToAlphaTeam);
            }
            else
            {
                betaTeam.Members.Add(player);
                playerInfo.SetTeam(betaTeam.Id);
                player.SendClientMessage(betaTeam.ColorHex, Messages.AssignedToBetaTeam);
            }
            action.Invoke(player, playerInfo);
        }

        teamTextDrawRenderer.UpdateTeamScore(alphaTeam);
        teamTextDrawRenderer.UpdateTeamScore(betaTeam);
        teamTextDrawRenderer.UpdateTeamMembers(alphaTeam);
        teamTextDrawRenderer.UpdateTeamMembers(betaTeam);
    }
}
