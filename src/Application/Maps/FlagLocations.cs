namespace CTF.Application.Maps;

public class FlagLocations
{
    public static readonly FlagLocations Empty = new()
    {
        Red  = new Vector3(0f, 0f, 0f),
        Blue = new Vector3(0f, 0f, 0f)
    };

    public required Vector3 Red { get; init; }
    public required Vector3 Blue { get; init; }
}
