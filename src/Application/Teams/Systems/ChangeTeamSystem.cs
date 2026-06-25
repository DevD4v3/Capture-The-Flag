namespace CTF.Application.Teams.Systems;

public class ChangeTeamSystem(
    IWorldService worldService,
    IDialogService dialogService,
    TeamTextDrawRenderer teamTextDrawRenderer,
    OnFlagDropped onFlagDropped) : ISystem
{
    [PlayerCommand("team")]
    public async Task ShowTeams(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        if (playerInfo.Team == Team.None)
        {
            player.SendClientMessage(Color.Red, Messages.NoTeam);
            return;
        }

        var columnHeaders = new[]
        {
            "Name",
            "Members"
        };
        var tablistDialog = new TablistDialog(
            caption: "Select a team", 
            button1: "Select", 
            button2: "Close", 
            columnHeaders);

        Team alphaTeam = Team.Alpha;
        tablistDialog.Add(columns:
        [
            $"{alphaTeam.ColorHex}{alphaTeam.Name}",
            $"{alphaTeam.ColorHex}{alphaTeam.Members.Count}"
        ], tag: alphaTeam);

        Team betaTeam = Team.Beta;
        tablistDialog.Add(columns:
        [
            $"{betaTeam.ColorHex}{betaTeam.Name}",
            $"{betaTeam.ColorHex}{betaTeam.Members.Count}"
        ], tag: betaTeam);

        TablistDialogResponse response = await dialogService.ShowAsync(player, tablistDialog);
        if (response.IsRightButtonOrDisconnected())
            return;

        Team selectedTeam = response.Item.Tag as Team;
        ChangeTeam(player, selectedTeam);
    }

    private void ChangeTeam(Player player, Team selectedTeam)
    {
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        PlayerInfo playerInfo = player.GetRequiredInfo();
        if (playerInfo.Team == selectedTeam)
        {
            player.SendClientMessage(Color.Red, Messages.PlayerIsAlreadyInTeam);
            return;
        }

        if (alphaTeam.Members.Count == betaTeam.Members.Count)
        {
            player.SendClientMessage(Color.Red, Messages.TeamsAreEqualInMembers);
            return;
        }

        if (selectedTeam.IsFull())
        {
            player.SendClientMessage(Color.Red, Messages.TeamIsFull);
            return;
        }

        if (playerInfo.IsCarryingEnemyFlag())
        {
            onFlagDropped.Handle(selectedTeam, player);
        }

        Team rivalTeam = selectedTeam.RivalTeam;
        selectedTeam.Members.Add(player);
        rivalTeam.Members.Remove(player);
        teamTextDrawRenderer.UpdateTeamMembers(selectedTeam);
        teamTextDrawRenderer.UpdateTeamMembers(rivalTeam);
        var message = Smart.Format(Messages.PlayerHasChangedTeams, new
        {
            PlayerName = player.Name,
            TeamName = selectedTeam.Name
        });
        worldService.SendClientMessage(selectedTeam.ColorHex, message);
        playerInfo.SetTeam(selectedTeam.Id);
        player.Team = (int)selectedTeam.Id;
        player.Spawn();
    }
}
