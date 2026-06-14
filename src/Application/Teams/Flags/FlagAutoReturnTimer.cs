namespace CTF.Application.Teams.Flags;

/// <summary>
/// A timer service that automatically returns the flag to its base if it is not picked up by a player within a certain time limit.
/// </summary>
public class FlagAutoReturnTimer(
    ITimerService timerService,
    IWorldService worldService,
    TeamPickupService teamPickupService,
    TeamSoundsService teamSoundsService,
    FlagAutoReturnSettings flagAutoReturnSettings)
{
    private TimerReference _alphaTeamTimer;
    private TimerReference _betaTeamTimer;

    public void Start(Team team)
    {
        void OnComplete(IServiceProvider serviceProvider)
        {
            teamPickupService.CreateFlagFromBasePosition(team);
            teamPickupService.DestroyExteriorMarker(team);
            teamSoundsService.PlayFlagReturnedSound(team);
            team.IsFlagAtBasePosition = true;
            var message = Smart.Format(Messages.FlagAutoReturn, new
            {
                Seconds = flagAutoReturnSettings.Delay,
                team.ColorName
            });
            worldService.SendClientMessage(team.ColorHex, message);
            worldService.GameText($"~n~~n~~n~{team.GameText}{team.ColorName} flag returned!", TimeSpan.FromSeconds(5), GameTextStyle.Style3);
            Stop(team);
        }

        if (team.Id == TeamId.Alpha)
        {
            TimeSpan interval = TimeSpan.FromSeconds(flagAutoReturnSettings.Delay);
            _alphaTeamTimer ??= timerService.Start(OnComplete, interval);
        }
        else if (team.Id == TeamId.Beta)
        {
            TimeSpan interval = TimeSpan.FromSeconds(flagAutoReturnSettings.Delay);
            _betaTeamTimer ??= timerService.Start(OnComplete, interval);
        }
    }

    public void Stop(Team team) 
    { 
        if (team.Id == TeamId.Alpha && _alphaTeamTimer is not null) 
        {
            timerService.Stop(_alphaTeamTimer);
            _alphaTeamTimer = default;
        }
        else if (team.Id == TeamId.Beta && _betaTeamTimer is not null)
        {
            timerService.Stop(_betaTeamTimer);
            _betaTeamTimer = default;
        }
    }
}
