namespace CTF.Application.Common.Paths;

public static class GameModePaths
{
    public static string Root =>
        Path.Combine(
            Directory.GetCurrentDirectory(),
            "gamemode");

    public static string Maps =>
        Path.Combine(
            Root,
            "Maps",
            "Files");

    public static string Sql =>
        Path.Combine(
            Root,
            "yesql");
}
