namespace Persistence.Tests.Common.DatabaseProviders;

public class MariaDbRepositoryManager : IRepositoryManager
{
    private readonly ServiceProvider _serviceProvider;
    public IPlayerRepository PlayerRepository { get; }
    public ITopPlayersRepository TopPlayersRepository { get; }
    public MariaDbRepositoryManager()
    {
        var services = new ServiceCollection();
        IConfiguration configuration = EnvConfigurationBuilder.Instance;
        services.AddSingleton(new TopPlayersSettings());
        services.AddSingleton<IPasswordHasher, FakePasswordHasher>();
        services.AddPersistenceMariaDBServices(configuration, TestPaths.Sql);
        _serviceProvider = services.BuildServiceProvider();
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
        var settings = _serviceProvider.GetRequiredService<MariaDbSettings>();
        var sqlCollection = _serviceProvider.GetRequiredService<ISqlCollection>();
        using var connection = new MySqlConnection(settings.ConnectionString);
        connection.Open();
        MySqlCommand command = connection.CreateCommand();
        command.CommandText = sqlCollection[tagName];
        command.ExecuteNonQuery();
    }
}
