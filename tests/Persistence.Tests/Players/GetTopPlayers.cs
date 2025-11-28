namespace Persistence.Tests.Players;

public class GetTopPlayers
{
    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void GetByTotalKills_WhenSeedDataIsAvailable_ShouldReturnPlayersOrderedByTotalKills(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        ITopPlayersRepository topPlayersRepository = repositoryManager.TopPlayersRepository;
        Result<MaxTopPlayers> result = MaxTopPlayers.Create(6);
        TopPlayersByTotalKills[] expectedPlayers = 
        [
            new() { PlayerName = "Basic_Player(6)", TotalKills = 251, Rank = RankId.Hitman },
            new() { PlayerName = "Basic_Player(5)", TotalKills = 200, Rank = RankId.Advanced },
            new() { PlayerName = "Basic_Player(7)", TotalKills = 200, Rank = RankId.Advanced },
            new() { PlayerName = "Basic_Player(4)", TotalKills = 170, Rank = RankId.SemiAdvance },
            new() { PlayerName = "Basic_Player(3)", TotalKills = 160, Rank = RankId.SemiAdvance },
            new() { PlayerName = "Basic_Player(2)", TotalKills = 150, Rank = RankId.SemiAdvance }
        ];

        // Act
        TopPlayersByTotalKills[] actual = topPlayersRepository
            .GetByTotalKills(result.Value)
            .ToArray();

        // Assert
        actual.Should().BeEquivalentTo(expectedPlayers);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void GetByTotalKills_WhenSeedDataIsNotAvailable_ShouldReturnEmptyCollection(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.RemoveSeedData();
        ITopPlayersRepository topPlayersRepository = repositoryManager.TopPlayersRepository;
        Result<MaxTopPlayers> result = MaxTopPlayers.Create(6);

        // Act
        TopPlayersByTotalKills[] actual = topPlayersRepository
            .GetByTotalKills(result.Value)
            .ToArray();

        // Assert
        actual.Should().BeEmpty();
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void GetByMaxKillingSpree_WhenSeedDataIsAvailable_ShouldReturnPlayersOrderedByMaxKillingSpree(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        ITopPlayersRepository topPlayersRepository = repositoryManager.TopPlayersRepository;
        Result<MaxTopPlayers> result = MaxTopPlayers.Create(6);
        TopPlayersByMaxKillingSpree[] expectedPlayers =
        [
            new() { PlayerName = "Basic_Player(6)", MaxKillingSpree = 50 },
            new() { PlayerName = "Basic_Player(7)", MaxKillingSpree = 30 },
            new() { PlayerName = "Basic_Player(5)", MaxKillingSpree = 25 },
            new() { PlayerName = "Basic_Player(4)", MaxKillingSpree = 20 },
            new() { PlayerName = "Basic_Player(3)", MaxKillingSpree = 15 },
            new() { PlayerName = "Basic_Player(2)", MaxKillingSpree = 10 }
        ];

        // Act
        TopPlayersByMaxKillingSpree[] actual = topPlayersRepository
            .GetByMaxKillingSpree(result.Value)
            .ToArray();

        // Assert
        actual.Should().BeEquivalentTo(expectedPlayers);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void GetByMaxKillingSpree_WhenSeedDataIsNotAvailable_ShouldReturnEmptyCollection(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.RemoveSeedData();
        ITopPlayersRepository topPlayersRepository = repositoryManager.TopPlayersRepository;
        Result<MaxTopPlayers> result = MaxTopPlayers.Create(6);

        // Act
        TopPlayersByMaxKillingSpree[] actual = topPlayersRepository
            .GetByMaxKillingSpree(result.Value)
            .ToArray();

        // Assert
        actual.Should().BeEmpty();
    }
}
