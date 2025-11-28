namespace Persistence.Tests.Players;

public class CreatePlayer
{
    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void Create_WhenCalled_ShouldCreatePlayerAndSetAccountId(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerInfo = new PlayerInfo();
        playerInfo.SetName("Player1");
        playerInfo.SetPassword("DSR8887$#");

        // Act
        playerRepository.Create(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerInfo.Name);

        // Asserts
        actual.AccountId.Should().BeGreaterThan(0);
        actual.Name.Should().Be(playerInfo.Name);
        actual.Password.Should().Be(playerInfo.Password);
        actual.RoleId.Should().Be(RoleId.Basic);
        actual.RankId.Should().Be(RankId.Noob);
    }
}
