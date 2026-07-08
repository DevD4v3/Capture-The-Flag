namespace CTF.Application.Tests.GunGames;

public class KillsRequiredPerLevelTests
{
    [Test]
    public void Constructor_WhenValueIsGreaterThanZero_ShouldCreateKillsRequiredPerLevel()
    {
        // Arrange
        const int expectedValue = 2;

        // Act
        var killsRequired = new KillsRequiredPerLevel(expectedValue);

        // Assert
        killsRequired.Value.Should().Be(expectedValue);
    }

    [Test]
    public void Constructor_WhenValueIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        const int invalidValue = 0;

        // Act
        Action act = () => _ = new KillsRequiredPerLevel(invalidValue);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
