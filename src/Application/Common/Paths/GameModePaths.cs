namespace CTF.Application.Common.Paths;

public static class GameModePaths
{
    public static string Maps =>
        Path.Combine(
            Root,
            "Maps",
            "Files");

    public static string Sql =>
        Path.Combine(
            Root,
            "yesql");

    private static string Root
    {
        get
        {
            // open.mp executes the gamemode from the server root directory,
            // while tests execute from the test output directory.
            // Fall back to the current directory when the "gamemode" folder is not present.
            var gameModePath = Path.Combine(Directory.GetCurrentDirectory(), "gamemode");
            return Directory.Exists(gameModePath)
                ? gameModePath
                : Directory.GetCurrentDirectory();
        }
    }
}
