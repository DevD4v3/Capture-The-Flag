namespace CTF.Application.Teams.Flags;

public class FlagStateResetter(
    TeamPickupService teamPickupService,
    TeamIconService teamIconService,
    FlagAutoReturnTimer flagAutoReturnTimer)
{
    public void Reset(Team alpha, Team beta)
    {
        alpha.Flag.Reset();
        beta.Flag.Reset();

        teamPickupService.DestroyAllPickups();
        teamPickupService.CreateFlagFromBasePosition(alpha);
        teamPickupService.CreateFlagFromBasePosition(beta);

        teamIconService.DestroyAll();
        teamIconService.CreateFromBasePosition(alpha);
        teamIconService.CreateFromBasePosition(beta);

        flagAutoReturnTimer.Stop(alpha);
        flagAutoReturnTimer.Stop(beta);
    }
}
