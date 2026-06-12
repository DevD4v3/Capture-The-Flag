namespace CTF.Application.Maps;

public class MapCollection
{
    private Map[] _maps;

    public MapCollection(string path)
    {
        LoadFromDirectory(path);
    }

    public int Count => _maps.Length;
    public IReadOnlyList<IMap> GetAll() => _maps;
    public IEnumerable<IMap> GetAll(string findBy)
    {
        foreach(Map map in _maps) 
        { 
            if (map.Name.StartsWith(findBy, StringComparison.OrdinalIgnoreCase))
                yield return map;
        }
    }

    public Result<IMap> GetById(int id)
    {
        if (id < 0 || id >= Count)
            return Result<IMap>.Failure(Messages.InvalidMap);

        Map map = _maps[id];
        return Result<IMap>.Success(map);
    }

    public Result<IMap> GetByName(string mapName)
    {
        Map map = _maps
            .FirstOrDefault(map => map.Name.Equals(mapName, StringComparison.OrdinalIgnoreCase));
        return map is null ?
            Result<IMap>.Failure(Messages.MapNotFound) :
            Result<IMap>.Success(map);
    }

    public IMap GetNext(IMap current)
    {
        int nextMapId = (current.Id + 1) % Count;
        return GetById(nextMapId).Value;
    }

    private class Map : IMap
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }

    private void LoadFromDirectory(string path)
    {
        var random = new Random();
        string[] names = Directory.GetFiles(path);
        random.Shuffle(names);
        _maps = new Map[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            var map = new Map
            {
                Id = i,
                Name = Path.GetFileNameWithoutExtension(names[i])
            };
            _maps[i] = map;
        }
    }
}
