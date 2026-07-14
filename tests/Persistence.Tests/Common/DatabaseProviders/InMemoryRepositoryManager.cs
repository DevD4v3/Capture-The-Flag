namespace Persistence.Tests.Common.DatabaseProviders;

public class InMemoryRepositoryManager : IRepositoryManager
{
    private readonly ServiceProvider _serviceProvider;

    public IPlayerRepository PlayerRepository { get; }
    public ITopPlayersRepository TopPlayersRepository { get; }
    public InMemoryRepositoryManager()
    {
        var services = new ServiceCollection();
        services.AddSingleton(new TopPlayersSettings());
        services.AddSingleton<IPasswordHasher, FakePasswordHasher>();
        services.AddPersistenceInMemoryServices();
        _serviceProvider = services.BuildServiceProvider();
        PlayerRepository = _serviceProvider.GetRequiredService<IPlayerRepository>();
        TopPlayersRepository = _serviceProvider.GetRequiredService<ITopPlayersRepository>();
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
        GC.SuppressFinalize(this);
    }

    public void InitializeSeedData()
    {

    }

    public void RemoveSeedData()
    {
        _serviceProvider
            .GetRequiredService<Dictionary<int, FakePlayer>>()
            .Clear();
    }
}
