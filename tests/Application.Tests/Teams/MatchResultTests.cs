namespace CTF.Application.Tests.Teams;

public class MatchResultTests
{
    [SetUp]
    public void Init()
    {
        Team.Alpha.Reset();
        Team.Beta.Reset();
    }

    [Test]
    public void Create_WhenAlphaTeamWins_ShouldReturnAlphaAsWinner()
    {
        // Arrange
        Team.Alpha.StatsPerRound.AddScore();

        // Act
        MatchResult result = MatchResult.Create(Team.Alpha, Team.Beta);

        // Assert
        result.Winner.Should().Be(Team.Alpha);
        result.IsTie.Should().BeFalse();
        result.Announcement.Should().Be(Messages.AlphaIsWinner);
    }

    [Test]
    public void Create_WhenBetaTeamWins_ShouldReturnBetaAsWinner()
    {
        // Arrange
        Team.Beta.StatsPerRound.AddScore();

        // Act
        MatchResult result = MatchResult.Create(Team.Alpha, Team.Beta);

        // Assert
        result.Winner.Should().Be(Team.Beta);
        result.IsTie.Should().BeFalse();
        result.Announcement.Should().Be(Messages.BetaIsWinner);
    }

    [Test]
    public void Create_WhenNoTeamWins_ShouldReturnTieResult()
    {
        // Arrange
        Team.Alpha.StatsPerRound.AddScore();
        Team.Beta.StatsPerRound.AddScore();

        // Act
        MatchResult result = MatchResult.Create(Team.Alpha, Team.Beta);

        // Assert
        result.Winner.Should().Be(Team.None);
        result.IsTie.Should().BeTrue();
        result.Announcement.Should().Be(Messages.TiedTeams);
    }
}
