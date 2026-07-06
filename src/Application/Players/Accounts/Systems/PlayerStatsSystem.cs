namespace CTF.Application.Players.Accounts.Systems;

public class PlayerStatsSystem(
    IWorldService worldService,
    IDialogService dialogService,
    IPlayerRepository playerRepository,
    PlayerRankUpdater playerRankUpdater,
    KillingSpreeUpdater killingSpreeUpdater,
    PlayerStatsRenderer playerStatsRenderer) : ISystem
{
    [Event]
    public void OnPlayerConnect(Player player)
    {
        playerStatsRenderer.CreateTextDraw(player);
    }

    [Event]
    public void OnPlayerSpawn(Player player)
    {
        playerStatsRenderer.UpdateTextDraw(player);
    }

    [Event]
    public void OnPlayerDisconnect(Player player, DisconnectReason reason)
    {
        if (player.IsUnauthenticated())
            return;

        PlayerInfo playerInfo = player.GetRequiredInfo();
        playerInfo.SetLastConnection();
        playerRepository.UpdateLastConnection(playerInfo);
    }

    [Event]
    public void OnPlayerDeath(Player victim, Player killer, Weapon reason)
    {
        PlayerInfo victimInfo = victim.GetRequiredInfo();
        victimInfo.StatsPerRound.AddDeaths();
        victimInfo.StatsPerRound.ResetKillingSpree();
        victimInfo.AddTotalDeaths();
        playerRepository.UpdateTotalDeaths(victimInfo);

        if (killer.IsInvalidPlayer())
            return;

        PlayerInfo killerInfo = killer.GetRequiredInfo();
        killerInfo.StatsPerRound.AddKills();
        killerInfo.AddTotalKills();
        killer.AddScore();
        playerRepository.UpdateTotalKills(killerInfo);
        killingSpreeUpdater.Update(killer);
        playerRankUpdater.Update(killer);
        playerStatsRenderer.UpdateTextDraw(killer);
    }

    [Event]
    public void OnPlayerKeyStateChange(Player player, Keys newKeys, Keys oldKeys)
    {
        if (KeyUtils.HasPressed(newKeys, oldKeys, Keys.AnalogRight))
        {
            ShowStats(player);
        }
    }

    [PlayerCommand("re")]
    public void ResetPlayerStats(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        playerInfo.StatsPerRound.ResetKills();
        playerInfo.StatsPerRound.ResetDeaths();
        player.SetScore(0);
        playerStatsRenderer.UpdateTextDraw(player);
        var message = Smart.Format(Messages.ResetPlayerStats, new
        {
            PlayerName = player.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
    }

    [PlayerCommand("mystats")]
    public void ShowStats(Player player)
    {
        var content = GetPlayerContent(player);
        var dialog = new MessageDialog($"Stats: {player.Name}", content, "Close");
        dialogService.ShowAsync(player, dialog);
    }

    [PlayerCommand("stats")]
    public void ShowStats(Player currentPlayer, [CommandParameter(Name = "playerId")]Player targetPlayer)
    {
        var content = GetPlayerContent(targetPlayer);
        var dialog = new MessageDialog($"Stats: {targetPlayer.Name}", content, "Close");
        dialogService.ShowAsync(currentPlayer, dialog);
    }

    private static string GetPlayerContent(Player player)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        string createdAt = player.IsUnauthenticated() ? 
            "None" : 
            playerInfo.CreatedAt.ToIsoDateString();

        var content =
        $"""
        Current Team: {playerInfo.Team.Name}
        Score for Round: {player.Score}
        Kills for Round: {playerInfo.StatsPerRound.Kills}
        Deaths for Round: {playerInfo.StatsPerRound.Deaths}
        Killing Spree for Round: {playerInfo.StatsPerRound.KillingSpree}
        Coins: {playerInfo.StatsPerRound.Coins}/100
        Max Killing Spree: {playerInfo.MaxKillingSpree}
        Total Kills: {playerInfo.TotalKills}
        Total Deaths: {playerInfo.TotalDeaths}
        Brought Flags: {playerInfo.BroughtFlags}
        Captured Flags: {playerInfo.CapturedFlags}
        Dropped Flags: {playerInfo.DroppedFlags}
        Returned Flags: {playerInfo.ReturnedFlags}
        HeadShots: {playerInfo.HeadShots}
        Role: {playerInfo.RoleId}
        Rank: {playerInfo.RankId}
        Registration Date: {createdAt}
        """;
        return content;
    }
}
