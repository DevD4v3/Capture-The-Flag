namespace Persistence.Tests.Common;

public class RepositoryManagerFactory
{
    public static IRepositoryManager Create(DatabaseProvider provider) => provider switch
    {
        DatabaseProvider.InMemory => new InMemoryRepositoryManager(),
        DatabaseProvider.MariaDb => new MariaDbRepositoryManager(),
        DatabaseProvider.Sqlite => new SqliteRepositoryManager(),
        _ => throw new NotSupportedException($"'{provider}' was not found as a database provider.")
    };
}
