namespace Persistence.Tests.Common;

public class RepositoryManagerTestCases : IEnumerable<DatabaseProvider>
{
    public IEnumerator<DatabaseProvider> GetEnumerator()
    {
        yield return DatabaseProvider.InMemory;
        yield return DatabaseProvider.Sqlite;
        yield return DatabaseProvider.MariaDb;
    }

    IEnumerator IEnumerable.GetEnumerator()
        => this.GetEnumerator();
}
