namespace CTF.Application.Teams.Flags.Systems;

public class FlagSystem(
    IDictionary<FlagStatus, IFlagEvent> flagEvents,
    PlayerStatsRenderer playerStatsRenderer,
    OnFlagDropped onFlagDropped) : ISystem
{
    [Event]
    public void OnPlayerDisconnect(Player player, DisconnectReason reason)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        if (playerInfo.IsCarryingEnemyFlag())
        {
            Team currentTeam = playerInfo.Team;
            onFlagDropped.Handle(currentTeam.RivalTeam, player);
        }
    }

    [Event]
    public void OnPlayerDeath(Player victim, Player killer, Weapon reason)
    {
        PlayerInfo victimInfo = victim.GetRequiredInfo();
        if (victimInfo.IsCarryingEnemyFlag())
        {
            Team currentTeam = victimInfo.Team;
            onFlagDropped.Handle(currentTeam.RivalTeam, victim);
            if (killer.IsValidPlayer())
            {
                PlayerInfo killerInfo = killer.GetRequiredInfo();
                killerInfo.StatsPerRound.AddCoins(4);
                killer.AddHealth(10);
                killer.AddScore(2);
                playerStatsRenderer.UpdateTextDraw(killer);
            }
        }
    }

    [Event]
    public void OnPlayerPickUpPickup(Player player, Pickup pickup)
    {
        if (pickup.Model == (int)FlagModel.Red)
        {
            FlagStatus flagStatus = Team.Alpha.HandleFlagInteraction(flagPicker: player);
            IFlagEvent flagEvent = flagEvents[flagStatus];
            flagEvent.Handle(Team.Alpha, player);
        }
        else if (pickup.Model == (int)FlagModel.Blue)
        {
            FlagStatus flagStatus = Team.Beta.HandleFlagInteraction(flagPicker: player);
            IFlagEvent flagEvent = flagEvents[flagStatus];
            flagEvent.Handle(Team.Beta, player);
        }
        else if (pickup.Model == (int)ExteriorMarker.Red)
        {
            if (player.Team == (int)TeamId.Alpha)
                player.GameText(Messages.RedFlagIsNotAtBasePosition, TimeSpan.FromSeconds(5), GameTextStyle.Style3);
        }
        else if (pickup.Model == (int)ExteriorMarker.Blue)
        {
            if (player.Team == (int)TeamId.Beta)
                player.GameText(Messages.BlueFlagIsNotAtBasePosition, TimeSpan.FromSeconds(5), GameTextStyle.Style3);
        }
    }
}
