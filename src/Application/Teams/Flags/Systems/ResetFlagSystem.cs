namespace CTF.Application.Teams.Flags.Systems;

public class ResetFlagSystem(
    IWorldService worldService,
    TeamPickupService teamPickupService,
    TeamSoundsService teamSoundsService,
    FlagAutoReturnTimer flagAutoReturnTimer) : ISystem
{
    [PlayerCommand("resetflag")]
    public void ResetToBasePosition(
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

        ResetFlagPosition(player, team);
    }

    private void ResetFlagPosition(Player player, Team team)
    {
        var message = Smart.Format(Messages.ResetFlagPosition, new
        {
            PlayerName = player.Name,
            team.ColorName
        });
        team.IsFlagAtBasePosition = true;
        team.Flag.Carrier?.HideOnRadarMap();
        team.Flag.RemoveCarrier();
        teamPickupService.CreateFlagFromBasePosition(team);
        teamPickupService.DestroyExteriorMarker(team);
        teamSoundsService.PlayFlagReturnedSound(team);
        flagAutoReturnTimer.Stop(team);
        worldService.GameText($"~n~~n~~n~{team.GameText}{team.ColorName} flag returned!", TimeSpan.FromSeconds(5), GameTextStyle.Style3);
        worldService.SendClientMessage(Color.Yellow, message);
    }
}
