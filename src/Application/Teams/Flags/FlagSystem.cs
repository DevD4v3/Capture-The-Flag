namespace CTF.Application.Teams.Flags;

public class FlagSystem(
    IWorldService worldService,
    IDictionary<FlagStatus, IFlagEvent> flagEvents,
    TeamPickupService teamPickupService,
    FlagAutoReturnTimer flagAutoReturnTimer,
    PlayerStatsRenderer playerStatsRenderer) : ISystem
{
    [Event]
    public void OnPlayerDisconnect(Player player, DisconnectReason reason)
    {
        PlayerInfo playerInfo = player.GetRequiredInfo();
        if (playerInfo.IsCarryingEnemyFlag())
        {
            Team currentTeam = playerInfo.Team;
            IFlagEvent flagDropped = flagEvents[FlagStatus.Dropped];
            flagDropped.Handle(currentTeam.RivalTeam, player);
        }
    }

    [Event]
    public void OnPlayerDeath(Player victim, Player killer, Weapon reason)
    {
        PlayerInfo victimInfo = victim.GetRequiredInfo();
        if (victimInfo.IsCarryingEnemyFlag())
        {
            Team currentTeam = victimInfo.Team;
            IFlagEvent flagDropped = flagEvents[FlagStatus.Dropped];
            flagDropped.Handle(currentTeam.RivalTeam, victim);
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
