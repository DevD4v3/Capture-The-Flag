namespace CTF.Application.Maps;

/// <summary>
/// Represents a service to load information from a map.
/// </summary>
public class MapInfoService
{
    private CurrentMap _currentMap;
    private readonly string _mapsPath;

    public MapInfoService(IMap initialMap, string mapsPath)
    {
        _mapsPath = mapsPath;
        Load(initialMap);
    }

    /// <summary>
    /// Gets the current information from a map.
    /// </summary>
    public CurrentMap CurrentMap => _currentMap;

    /// <summary>
    /// Loads map information from the file system.
    /// </summary>
    /// <param name="map">The map to load.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Load(IMap map)
    {
        ArgumentNullException.ThrowIfNull(map);
        var path = Path.Combine(_mapsPath, $"{map.Name}.ini");
        ISectionsData sections = SectionsFile.Load(path);
        SpawnLocation[] alphaTeamLocations = GetSpawnLocations(sections["AlphaTeamLocations"]);
        SpawnLocation[] betaTeamLocations = GetSpawnLocations(sections["BetaTeamLocations"]);
        FlagLocations flagLocations = new()
        {
            Red  = GetFlagLocation(sections["RedFlagLocation"]),
            Blue = GetFlagLocation(sections["BlueFlagLocation"])
        };
        sections.TryGetData(section: "Interior",  out ISectionData retrievedInterior);
        sections.TryGetData(section: "Weather",   out ISectionData retrievedWeather);
        sections.TryGetData(section: "WorldTime", out ISectionData retrievedWorldTime);

        int interior = retrievedInterior is null ? 
            CurrentMap.DefaultInterior : 
            int.Parse(retrievedInterior.First());

        int weather = retrievedWeather is null ?
            CurrentMap.DefaultWeather : 
            int.Parse(retrievedWeather.First());

        int worldTime = retrievedWorldTime is null ?
            CurrentMap.DefaultWorldTime : 
            int.Parse(retrievedWorldTime.First());

        _currentMap = new CurrentMap(
            map, 
            alphaTeamLocations, 
            betaTeamLocations,
            flagLocations,
            interior, 
            weather, 
            worldTime);
    }

    private static SpawnLocation[] GetSpawnLocations(ISectionData section)
    {
        var locations = new SpawnLocation[section.Count];
        for(int i = 0; i < section.Count; i++)
        {
            string data = section[i];
            string[] coordinates = data.Split(',');
            var position = new Vector3(
                float.Parse(coordinates[0], CultureInfo.InvariantCulture),
                float.Parse(coordinates[1], CultureInfo.InvariantCulture),
                float.Parse(coordinates[2], CultureInfo.InvariantCulture)
            );
            float angle = float.Parse(coordinates[3], CultureInfo.InvariantCulture);
            var spawnLocation = new SpawnLocation(position, angle);
            locations[i] = spawnLocation;
        }
        return locations;
    }

    private static Vector3 GetFlagLocation(ISectionData section) 
    {
        string data = section[0];
        string[] coordinates = data.Split(',');
        var position = new Vector3(
            float.Parse(coordinates[0], CultureInfo.InvariantCulture),
            float.Parse(coordinates[1], CultureInfo.InvariantCulture),
            float.Parse(coordinates[2], CultureInfo.InvariantCulture)
        );
        return position;
    }
}
