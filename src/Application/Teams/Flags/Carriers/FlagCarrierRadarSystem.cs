namespace CTF.Application.Teams.Flags.Carriers;

public class FlagCarrierRadarSystem(
    FlagCarrierSettings flagCarrierSettings,
    IWorldService worldService) : ISystem
{
    [PlayerCommand("showrm")]
    public void ShowOnRadarMap(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.Moderator))
            return;

        var message = Smart.Format(Messages.ShowFlagCarriersOnRadarMap, new
        {
            PlayerName = player.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
        Team.Alpha.Flag.Carrier?.ShowOnRadarMap();
        Team.Beta.Flag.Carrier?.ShowOnRadarMap();
        flagCarrierSettings.ShowOnRadarMap = true;
    }

    [PlayerCommand("hiderm")]
    public void HideOnRadarMap(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.Moderator))
            return;

        var message = Smart.Format(Messages.HideFlagCarriersOnRadarMap, new
        {
            PlayerName = player.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
        Team.Alpha.Flag.Carrier?.HideOnRadarMap();
        Team.Beta.Flag.Carrier?.HideOnRadarMap();
        flagCarrierSettings.ShowOnRadarMap = false;
    }
}
