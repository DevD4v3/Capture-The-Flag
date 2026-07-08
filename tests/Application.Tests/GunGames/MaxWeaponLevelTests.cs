namespace CTF.Application.Tests.GunGames;

public class MaxWeaponLevelTests
{
    [Test]
    public void Constructor_WhenValueIsGreaterThanZero_ShouldCreateMaxWeaponLevel()
    {
        // Arrange
        const int expectedValue = 5;

        // Act
        var maxWeaponLevel = new MaxWeaponLevel(expectedValue);

        // Assert
        maxWeaponLevel.Value.Should().Be(expectedValue);
    }

    [Test]
    public void Constructor_WhenValueIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        const int invalidValue = 0;

        // Act
        Action act = () => _ = new MaxWeaponLevel(invalidValue);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
