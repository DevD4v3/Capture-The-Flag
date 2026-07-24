namespace CTF.Application.Teams;

public class TeamPickupService
{
    private readonly MapInfoService _mapInfoService;
    private readonly IWorldService _worldService;
    private Pickup _redFlagPickup;
    private Pickup _blueFlagPickup;
    private Pickup _redExteriorMarker;
    private Pickup _blueExteriorMarker;

    public TeamPickupService(MapInfoService mapInfoService, IWorldService worldService)
    {
        _mapInfoService = mapInfoService;
        _worldService = worldService;
        CreateFlagFromBasePosition(Team.Alpha);
        CreateFlagFromBasePosition(Team.Beta);
    }

    public void CreateFlagFromBasePosition(Team team)
    {
        ArgumentNullException.ThrowIfNull(team);
        CurrentMap currentMap = _mapInfoService.CurrentMap;
        if (team.Id == TeamId.Alpha)
        {
            CreateFlagFromVector3(team, currentMap.FlagLocations.Red);
        }
        else if (team.Id == TeamId.Beta)
        {
            CreateFlagFromVector3(team, currentMap.FlagLocations.Blue);
        }
    }

    public void CreateFlagFromVector3(Team team, Vector3 position)
    {
        ArgumentNullException.ThrowIfNull(team);
        DestroyFlag(team);
        if (team.Id == TeamId.Alpha)
        {
            _redFlagPickup = _worldService.CreatePickup(
                model: (int)Team.Alpha.Flag.Model,
                type: PickupType.ScriptedActionsOnlyEveryFewSeconds,
                position
            );
        }
        else if (team.Id == TeamId.Beta)
        {
            _blueFlagPickup = _worldService.CreatePickup(
               model: (int)Team.Beta.Flag.Model,
               type: PickupType.ScriptedActionsOnlyEveryFewSeconds,
               position
            );
        }
    }

    public void DestroyFlag(Team team)
    {
        ArgumentNullException.ThrowIfNull(team);
        if (team.Id == TeamId.Alpha)
        {
            _redFlagPickup?.Destroy();
            _redFlagPickup = default;
        }
        else if (team.Id == TeamId.Beta)
        {
            _blueFlagPickup?.Destroy();
            _blueFlagPickup = default;
        }
    }

    public void DestroyFlags()
    {
        DestroyFlag(Team.Alpha);
        DestroyFlag(Team.Beta);
    }

    public void CreateExteriorMarker(Team team)
    {
        ArgumentNullException.ThrowIfNull(team);
        CurrentMap currentMap = _mapInfoService.CurrentMap;
        DestroyExteriorMarker(team);
        if (team.Id == TeamId.Alpha)
        {
            _redExteriorMarker = _worldService.CreatePickup(
                model: (int)ExteriorMarker.Red,
                type: PickupType.ScriptedActionsOnlyEveryFewSeconds,
                position: currentMap.FlagLocations.Red
            );
        }
        else if (team.Id == TeamId.Beta)
        {
            _blueExteriorMarker = _worldService.CreatePickup(
                model: (int)ExteriorMarker.Blue,
                type: PickupType.ScriptedActionsOnlyEveryFewSeconds,
                position: currentMap.FlagLocations.Blue
            );
        }
    }

    public void DestroyExteriorMarker(Team team)
    {
        ArgumentNullException.ThrowIfNull(team);
        if (team.Id == TeamId.Alpha)
        {
            _redExteriorMarker?.Destroy();
            _redExteriorMarker = default;
        }
        else if (team.Id == TeamId.Beta)
        {
            _blueExteriorMarker?.Destroy();
            _blueExteriorMarker = default;
        }
    }

    public void DestroyAllPickups()
    {
        DestroyExteriorMarker(Team.Alpha);
        DestroyExteriorMarker(Team.Beta);
        DestroyFlags();
    }
}
