namespace CTF.Application.Teams.Flags.Events;

/// <summary>
/// This event occurs when a player has captured the opposing team's flag from their base.
/// </summary>
public class OnFlagCaptured(
    IPlayerRepository playerRepository,
    IWorldService worldService,
    TeamPickupService teamPickupService,
    TeamSoundsService teamSoundsService,
    PlayerStatsRenderer playerStatsRenderer,
    FlagCarrierSettings flagCarrierSettings) : IFlagEvent
{
    public FlagStatus FlagStatus => FlagStatus.Captured;

    public void Handle(Team team, Player player)
    {
        teamPickupService.CreateExteriorMarker(team);
        teamPickupService.DestroyFlag(team);
        teamSoundsService.PlayFlagTakenSound(team);
        var message = Smart.Format(Messages.OnFlagCaptured, new
        {
            PlayerName = player.Name,
            TeamName = team.Name,
            team.ColorName
        });
        worldService.SendClientMessage(team.ColorHex, message);
        worldService.GameText($"~n~~n~~n~{team.GameText}{team.ColorName} flag captured!", TimeSpan.FromSeconds(5), GameTextStyle.Style3);

        PlayerInfo playerInfo = player.GetInfo();
        playerInfo.StatsPerRound.AddCoins(5);
        playerInfo.AddCapturedFlags();
        player.AddScore(2);
        if (flagCarrierSettings.ShowOnRadarMap)
        {
            player.ShowOnRadarMap();
        }
        playerRepository.UpdateCapturedFlags(playerInfo);
        playerStatsRenderer.UpdateTextDraw(player);
    }
}
