namespace CTF.Application.Tests.Players.Extensions;

public class InvalidPlayerExtensionsTests
{
    [Test]
    public void IsInvalidPlayer_WhenPlayerIsInvalid_ShouldReturnTrue()
    {
        // Arrange
        Player player = default;

        // Act
        bool actual = player.IsInvalidPlayer();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsInvalidPlayer_WhenPlayerIsValid_ShouldReturnFalse()
    {
        // Arrange
        Player player = new FakePlayer(id: 1, name: "Bob");

        // Act
        bool actual = player.IsInvalidPlayer();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void IsValidPlayer_WhenPlayerIsInvalid_ShouldReturnFalse()
    {
        // Arrange
        Player player = default;

        // Act
        bool actual = player.IsValidPlayer();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void IsValidPlayer_WhenPlayerIsValid_ShouldReturnTrue()
    {
        // Arrange
        Player player = new FakePlayer(id: 1, name: "Bob");

        // Act
        bool actual = player.IsValidPlayer();

        // Assert
        actual.Should().BeTrue();
    }
}
