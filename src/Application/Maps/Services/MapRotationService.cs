namespace CTF.Application.Maps.Services;

public class MapRotationService(
    IServerService serverService,
    IWorldService worldService,
    ITimerService timerService,
    MapInfoService mapInfoService,
    MapTextDrawRenderer mapTextDrawRenderer,
    TeamIconService teamIconService,
    TeamPickupService teamPickupService,
    TeamBalancer teamBalancer,
    FlagAutoReturnTimer flagAutoReturnTimer)
{
    private LoadTime _loadTime;
    private TimerReference _timerReference;
    private readonly TimeLeft _timeLeft = new();
    public TimeLeft TimeLeft => _timeLeft;
    private bool _isMapLoading;
    public bool IsMapLoading() => _isMapLoading;

    public delegate void LoadingMapEventHandler();
    public delegate void LoadedMapEventHandler();
    public event LoadingMapEventHandler LoadingMapEvent;
    public event LoadedMapEventHandler LoadedMapEvent;

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
        if(_timeLeft.IsCompleted())
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
        if (Team.Alpha.IsWinner())
            worldService.SendClientMessage(Color.Yellow, Messages.AlphaIsWinner);
        else if(Team.Beta.IsWinner())
            worldService.SendClientMessage(Color.Yellow, Messages.BetaIsWinner);
        else
            worldService.SendClientMessage(Color.Yellow, Messages.TiedTeams);

        CurrentMap currentMap = mapInfoService.Read();
        serverService.SendRconCommand($"unloadfs {currentMap.Name}");

        IEnumerable<Player> players = AlphaBetaTeamPlayers.GetAll();
        foreach (Player player in players)
            player.ToggleSpectating(true);

        string message = Smart.Format(Messages.NextMapWillBeLoadedSoon, new { currentMap.NextMap.Name });
        worldService.SendClientMessage(Color.Orange, message);
        IMap nextMap = currentMap.NextMap;
        mapInfoService.Load(nextMap);
        Team.Alpha.Flag.RemoveCarrier();
        Team.Beta.Flag.RemoveCarrier();
        teamPickupService.DestroyAllPickups();
        teamPickupService.CreateFlagFromBasePosition(Team.Alpha);
        teamPickupService.CreateFlagFromBasePosition(Team.Beta);
        teamIconService.DestroyAll();
        teamIconService.CreateFromBasePosition(Team.Alpha);
        teamIconService.CreateFromBasePosition(Team.Beta);
        flagAutoReturnTimer.Stop(Team.Alpha);
        flagAutoReturnTimer.Stop(Team.Beta);
        serverService.SendRconCommand($"loadfs {nextMap.Name}");
        serverService.SetMapName(nextMap.Name);
    }

    private void OnLoadedMap()
    {
        _isMapLoading = false;
        LoadedMapEvent?.Invoke();
        TimeLeft.Reset();
        CurrentMap currentMap = mapInfoService.Read();
        string message = Smart.Format(Messages.MapSuccessfullyLoaded, new { currentMap.Name });
        worldService.SendClientMessage(Color.Orange, message);
        static void HandlePlayerAction(Player player, PlayerInfo playerInfo)
        {
            playerInfo.StatsPerRound.ResetStats();
            player.ToggleControllable(true);
            player.Health = 100;
            player.Color = playerInfo.Team.ColorHex;
            player.SetScore(0);
            player.ToggleSpectating(false);
        }
        teamBalancer.Balance(action: HandlePlayerAction);
        worldService.SetWeather(currentMap.Weather);
        serverService.SetWorldTime(currentMap.WorldTime);
        mapTextDrawRenderer.UpdateMapName(currentMap);
    }
}
