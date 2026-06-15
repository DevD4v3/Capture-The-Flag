namespace CTF.Application.Teams.Flags.Events;

/// <summary>
/// This event occurs when a player has dropped the opposing team's flag.
/// </summary>
public class OnFlagDropped(
    IPlayerRepository playerRepository,
    IWorldService worldService,
    TeamPickupService teamPickupService,
    TeamSoundsService teamSoundsService,
    FlagAutoReturnTimer flagAutoReturnTimer) : IFlagEvent
{
    public FlagStatus FlagStatus => FlagStatus.Dropped;

    public void Handle(Team team, Player player)
    {
        teamPickupService.CreateFlagFromVector3(team, player.Position);
        teamSoundsService.PlayFlagDroppedSound(team);
        flagAutoReturnTimer.Start(team);
        team.Flag.Drop();
        var message = Smart.Format(Messages.OnFlagDropped, new
        {
            PlayerName = player.Name,
            TeamName = team.Name,
            team.ColorName
        });
        worldService.SendClientMessage(team.ColorHex, message);
        worldService.GameText($"~n~~n~~n~{team.GameText}{team.ColorName} flag dropped!", TimeSpan.FromSeconds(5), GameTextStyle.Style3);

        PlayerInfo playerInfo = player.GetInfo();
        playerInfo.AddDroppedFlags();
        player.HideOnRadarMap();
        playerRepository.UpdateDroppedFlags(playerInfo);
    }
}
