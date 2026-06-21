namespace CTF.Application.Teams.Flags;

public class FlagStateResetter(
    TeamPickupService teamPickupService,
    TeamIconService teamIconService,
    FlagAutoReturnTimer flagAutoReturnTimer)
{
    public void Reset(Team firstTeam, Team secondTeam)
    {
        firstTeam.Flag.Reset();
        secondTeam.Flag.Reset();

        teamPickupService.DestroyAllPickups();
        teamPickupService.CreateFlagFromBasePosition(firstTeam);
        teamPickupService.CreateFlagFromBasePosition(secondTeam);

        teamIconService.DestroyAll();
        teamIconService.CreateFromBasePosition(firstTeam);
        teamIconService.CreateFromBasePosition(secondTeam);

        flagAutoReturnTimer.Stop(firstTeam);
        flagAutoReturnTimer.Stop(secondTeam);
    }
}
