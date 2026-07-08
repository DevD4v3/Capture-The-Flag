namespace CTF.Application.Teams.Flags.Systems;

public class ReturnFlagSystem(
    IWorldService worldService,
    TeamPickupService teamPickupService,
    FlagAutoReturnTimer flagAutoReturnTimer) : ISystem
{
    [PlayerCommand("returnflag")]
    public void ReturnToBasePosition(
        Player player, 
        [CommandParameter(Name = "red/blue")]string color)
    {
        if (player.HasLowerRoleThan(RoleId.Moderator))
            return;

        Team team = color.ToLower() switch
        {
            "red" => Team.Alpha,
            "blue" => Team.Beta,
            _ => null
        };

        if (team is null)
        {
            player.SendClientMessage(Color.Red, Messages.InvalidFlagColor);
            return;
        }

        var message = Smart.Format(Messages.ReturnFlagToBasePosition, new
        {
            PlayerName = player.Name,
            team.ColorName
        });

        team.Flag.Carrier?.HideOnRadarMap();
        team.Flag.ReturnToBase();
        teamPickupService.CreateFlagFromBasePosition(team);
        teamPickupService.DestroyExteriorMarker(team);
        team.Sounds.PlayFlagReturnedSound();
        flagAutoReturnTimer.Stop(team);
        worldService.GameText($"~n~~n~~n~{team.GameText}{team.ColorName} flag returned!", TimeSpan.FromSeconds(5), GameTextStyle.Style3);
        worldService.SendClientMessage(Color.Yellow, message);
    }
}
