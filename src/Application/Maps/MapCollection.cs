namespace CTF.Application.Maps;

public class MapCollection
{
    private static Map[] s_maps;
    static MapCollection() => SetMapNamesFromFileSystem();
    private static void SetMapNamesFromFileSystem()
    {
        var path = GameModePaths.Maps;
        var random = new Random();
        string[] names = Directory.GetFiles(path);
        random.Shuffle(names);
        s_maps = new Map[names.Length];
        for (int i = 0; i < names.Length; i++)
        {
            var map = new Map 
            { 
                Id = i, 
                Name = Path.GetFileNameWithoutExtension(names[i])
            };
            s_maps[i] = map;
        }
    }

    public static int Count => s_maps.Length;
    public static IReadOnlyList<IMap> GetAll() => s_maps;
    public static IEnumerable<IMap> GetAll(string findBy)
    {
        foreach(Map map in s_maps) 
        { 
            if (map.Name.StartsWith(findBy, StringComparison.OrdinalIgnoreCase))
                yield return map;
        }
    }

    public static Result<IMap> GetById(int id)
    {
        if (id < 0 || id >= Count)
            return Result<IMap>.Failure(Messages.InvalidMap);

        Map map = s_maps[id];
        return Result<IMap>.Success(map);
    }

    public static Result<IMap> GetByName(string mapName)
    {
        Map map = s_maps
            .FirstOrDefault(map => map.Name.Equals(mapName, StringComparison.OrdinalIgnoreCase));
        return map is null ?
            Result<IMap>.Failure(Messages.MapNotFound) :
            Result<IMap>.Success(map);
    }

    public static IMap GetNext(IMap current)
    {
        int nextMapId = (current.Id + 1) % Count;
        return GetById(nextMapId).Value;
    }

    private class Map : IMap
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
}
