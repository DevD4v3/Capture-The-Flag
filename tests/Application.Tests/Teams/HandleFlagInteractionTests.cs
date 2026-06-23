namespace CTF.Application.Tests.Teams;

public class HandleFlagInteractionTests
{
    [SetUp]
    public void Init()
    {
        Team.Alpha.Reset();
        Team.Beta.Reset();
    }

    [Test]
    public void HandleFlagInteraction_WhenArgumentIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        Player flagPicker = default;

        // Act
        Action act = () => alphaTeam.HandleFlagInteraction(flagPicker);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void HandleFlagInteraction_WhenAlphaTeamCapturesTheFlagOfTheBetaTeam_ShouldReturnCapturedStatus()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        var alphaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: alphaTeam.Id);
        var expectedStatus = FlagStatus.Captured;

        // Act
        FlagStatus actual = betaTeam.HandleFlagInteraction(flagPicker: alphaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        betaTeam.Flag.Status.Should().Be(expectedStatus);
        betaTeam.Flag.Carrier.Should().Be(alphaTeamPlayer);
    }

    [Test]
    public void HandleFlagInteraction_WhenBetaTeamCapturesTheFlagOfTheAlphaTeam_ShouldReturnCapturedStatus()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        var betaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: betaTeam.Id);
        var expectedStatus = FlagStatus.Captured;

        // Act
        FlagStatus actual = alphaTeam.HandleFlagInteraction(flagPicker: betaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        alphaTeam.Flag.Status.Should().Be(expectedStatus);
        alphaTeam.Flag.Carrier.Should().Be(betaTeamPlayer);
    }

    [Test]
    public void HandleFlagInteraction_WhenPlayerFromAlphaTeamBroughtTheFlagOfTheBetaTeamToTheirOwnBase_ShouldReturnBroughtStatus()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        var alphaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: alphaTeam.Id);
        var expectedStatus = FlagStatus.Brought;
        int expectedScore = 1;
        betaTeam.Flag.Capture(alphaTeamPlayer);

        // Act
        FlagStatus actual = alphaTeam.HandleFlagInteraction(flagPicker: alphaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        betaTeam.Flag.Carrier.Should().BeNull();
        betaTeam.Flag.Status.Should().Be(FlagStatus.BasePosition);
        alphaTeam.StatsPerRound.Score.Should().Be(expectedScore);
    }

    [Test]
    public void HandleFlagInteraction_WhenPlayerFromBetaTeamBroughtTheFlagOfTheAlphaTeamToTheirOwnBase_ShouldReturnBroughtStatus()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        var betaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: betaTeam.Id);
        var expectedStatus = FlagStatus.Brought;
        int expectedScore = 1;
        alphaTeam.Flag.Capture(betaTeamPlayer);

        // Act
        FlagStatus actual = betaTeam.HandleFlagInteraction(flagPicker: betaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        alphaTeam.Flag.Carrier.Should().BeNull();
        alphaTeam.Flag.Status.Should().Be(FlagStatus.BasePosition);
        betaTeam.StatsPerRound.Score.Should().Be(expectedScore);
    }

    [Test]
    public void HandleFlagInteraction_WhenPlayerFromAlphaTeamAttemptsToCaptureTheFlagOfTheirTeamFromBase_ShouldReturnBasePositionStatus()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        var alphaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: alphaTeam.Id);
        var expectedStatus = FlagStatus.BasePosition;

        // Act
        FlagStatus actual = alphaTeam.HandleFlagInteraction(flagPicker: alphaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        alphaTeam.Flag.Carrier.Should().BeNull();
        alphaTeam.Flag.Status.Should().Be(expectedStatus);
    }

    [Test]
    public void HandleFlagInteraction_WhenPlayerFromBetaTeamAttemptsToCaptureTheFlagOfTheirTeamFromBase_ShouldReturnBasePositionStatus()
    {
        // Arrange
        Team betaTeam = Team.Beta;
        var betaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: betaTeam.Id);
        var expectedStatus = FlagStatus.BasePosition;

        // Act
        FlagStatus actual = betaTeam.HandleFlagInteraction(flagPicker: betaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        betaTeam.Flag.Carrier.Should().BeNull();
        betaTeam.Flag.Status.Should().Be(expectedStatus);
    }

    [Test]
    public void HandleFlagInteraction_WhenPlayerFromAlphaTeamReturnsTheFlagOfTheirOwnTeam_ShouldReturnReturnedStatus()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        var alphaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: alphaTeam.Id);
        var expectedStatus = FlagStatus.Returned;
        alphaTeam.Flag.Drop();

        // Act
        FlagStatus actual = alphaTeam.HandleFlagInteraction(flagPicker: alphaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        alphaTeam.Flag.Status.Should().Be(FlagStatus.BasePosition);
        alphaTeam.Flag.Carrier.Should().BeNull();
    }

    [Test]
    public void HandleFlagInteraction_WhenPlayerFromBetaTeamReturnsTheFlagOfTheirOwnTeam_ShouldReturnReturnedStatus()
    {
        // Arrange
        Team betaTeam = Team.Beta;
        var betaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: betaTeam.Id);
        var expectedStatus = FlagStatus.Returned;
        betaTeam.Flag.Drop();

        // Act
        FlagStatus actual = betaTeam.HandleFlagInteraction(flagPicker: betaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        betaTeam.Flag.Status.Should().Be(FlagStatus.BasePosition);
        betaTeam.Flag.Carrier.Should().BeNull();
    }

    [Test]
    public void HandleFlagInteraction_WhenPlayerFromBetaTeamTakesTheFlagOfTheAlphaTeamFrom_A_PositionOtherThanTheBase_ShouldReturnTakenStatus()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        var betaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: betaTeam.Id);
        var expectedStatus = FlagStatus.Taken;
        alphaTeam.Flag.Drop();

        // Act
        FlagStatus actual = alphaTeam.HandleFlagInteraction(flagPicker: betaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        alphaTeam.Flag.Status.Should().Be(expectedStatus);
        alphaTeam.Flag.Carrier.Should().Be(betaTeamPlayer);
    }

    [Test]
    public void HandleFlagInteraction_WhenPlayerFromAlphaTeamTakesTheFlagOfTheBetaTeamFrom_A_PositionOtherThanTheBase_ShouldReturnTakenStatus()
    {
        // Arrange
        Team alphaTeam = Team.Alpha;
        Team betaTeam = Team.Beta;
        var alphaTeamPlayer = new FakePlayer(id: 1, name: "Bob", team: alphaTeam.Id);
        var expectedStatus = FlagStatus.Taken;
        betaTeam.Flag.Drop();

        // Act
        FlagStatus actual = betaTeam.HandleFlagInteraction(flagPicker: alphaTeamPlayer);

        // Asserts
        actual.Should().Be(expectedStatus);
        betaTeam.Flag.Status.Should().Be(expectedStatus);
        betaTeam.Flag.Carrier.Should().Be(alphaTeamPlayer);
    }
}
