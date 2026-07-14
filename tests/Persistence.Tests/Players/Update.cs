namespace Persistence.Tests.Players;

public class UpdatePlayer
{
    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdatePlayerName(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var oldName = "Moderator_Player";
        var newName = "Player1";
        PlayerInfo playerInfo = playerRepository.GetOrDefault(oldName);
        playerInfo.SetName(newName);

        // Act
        playerRepository.UpdateName(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(newName);

        // Assert
        actual.Name.Should().Be(newName);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdatePlayerPassword(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        var expectedPassword = "D123456$";
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.SetPassword(expectedPassword);

        // Act
        playerRepository.UpdatePassword(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.Password.Should().Be(expectedPassword);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateTotalKills(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedTotalKills = 20;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.SetTotalKills(expectedTotalKills);

        // Act
        playerRepository.UpdateTotalKills(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.TotalKills.Should().Be(expectedTotalKills);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateTotalDeaths(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedTotalDeaths = 100;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.SetTotalDeaths(expectedTotalDeaths);

        // Act
        playerRepository.UpdateTotalDeaths(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.TotalDeaths.Should().Be(expectedTotalDeaths);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateMaxKillingSpree(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedKillingSpree = 25;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.SetMaxKillingSpree(expectedKillingSpree);

        // Act
        playerRepository.UpdateMaxKillingSpree(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.MaxKillingSpree.Should().Be(expectedKillingSpree);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateBroughtFlags(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedBroughtFlags = 2;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.AddBroughtFlags();
        playerInfo.AddBroughtFlags();

        // Act
        playerRepository.UpdateBroughtFlags(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.BroughtFlags.Should().Be(expectedBroughtFlags);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateCapturedFlags(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedCapturedFlags = 2;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.AddCapturedFlags();
        playerInfo.AddCapturedFlags();

        // Act
        playerRepository.UpdateCapturedFlags(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.CapturedFlags.Should().Be(expectedCapturedFlags);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateDroppedFlags(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedDroppedFlags = 2;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.AddDroppedFlags();
        playerInfo.AddDroppedFlags();

        // Act
        playerRepository.UpdateDroppedFlags(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.DroppedFlags.Should().Be(expectedDroppedFlags);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateReturnedFlags(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedReturnedFlags = 2;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.AddReturnedFlags();
        playerInfo.AddReturnedFlags();

        // Act
        playerRepository.UpdateReturnedFlags(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.ReturnedFlags.Should().Be(expectedReturnedFlags);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateHeadShots(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedHeadShots = 2;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.AddHeadShots();
        playerInfo.AddHeadShots();

        // Act
        playerRepository.UpdateHeadShots(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.HeadShots.Should().Be(expectedHeadShots);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateGunGameWins(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedGunGameWins = 2;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.AddGunGameWins();
        playerInfo.AddGunGameWins();

        // Act
        playerRepository.UpdateGunGameWins(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.GunGameWins.Should().Be(expectedGunGameWins);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateRole(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        RoleId expectedRoleId = RoleId.Admin;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.SetRole(expectedRoleId);

        // Act
        playerRepository.UpdateRole(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.RoleId.Should().Be(expectedRoleId);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateSkin(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        int expectedSkinId = 100;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.SetSkin(expectedSkinId);

        // Act
        playerRepository.UpdateSkin(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.SkinId.Should().Be(expectedSkinId);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateRank(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        RankId expectedRankId = RankId.GameMaster;
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.SetRank(expectedRankId);

        // Act
        playerRepository.UpdateRank(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.RankId.Should().Be(expectedRankId);
    }

    [TestCaseSource(typeof(RepositoryManagerTestCases))]
    public void ShouldUpdateLastConnection(DatabaseProvider provider)
    {
        // Arrange
        using IRepositoryManager repositoryManager = RepositoryManagerFactory.Create(provider);
        repositoryManager.InitializeSeedData();
        IPlayerRepository playerRepository = repositoryManager.PlayerRepository;
        var playerName = "Moderator_Player";
        PlayerInfo playerInfo = playerRepository.GetOrDefault(playerName);
        playerInfo.SetLastConnection();

        // Act
        playerRepository.UpdateLastConnection(playerInfo);
        PlayerInfo actual = playerRepository.GetOrDefault(playerName);

        // Assert
        actual.LastConnection.Should().BeSameDateAs(playerInfo.LastConnection);
    }
}
