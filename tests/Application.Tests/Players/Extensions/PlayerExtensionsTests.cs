namespace CTF.Application.Tests.Players.Extensions;

public class PlayerExtensionsTests
{
    [Test]
    public void GetInfo_WhenNoAccountComponent_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var fakePlayer = new FakePlayer2();

        // Act
        Action act = () => fakePlayer.GetInfo();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void GetInfo_WhenAccountComponentIsAssigned_ShouldNotThrowInvalidOperationException()
    {
        // Arrange
        var fakePlayer = new FakePlayer3();

        // Act
        Action act = () => fakePlayer.GetInfo();

        // Assert
        act.Should().NotThrow<InvalidOperationException>();
    }

    [Test]
    public void IsUnauthenticated_WhenPlayerIsUnauthenticated_ShouldReturnTrue()
    {
        // Arrange
        var fakePlayer = new FakePlayer3()
        {
            IsAuthenticated = false
        };

        // Act
        bool actual = fakePlayer.IsUnauthenticated();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsUnauthenticated_WhenPlayerIsAuthenticated_ShouldReturnFalse()
    {
        // Arrange
        var fakePlayer = new FakePlayer3()
        {
            IsAuthenticated = true
        };

        // Act
        bool actual = fakePlayer.IsUnauthenticated();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void IsUnauthenticated_WhenNoAccountComponent_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var fakePlayer = new FakePlayer2();

        // Act
        Action act = () => fakePlayer.IsUnauthenticated();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
