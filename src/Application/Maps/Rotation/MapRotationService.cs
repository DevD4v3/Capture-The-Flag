namespace CTF.Application.Maps.Rotation;

public class MapRotationService(
    IServerService serverService,
    IMapObjectService mapObjects,
    IWorldService worldService,
    ITimerService timerService,
    MapInfoService mapInfoService,
    MapCollection mapCollection,
    MapTextDrawRenderer mapTextDrawRenderer,
    FlagStateResetter flagStateResetter,
    TeamBalancer teamBalancer)
{
    private LoadTime _loadTime;
    private TimerReference _timerReference;
    private bool _isMapLoading;
    private IMap _forcedNextMap;
    private readonly TimeLeft _timeLeft = new();
    public TimeLeft TimeLeft => _timeLeft;
    public bool IsMapLoading => _isMapLoading;
    public IMap NextMap => _forcedNextMap ?? mapCollection.GetNext(mapInfoService.CurrentMap);

    public delegate void LoadingMapEventHandler();
    public delegate void LoadedMapEventHandler();
    public event LoadingMapEventHandler LoadingMapEvent;
    public event LoadedMapEventHandler LoadedMapEvent;

    public void ForceNextMap(IMap map)
    {
        ArgumentNullException.ThrowIfNull(map);
        _forcedNextMap = map;
    }

    public void StartRotationTimer()
    {
        _loadTime ??= new LoadTime(OnLoadingMap, OnLoadedMap);
        _timerReference ??= timerService.Start(action: OnTimer, interval: TimeSpan.FromMilliseconds(1000));
    }

    public void StopRotationTimer()
    {
        if (_timerReference is null)
            return;

        timerService.Stop(_timerReference);
        _timerReference = default;
    }

    private void OnTimer(IServiceProvider serviceProvider)
    {
        if (_timeLeft.IsCompleted())
        {
            _loadTime.Decrease();
            mapTextDrawRenderer.UpdateLoadTime(_loadTime);
            return;
        }

        _timeLeft.Decrease();
        mapTextDrawRenderer.UpdateTimeLeft(_timeLeft);
    }

    private void OnLoadingMap()
    {
        _isMapLoading = true;
        LoadingMapEvent?.Invoke();
        var matchResult = MatchResult.Create(Team.Alpha, Team.Beta);
        worldService.SendClientMessage(Color.Yellow, matchResult.Announcement);
        mapObjects.Unload();

        IEnumerable<Player> players = MatchPlayers.GetAll();
        foreach (Player player in players)
            player.ToggleSpectating(true);

        IMap nextMap = NextMap;
        string message = Smart.Format(Messages.NextMapWillBeLoadedSoon, new { nextMap.Name });
        worldService.SendClientMessage(Color.Orange, message);
        mapInfoService.Load(nextMap);
        flagStateResetter.Reset(Team.Alpha, Team.Beta);
        mapObjects.Load(nextMap.Name);
        serverService.SetMapName(nextMap.Name);
    }

    private void OnLoadedMap()
    {
        _isMapLoading = false;
        _forcedNextMap = default;
        LoadedMapEvent?.Invoke();
        TimeLeft.Reset();
        CurrentMap currentMap = mapInfoService.CurrentMap;
        string message = Smart.Format(Messages.MapSuccessfullyLoaded, new { currentMap.Name });
        worldService.SendClientMessage(Color.Orange, message);
        static void PreparePlayerForRound(Player player, PlayerInfo playerInfo)
        {
            playerInfo.StatsPerRound.ResetStats();
            player.ToggleControllable(true);
            player.Health = 100;
            player.Color = playerInfo.Team.ColorHex;
            player.SetScore(0);
            player.ToggleSpectating(false);
        }
        teamBalancer.Balance(Team.Alpha, Team.Beta, onPlayerAssigned: PreparePlayerForRound);
        worldService.SetWeather(currentMap.Weather);
        serverService.SetWorldTime(currentMap.WorldTime);
        mapTextDrawRenderer.UpdateMapName(currentMap);
    }
}
