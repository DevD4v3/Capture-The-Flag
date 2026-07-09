namespace CTF.Application.Teams.Flags.Carriers;

/// <summary>
/// A system that handles the pause logic for flag carriers.
/// </summary>
/// <remarks>
/// It checks if the carrier is paused and updates the timer. If the timer runs out, the flag is returned to the base.
/// </remarks>
public class FlagCarrierPauseHandler(
    IWorldService worldService,
    ITimerService timerService,
    TeamPickupService teamPickupService,
    FlagCarrierSettings flagCarrierSettings) : ISystem
{
    [Event]
    public void OnPlayerDisconnect(Player player, DisconnectReason reason)
    {
        var pauseTimerReference = player.GetComponent<PauseTimerReference>();
        if (pauseTimerReference is null)
            return;

        timerService.Stop(pauseTimerReference.Value);
    }

    [Event]
    public void OnPlayerPauseStateChange(Player player, bool pauseState)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        if (pauseState && playerInfo.IsCarryingEnemyFlag())
        {
            var interval = TimeSpan.FromSeconds(flagCarrierSettings.PauseTime);
            var timerReference = timerService.Start(OnComplete, interval);
            player.AddComponent<PauseTimerReference>(timerReference);
        }
        else if (!pauseState)
        {
            var pauseTimerReference = player.GetComponent<PauseTimerReference>();
            if (pauseTimerReference is null)
                return;

            timerService.Stop(pauseTimerReference.Value);
            pauseTimerReference.Destroy();
        }

        void OnComplete(IServiceProvider serviceProvider)
        {
            if (!player.IsComponentAlive)
                return;

            var pauseTimerReference = player.GetComponent<PauseTimerReference>();
            timerService.Stop(pauseTimerReference.Value);
            pauseTimerReference.Destroy();

            if (!playerInfo.IsCarryingEnemyFlag())
                return;

            Team rivalTeam = playerInfo.Team.RivalTeam;
            rivalTeam.Flag.ReturnToBase();
            player.HideOnRadarMap();
            teamPickupService.CreateFlagFromBasePosition(rivalTeam);
            teamPickupService.DestroyExteriorMarker(rivalTeam);
            rivalTeam.Sounds.PlayFlagReturnedSound();
            var message = Smart.Format(Messages.FlagAutoReturn2, new
            {
                rivalTeam.ColorName,
                PlayerName = player.Name,
                Seconds = flagCarrierSettings.PauseTime
            });
            worldService.SendClientMessage(rivalTeam.ColorHex, message);
            worldService.GameText($"~n~~n~~n~{rivalTeam.GameText}{rivalTeam.ColorName} flag returned!", TimeSpan.FromSeconds(5), GameTextStyle.Style3);
        }
    }

    private class PauseTimerReference(TimerReference value) : Component
    {
        public TimerReference Value { get; } = value;
    }
}
