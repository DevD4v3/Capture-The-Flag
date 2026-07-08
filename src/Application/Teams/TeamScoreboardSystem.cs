namespace CTF.Application.Teams;

public class TeamScoreboardSystem(IDialogService dialogService) : ISystem
{
    [Event]
    public void OnPlayerKeyStateChange(Player player, Keys newKeys, Keys oldKeys)
    {
        if(KeyUtils.HasPressed(newKeys, oldKeys, Keys.No))
        {
            ShowPlayers(player);
        }
    }

    [PlayerCommand("scoreboard")]
    public void ShowPlayers(Player player)
    {
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;

        var caption = 
            $"{alphaTeam.ColorHex}Alpha Score: {alphaTeam.StatsPerRound.Score} " +
            $"{betaTeam.ColorHex}Beta Score: {betaTeam.StatsPerRound.Score}";

        var columnHeaders = new[]
        {
            "Name",
            "Score",
            "Kills",
            "Deaths"
        };

        var tablistDialog = new TablistDialog(
            caption,
            button1: "Close",
            button2: default,
            columnHeaders);

        var alphaTeamMembers = alphaTeam
            .Members
            .OrderByDescending(player => player.Score);

        foreach (Player teamMember in alphaTeamMembers) 
        {
            PlayerInfo teamMemberInfo = teamMember.GetRequiredInfo();
            string[] columns = 
            [
                $"{alphaTeam.ColorHex}{teamMember.Name}",
                $"{alphaTeam.ColorHex}{teamMember.Score}",
                $"{alphaTeam.ColorHex}{teamMemberInfo.StatsPerRound.Kills}",
                $"{alphaTeam.ColorHex}{teamMemberInfo.StatsPerRound.Deaths}"
            ];
            tablistDialog.Add(columns);
        }

        var betaTeamMembers = betaTeam
            .Members
            .OrderByDescending(player => player.Score);

        foreach (Player teamMember in betaTeamMembers)
        {
            PlayerInfo teamMemberInfo = teamMember.GetRequiredInfo();
            string[] columns = 
            [
                $"{betaTeam.ColorHex}{teamMember.Name}",
                $"{betaTeam.ColorHex}{teamMember.Score}",
                $"{betaTeam.ColorHex}{teamMemberInfo.StatsPerRound.Kills}",
                $"{betaTeam.ColorHex}{teamMemberInfo.StatsPerRound.Deaths}"
            ];
            tablistDialog.Add(columns);
        }

        dialogService.ShowAsync(player, tablistDialog);
    }
}
