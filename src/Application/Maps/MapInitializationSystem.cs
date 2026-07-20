namespace CTF.Application.Maps;

public class MapInitializationSystem(
    IWorldService worldService,
    IServerService serverService,
    IMapObjectService mapObjects,
    MapInfoService mapInfoService,
    MapCollection mapCollection,
    TeamPickupService teamPickupService,
    TeamIconService teamIconService,
    MapTextDrawRenderer mapTextDrawRenderer,
    ServerSettings serverSettings) : ISystem
{
    [Event]
    public void OnGameModeInit()
    {
        Result<IMap> mapResult = mapCollection.GetByName(serverSettings.MapName);
        if (mapResult.IsSuccess)
        {
            mapInfoService.Load(mapResult.Value);
        }

        CurrentMap currentMap = mapInfoService.CurrentMap;
        serverService.SetMapName(currentMap.Name);
        mapObjects.Load(currentMap.Name);
        mapTextDrawRenderer.UpdateMapName(currentMap);

        worldService.SetWeather(currentMap.Weather);
        serverService.SetWorldTime(currentMap.WorldTime);
        teamPickupService.CreateFlagFromBasePosition(Team.Alpha);
        teamPickupService.CreateFlagFromBasePosition(Team.Beta);
        teamIconService.CreateFromBasePosition(Team.Alpha);
        teamIconService.CreateFromBasePosition(Team.Beta);
    }
}
