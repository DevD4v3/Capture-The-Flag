namespace CTF.Application.Teams.Flags.Events;

/// <summary>
/// This event occurs when a player has captured the opposing team's flag and brought it back to their own base.
/// </summary>
public class OnFlagScore(
    IPlayerRepository playerRepository,
    IWorldService worldService,
    TeamPickupService teamPickupService,
    TeamTextDrawRenderer teamTextDrawRenderer,
    PlayerStatsRenderer playerStatsRenderer) : IFlagEvent
{
    private const int CarrierEarnedCoins = 8;
    private const int CarrierEarnedScore = 4;
    private const int TeamEarnedCoins    = 5;
    private const int TeamEarnedHealth   = 10;
    private const int TeamEarnedScore    = 1;

    public FlagStatus FlagStatus => FlagStatus.Brought;

    public void Handle(Team team, Player player)
    {
        teamPickupService.CreateFlagFromBasePosition(team.RivalTeam);
        teamPickupService.DestroyExteriorMarker(team.RivalTeam);
        team.Sounds.PlayTeamScoresSound();
        teamTextDrawRenderer.UpdateTeamScore(team);

        var message = Smart.Format(Messages.OnFlagScore, new
        {
            PlayerName = player.Name,
            TeamName = team.Name,
            team.RivalTeam.ColorName
        });
        worldService.SendClientMessage(team.ColorHex, message);
        worldService.GameText($"~n~~n~~n~{team.GameText}{team.ColorName} team scores!", TimeSpan.FromSeconds(5), GameTextStyle.Style3);

        PlayerInfo playerInfo = player.GetRequiredInfo();
        playerInfo.StatsPerRound.AddCoins(CarrierEarnedCoins);
        playerInfo.AddBroughtFlags();
        player.AddScore(CarrierEarnedScore);
        player.HideOnRadarMap();
        playerRepository.UpdateBroughtFlags(playerInfo);
        GiveRewards(team);
    }

    private void GiveRewards(Team team)
    {
        TeamMembers teamMembers = team.Members;
        foreach (Player player in teamMembers)
        {
            PlayerInfo playerInfo = player.GetRequiredInfo();
            playerInfo.StatsPerRound.AddCoins(TeamEarnedCoins);
            player.AddHealth(TeamEarnedHealth);
            player.AddScore(TeamEarnedScore);
            playerStatsRenderer.UpdateTextDraw(player);
        }
    }
}
