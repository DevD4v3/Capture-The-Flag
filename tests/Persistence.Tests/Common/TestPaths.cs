namespace Persistence.Tests.Common;

public class TestPaths
{
    public static string Sql =>
        Path.Combine(
            Directory.GetCurrentDirectory(),
            "yesql");
}
