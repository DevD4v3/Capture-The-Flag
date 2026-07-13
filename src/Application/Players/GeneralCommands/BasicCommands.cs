namespace CTF.Application.Players.GeneralCommands;

public class BasicCommands(
    IEntityManager entityManager,
    IDialogService dialogService) : ISystem
{
    [PlayerCommand("cmds")]
    public async Task ShowFirstCommandsPage(Player player)
    {
        var content = Smart.Format(DetailedCommandInfo.Public1, new
        {
            Color1 = Color.Yellow,
            Color2 = Color.White
        });

        var dialog = new MessageDialog(
            caption: "Commands [1/2]",
            content,
            button1: "Next",
            button2: "Close"
        );

        MessageDialogResponse response = await dialogService.ShowAsync(player, dialog);

        if (response.Response == DialogResponse.LeftButton)
            await ShowSecondCommandsPage(player);
    }

    private async Task ShowSecondCommandsPage(Player player)
    {
        var content = Smart.Format(DetailedCommandInfo.Public2, new
        {
            Color1 = Color.Yellow,
            Color2 = Color.White
        });

        var dialog = new MessageDialog(
            caption: "Commands [2/2]",
            content,
            button1: "Previous",
            button2: "Close"
        );

        MessageDialogResponse response = await dialogService.ShowAsync(player, dialog);

        if (response.Response == DialogResponse.LeftButton)
            await ShowFirstCommandsPage(player);
    }

    [PlayerCommand("help")]
    public void ShowHelp(Player player)
    {
        var content = Smart.Format(DetailedCommandInfo.Help, new
        {
            Color1 = Color.Yellow,
            Color2 = Color.White
        });

        var dialog = new MessageDialog(
            caption: "Help", 
            content, 
            button1: "Close"
        );

        dialogService.ShowAsync(player, dialog);
    }

    [PlayerCommand("credits")]
    public void ShowCredits(Player player)
    {
        var content = Smart.Format(DetailedCommandInfo.Credits, new
        {
            Color1 = Color.Yellow,
            Color2 = Color.White
        });

        var dialog = new MessageDialog(
            caption: "Credits",
            content,
            button1: "Close"
        );

        dialogService.ShowAsync(player, dialog);
    }

    [PlayerCommand("kill")]
    public void Kill(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();

        if (playerInfo.Team == Team.None)
        {
            player.SendClientMessage(Color.Red, Messages.NoTeam);
            return;
        }

        player.Health = 0;
    }

    [PlayerCommand("report")]
    public void ReportPlayer(
        Player currentPlayer,
        [CommandParameter(Name = "playerId")]Player targetPlayer,
        string reason)
    {
        if (currentPlayer == targetPlayer)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.PlayerIsEqualsToTargetPlayer);
            return;
        }

        IEnumerable<Player> admins = entityManager
            .GetComponents<Player>()
            .Where(player => player.GetRequiredInfo().RoleId >= RoleId.Moderator);

        if (!admins.Any())
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.NoAdminsConnected);
            return;
        }

        var message = Smart.Format(Messages.ReportToAnotherPlayer, new
        {
            CurrentPlayer = currentPlayer.Name,
            TargetPlayer = targetPlayer.Name,
            Reason = reason
        });

        foreach (Player admin in admins)
        {
            admin.SendClientMessage(Color.Red, message);
        }

        currentPlayer.SendClientMessage(Color.Yellow, Messages.ReportSuccessfullySent);
        currentPlayer.PlaySound(1058);
    }

    [PlayerCommand("spec")]
    public void EnableSpectatorMode(
        Player currentPlayer,
        [CommandParameter(Name = "playerId")]Player targetPlayer,
        TeamTextDrawRenderer teamTextDrawRenderer)
    {
        if (currentPlayer == targetPlayer)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.PlayerIsEqualsToTargetPlayer);
            return;
        }

        if (targetPlayer.IsInClassSelection())
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.PlayerIsInClassSelection);
            return;
        }

        if (currentPlayer.GetRequiredInfo().IsCarryingEnemyFlag())
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.HasCapturedFlag);
            return;
        }

        if (currentPlayer.Health < 85)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.PlayerWithInsufficientHealth);
            return;
        }

        Team removedTeam = currentPlayer.RemoveFromCurrentTeam();
        teamTextDrawRenderer.UpdateTeamMembers(removedTeam);
        currentPlayer.Interior = targetPlayer.Interior;
        currentPlayer.VirtualWorld = targetPlayer.VirtualWorld;
        currentPlayer.ToggleSpectating(true);
        currentPlayer.SpectatePlayer(targetPlayer);
        currentPlayer.SendClientMessage(Color.Yellow, Messages.ExitSpectatorMode);
    }
}
