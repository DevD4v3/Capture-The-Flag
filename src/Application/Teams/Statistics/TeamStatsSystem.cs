namespace CTF.Application.Teams.Statistics;

public class TeamStatsSystem(
    IDialogService dialogService, 
    IWorldService worldService,
    TeamTextDrawRenderer teamTextDrawRenderer) : ISystem
{
    [Event]
    public void OnPlayerSpawn(Player player)
    {
        teamTextDrawRenderer.Show(player);
    }

    [Event]
    public void OnPlayerDeath(Player victim, Player killer, Weapon reason)
    {
        PlayerInfo victimInfo = victim.GetRequiredInfo();
        victimInfo.Team.StatsPerRound.AddDeaths();

        if (killer.IsInvalidPlayer())
            return;

        PlayerInfo killerInfo = killer.GetRequiredInfo();
        killerInfo.Team.StatsPerRound.AddKills();
    }

    [PlayerCommand("rstats")]
    [RequiresRole(RoleId.Moderator)]
    public void ResetStats(Player player) 
    {
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        alphaTeam.StatsPerRound.Reset();
        betaTeam.StatsPerRound.Reset();
        teamTextDrawRenderer.UpdateTeamScore(alphaTeam);
        teamTextDrawRenderer.UpdateTeamScore(betaTeam);
        var message = Smart.Format(Messages.ResetTeamStats, new
        {
            PlayerName = player.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
    }

    [PlayerCommand("tstats")]
    public void ShowStats(Player player)
    {
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        var content =
        $"""
        {alphaTeam.ColorHex}>>> Alpha Team: 
        Members: {alphaTeam.Members.Count}
        Score: {alphaTeam.StatsPerRound.Score}
        Kills: {alphaTeam.StatsPerRound.Kills}
        Deaths: {alphaTeam.StatsPerRound.Deaths}
        Flag captured by: {alphaTeam.Flag.CarrierName}

        {betaTeam.ColorHex}>>> Beta Team: 
        Members: {betaTeam.Members.Count}
        Score: {betaTeam.StatsPerRound.Score}
        Kills: {betaTeam.StatsPerRound.Kills}
        Deaths: {betaTeam.StatsPerRound.Deaths}
        Flag captured by: {betaTeam.Flag.CarrierName}
        """;
        var dialog = new MessageDialog("Team Stats", content, "Close");
        dialogService.ShowAsync(player, dialog);
    }
}
