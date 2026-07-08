namespace CTF.Application.Teams.Flags.Events;

/// <summary>
/// This event occurs when a player has taken the flag from a position other than the base.
/// </summary>
public class OnFlagTaken(
    IWorldService worldService,
    TeamPickupService teamPickupService,
    FlagAutoReturnTimer flagAutoReturnTimer,
    FlagCarrierSettings flagCarrierSettings) : IFlagEvent
{
    public FlagStatus FlagStatus => FlagStatus.Taken;

    public void Handle(Team team, Player player)
    {
        teamPickupService.DestroyFlag(team);
        team.Sounds.PlayFlagTakenSound();
        flagAutoReturnTimer.Stop(team);
        var message = Smart.Format(Messages.OnFlagTaken, new
        {
            PlayerName = player.Name,
            TeamName = team.Name,
            team.ColorName
        });
        worldService.SendClientMessage(team.ColorHex, message);
        worldService.GameText($"~n~~n~~n~{team.GameText}{team.ColorName} flag taken!", TimeSpan.FromSeconds(5), GameTextStyle.Style3);
        if (flagCarrierSettings.ShowOnRadarMap)
        {
            player.ShowOnRadarMap();
        }
    }
}
