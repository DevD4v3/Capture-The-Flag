namespace CTF.Application.Tests.Players.Accounts;

public class IsCarryingEnemyFlagTests
{
    [SetUp]
    public void Init()
    {
        Team.Alpha.Reset();
        Team.Beta.Reset();
    }

    [Test]
    public void IsCarryingEnemyFlag_WhenPlayerIsNotAssignedToAnyTeam_ShouldReturnFalse()
    {
        // Arrange
        var fakePlayer = new FakePlayer(id: 1, name: "Bob", team: TeamId.NoTeam);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.NoTeam);
        player.SetName(fakePlayer.Name);

        // Act
        bool actual = player.IsCarryingEnemyFlag();

        // Assert
        actual.Should().BeFalse();
    }

    [TestCase("Bob")]
    [TestCase("BOB")]
    [TestCase("bob")]
    public void IsCarryingEnemyFlag_WhenPlayerFromTheAlphaTeamIsCarryingTheBetaFlag_ShouldReturnTrue(string playerName)
    {
        // Arrange
        Team betaTeam = Team.Beta;
        var alphaTeamPlayer = new FakePlayer(id: 1, playerName, team: TeamId.Alpha);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Alpha);
        player.SetName("Bob");
        betaTeam.Flag.Capture(alphaTeamPlayer);

        // Act
        bool actual = player.IsCarryingEnemyFlag();

        // Assert
        actual.Should().BeTrue();
    }

    [TestCase("Bob")]
    [TestCase("BOB")]
    [TestCase("bob")]
    public void IsCarryingEnemyFlag_WhenPlayerFromTheBetaTeamIsCarryingTheAlphaFlag_ShouldReturnTrue(string playerName)
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        var betaTeamPlayer = new FakePlayer(id: 1, playerName, team: TeamId.Beta);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Beta);
        player.SetName("Bob");
        alphaTeam.Flag.Capture(betaTeamPlayer);

        // Act
        bool actual = player.IsCarryingEnemyFlag();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsCarryingEnemyFlag_WhenAnotherPlayerFromTheAlphaTeamIsCarryingTheBetaFlag_ShouldReturnFalse()
    {
        // Arrange
        Team betaTeam = Team.Beta;
        var alphaTeamPlayer1 = new FakePlayer(id: 1, name: "Bob", team: TeamId.Alpha);
        var alphaTeamPlayer2 = new FakePlayer(id: 2, name: "Alice", team: TeamId.Alpha);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Alpha);
        player.SetName(alphaTeamPlayer1.Name);
        betaTeam.Flag.Capture(alphaTeamPlayer2);

        // Act
        bool actual = player.IsCarryingEnemyFlag();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void IsCarryingEnemyFlag_WhenAnotherPlayerFromTheBetaTeamIsCarryingTheAlphaFlag_ShouldReturnFalse()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        var betaTeamPlayer1 = new FakePlayer(id: 1, name: "Bob", team: TeamId.Beta);
        var betaTeamPlayer2 = new FakePlayer(id: 2, name: "Alice", team: TeamId.Beta);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Beta);
        player.SetName(betaTeamPlayer1.Name);
        alphaTeam.Flag.Capture(betaTeamPlayer2);

        // Act
        bool actual = player.IsCarryingEnemyFlag();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void IsCarryingEnemyFlag_WhenPlayerFromTheAlphaTeamTakesADroppedBetaFlag_ShouldReturnTrue()
    {
        // Arrange
        Team betaTeam = Team.Beta;
        var alphaTeamPlayer = new FakePlayer(
            id: 1,
            name: "Bob",
            team: TeamId.Alpha);

        var player = new PlayerInfo();
        player.SetTeam(TeamId.Alpha);
        player.SetName("Bob");
        betaTeam.Flag.Drop();
        betaTeam.Flag.Take(alphaTeamPlayer);

        // Act
        bool actual = player.IsCarryingEnemyFlag();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsCarryingEnemyFlag_WhenPlayerFromTheBetaTeamTakesADroppedAlphaFlag_ShouldReturnTrue()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        var betaTeamPlayer = new FakePlayer(
            id: 1,
            name: "Bob",
            team: TeamId.Beta);

        var player = new PlayerInfo();
        player.SetTeam(TeamId.Beta);
        player.SetName("Bob");
        alphaTeam.Flag.Drop();
        alphaTeam.Flag.Take(betaTeamPlayer);

        // Act
        bool actual = player.IsCarryingEnemyFlag();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsCarryingEnemyFlag_WhenTheAlphaFlagHasNoCarrier_ShouldReturnFalse()
    {
        // Arrange
        var betaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: TeamId.Beta);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Beta);
        player.SetName(betaTeamPlayer.Name);

        // Act
        bool actual = player.IsCarryingEnemyFlag();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void IsCarryingEnemyFlag_WhenTheBetaFlagHasNoCarrier_ShouldReturnFalse()
    {
        // Arrange
        var alphaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: TeamId.Alpha);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Alpha);
        player.SetName(alphaTeamPlayer.Name);

        // Act
        bool actual = player.IsCarryingEnemyFlag();

        // Assert
        actual.Should().BeFalse();
    }
}