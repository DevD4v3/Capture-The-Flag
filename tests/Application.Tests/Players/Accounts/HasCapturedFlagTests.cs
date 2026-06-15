namespace CTF.Application.Tests.Players.Accounts;

public class HasCapturedFlagTests
{
    [SetUp]
    public void Init()
    {
        Team.Alpha.Reset();
        Team.Beta.Reset();
    }

    [Test]
    public void HasCapturedFlag_WhenPlayerIsNotAssignedToAnyTeam_ShouldReturnsFalse()
    {
        // Arrange
        var fakePlayer = new FakePlayer(id: 1, name: "Bob", team: TeamId.NoTeam);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.NoTeam);
        player.SetName(fakePlayer.Name);

        // Act
        bool actual = player.HasCapturedFlag();

        // Assert
        actual.Should().BeFalse();
    }

    [TestCase("Bob")]
    [TestCase("BOB")]
    [TestCase("bob")]
    public void HasCapturedFlag_WhenPlayerFromTheAlphaTeamHasCapturedTheFlagOfTheBetaTeam_ShouldReturnsTrue(string playerName)
    {
        // Arrange
        Team betaTeam = Team.Beta;
        var alphaTeamPlayer = new FakePlayer(id: 1, playerName, team: TeamId.Alpha);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Alpha);
        player.SetName("Bob");
        betaTeam.Flag.Capture(alphaTeamPlayer);

        // Act
        bool actual = player.HasCapturedFlag();

        // Assert
        actual.Should().BeTrue();
    }

    [TestCase("Bob")]
    [TestCase("BOB")]
    [TestCase("bob")]
    public void HasCapturedFlag_WhenPlayerFromTheBetaTeamHasCapturedTheFlagOfTheAlphaTeam_ShouldReturnsTrue(string playerName)
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        var betaTeamPlayer = new FakePlayer(id: 1, playerName, team: TeamId.Beta);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Beta);
        player.SetName("Bob");
        alphaTeam.Flag.Capture(betaTeamPlayer);

        // Act
        bool actual = player.HasCapturedFlag();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void HasCapturedFlag_WhenPlayerFromTheAlphaTeamHasNotCapturedTheFlagOfTheBetaTeam_ShouldReturnsFalse()
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
        bool actual = player.HasCapturedFlag();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void HasCapturedFlag_WhenPlayerFromTheBetaTeamHasNotCapturedTheFlagOfTheAlphaTeam_ShouldReturnsFalse()
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
        bool actual = player.HasCapturedFlag();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void HasCapturedFlag_WhenAlphaTeamFlagIsNotCaptured_ShouldReturnsFalse()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        var betaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: TeamId.Beta);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Beta);
        player.SetName(betaTeamPlayer.Name);

        // Act
        bool actual = player.HasCapturedFlag();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void HasCapturedFlag_WhenBetaTeamFlagIsNotCaptured_ShouldReturnsFalse()
    {
        // Arrange
        Team betaTeam = Team.Beta;
        var alphaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: TeamId.Alpha);
        var player = new PlayerInfo();
        player.SetTeam(TeamId.Alpha);
        player.SetName(alphaTeamPlayer.Name);

        // Act
        bool actual = player.HasCapturedFlag();

        // Assert
        actual.Should().BeFalse();
    }
}
