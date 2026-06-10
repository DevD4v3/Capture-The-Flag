namespace CTF.Application.Teams.Services;

/// <summary>
/// Class responsible for balancing teams during map rotation.
/// </summary>
/// <remarks>
/// This class was designed to be used only in <see cref="MapRotationService"/>.
/// </remarks>
public class TeamBalancer(TeamTextDrawRenderer teamTextDrawRenderer)
{
    /// <summary>
    /// Balances the teams based on the players' scores.
    /// </summary>
    /// <param name="action">
    /// The action to perform for each player after being assigned to a team.
    /// </param>
    /// <remarks>
    /// This method sorts the players by score in descending order,
    /// alternately assigns them to the Alpha and Beta teams.
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
