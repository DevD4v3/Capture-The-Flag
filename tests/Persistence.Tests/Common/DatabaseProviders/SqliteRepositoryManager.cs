namespace Persistence.Tests.Common.DatabaseProviders;

public class SqliteRepositoryManager : IRepositoryManager
{
    private readonly ISqlCollection _seedSqlCollection;
    private readonly ServiceProvider _serviceProvider;

    public IPlayerRepository PlayerRepository { get; }
    public ITopPlayersRepository TopPlayersRepository { get; }
    public SqliteRepositoryManager()
    {
        var services = new ServiceCollection();
        IConfiguration configuration = EnvConfigurationBuilder.Instance;
        services.AddSingleton(new TopPlayersSettings());
        services.AddSingleton<IPasswordHasher, FakePasswordHasher>();
        services.AddPersistenceSQLiteServices(configuration, TestPaths.Sql);
        _serviceProvider = services.BuildServiceProvider();

        var sqlFile = Path.Combine(
            TestPaths.Sql, 
            typeof(PersistenceSQLiteServicesExtensions).Namespace, 
            "sql",
            "seed_data.sql"
        );

        _seedSqlCollection = new YeSqlLoader()
            .LoadFromFiles(sqlFile);

        PlayerRepository = _serviceProvider.GetRequiredService<IPlayerRepository>();
        TopPlayersRepository = _serviceProvider.GetRequiredService<ITopPlayersRepository>();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
        GC.SuppressFinalize(this);
    }

    public void InitializeSeedData() => ExecuteCommand("InitializeSeedData");
    public void RemoveSeedData() => ExecuteCommand("RemoveSeedData");

    private void ExecuteCommand(string tagName)
    {
        var settings = _serviceProvider.GetRequiredService<SQLiteSettings>();
        using var connection = new SqliteConnection(settings.ConnectionString);
        connection.Open();
        connection.CreateRegexpFunction();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = _seedSqlCollection[tagName];
        command.ExecuteNonQuery();
    }
}
