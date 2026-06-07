namespace CTF.Application.Tests.Players.Extensions;

public class PlayerExtensionsTests
{
    [Test]
    public void GetInfo_WhenNoAccountComponent_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var fakePlayer = new FakePlayer1();

        // Act
        Action act = () => fakePlayer.GetInfo();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Test]
    public void GetInfo_WhenAccountComponentIsAssigned_ShouldNotThrowInvalidOperationException()
    {
        // Arrange
        var fakePlayer = new FakePlayer2();

        // Act
        Action act = () => fakePlayer.GetInfo();

        // Assert
        act.Should().NotThrow<InvalidOperationException>();
    }

    [Test]
    public void IsUnauthenticated_WhenPlayerIsUnauthenticated_ShouldReturnsTrue()
    {
        // Arrange
        var fakePlayer = new FakePlayer2()
        {
            IsAuthenticated = false
        };

        // Act
        bool actual = fakePlayer.IsUnauthenticated();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsUnauthenticated_WhenPlayerIsAuthenticated_ShouldReturnsFalse()
    {
        // Arrange
        var fakePlayer = new FakePlayer2()
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
        var fakePlayer = new FakePlayer1();

        // Act
        Action act = () => fakePlayer.IsUnauthenticated();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    private class FakePlayer1 : Player
    {
        public FakePlayer1() : base(Substitute.For<IOmpEntityProvider>(), default)
        {
        }

        public override T GetComponent<T>()
        {
            return null;
        }
    }

    private class FakePlayer2 : Player
    {
        public FakePlayer2() : base(Substitute.For<IOmpEntityProvider>(), default)
        {
        }

        public bool IsAuthenticated { get; set; } = true;
        public override T GetComponent<T>()
        {
            var accountComponent = new AccountComponent(new PlayerInfo(), IsAuthenticated);
            return accountComponent as T;
        }
    }
}
