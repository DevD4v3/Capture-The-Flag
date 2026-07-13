namespace CTF.Application.Players.GeneralCommands;

public class AdminCommands(
    IEntityManager entityManager,
    IServerService serverService,
    IWorldService worldService,
    IDialogService dialogService) : ISystem
{
    [PlayerCommand("cmdsadmin")]
    public void ShowAdminCommands(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.Admin))
            return;

        var content = Smart.Format(DetailedCommandInfo.Admin, new
        {
            Color1 = Color.Yellow,
            Color2 = Color.White
        });

        var dialog = new MessageDialog(
            caption: "Admin Commands", 
            content, 
            button1: "Close"
        );

        dialogService.ShowAsync(player, dialog);
    }

    [PlayerCommand("jetall")]
    public void GiveJetpackToPlayers(Player currentPlayer)
    {
        if (currentPlayer.HasLowerRoleThan(RoleId.Admin))
            return;

        var players = entityManager.GetComponents<Player>();

        foreach (Player player in players)
        {
            player.SpecialAction = SpecialAction.UseJetpack;
        }

        var message = Smart.Format(Messages.GiveJetpackToPlayers, new 
        { 
            PlayerName = currentPlayer.Name 
        });

        worldService.SendClientMessage(Color.Yellow, message);
    }

    [PlayerCommand("goto")]
    public void GoToPlayerPosition(
        Player currentPlayer, 
        [CommandParameter(Name = "playerId")]Player targetPlayer)
    {
        if (currentPlayer.HasLowerRoleThan(RoleId.Admin))
            return;

        if (currentPlayer == targetPlayer)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.PlayerIsEqualsToTargetPlayer);
            return;
        }

        currentPlayer.Position = targetPlayer.Position;
    }

    [PlayerCommand("get")]
    public void BringPlayerToMyPosition(
        Player currentPlayer,
        [CommandParameter(Name = "playerId")]Player targetPlayer)
    {
        if (currentPlayer.HasLowerRoleThan(RoleId.Admin))
            return;

        if (currentPlayer == targetPlayer)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.PlayerIsEqualsToTargetPlayer);
            return;
        }

        targetPlayer.Position = currentPlayer.Position;
    }

    [PlayerCommand("ban")]
    public void BanPlayer(
        Player currentPlayer,
        [CommandParameter(Name = "playerId")]Player targetPlayer,
        string reason)
    {
        if (currentPlayer.HasLowerRoleThan(RoleId.Admin))
            return;

        if (currentPlayer == targetPlayer)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.PlayerIsEqualsToTargetPlayer);
            return;
        }

        const int MaxLength = 50;
        if (reason.Length > MaxLength)
        {
            var message = Smart.Format(Messages.BanReason, new { Length = MaxLength });
            currentPlayer.SendClientMessage(Color.Red, message);
            return;
        }

        {
            var message = Smart.Format(Messages.SuccessfullyBanned, new
            {
                CurrentPlayer = currentPlayer.Name,
                TargetPlayer = targetPlayer.Name,
                Reason = reason
            });

            worldService.SendClientMessage(Color.Red, message);
        }

        targetPlayer.Ban(reason);
    }

    [PlayerCommand("unban")]
    public void UnbanPlayer(Player currentPlayer, string ip)
    {
        if (currentPlayer.HasLowerRoleThan(RoleId.Admin))
            return;

        var message = Smart.Format(Messages.SuccessfullyUnbanned, new { Ip = ip });
        currentPlayer.SendClientMessage(Color.Yellow, message);
        serverService.SendRconCommand($"unbanip {ip}");
    }

    [PlayerCommand("bannedips")]
    public void ShowBannedIPs(Player currentPlayer)
    {
        if (currentPlayer.HasLowerRoleThan(RoleId.Admin))
            return;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "bans.json");
        var content = File.ReadAllText(path);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var bannedPlayers = JsonSerializer.Deserialize<BannedPlayer[]>(content, options);

        if (bannedPlayers.Length == 0)
        {
            currentPlayer.SendClientMessage(Color.Red, Messages.NoMatchFound);
            return;
        }

        var dialog = new ListDialog(
            caption: $"Banned Players: {bannedPlayers.Length}", 
            button1: "Close"
        );

        foreach (BannedPlayer bannedPlayer in bannedPlayers)
        {
            dialog.Add(bannedPlayer.ToString());
        }

        dialogService.ShowAsync(currentPlayer, dialog);
    }

    private class BannedPlayer
    {
        public string Address { get; set; } = string.Empty;
        public string Player { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Time { get; set; } = "2023-12-07T16:05:21-0500";
        public override string ToString()
        {
            var dt = DateTimeOffset.Parse(Time).DateTime;
            var date = dt.ToString("yyyy/MM/dd");
            var time = dt.ToString("HH:mm:ss");
            return $"{Address} [{date} | {time}] {Player} - {Reason}";
        }
    }
}
