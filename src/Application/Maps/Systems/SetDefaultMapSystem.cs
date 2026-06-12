namespace CTF.Application.Maps.Systems;

public class SetDefaultMapSystem(
    IWorldService worldService,
    IServerService serverService,
    MapInfoService mapInfoService,
    TeamPickupService teamPickupService,
    TeamIconService teamIconService,
    MapTextDrawRenderer mapTextDrawRenderer,
    ServerSettings serverSettings) : ISystem
{
    [Event]
    public void OnGameModeInit()
    {
        Result<IMap> mapResult = MapCollection.GetByName(serverSettings.MapName);
        if (mapResult.IsSuccess)
        {
            mapInfoService.Load(mapResult.Value);
        }
        CurrentMap currentMap = mapInfoService.CurrentMap;
        serverService.SetMapName(currentMap.Name);
        serverService.SendRconCommand($"loadfs {currentMap.Name}");
        mapTextDrawRenderer.UpdateMapName(currentMap);

        worldService.SetWeather(currentMap.Weather);
        serverService.SetWorldTime(currentMap.WorldTime);
        teamPickupService.CreateFlagFromBasePosition(Team.Alpha);
        teamPickupService.CreateFlagFromBasePosition(Team.Beta);
        teamIconService.CreateFromBasePosition(Team.Alpha);
        teamIconService.CreateFromBasePosition(Team.Beta);
    }
}
