namespace CTF.Application.Tests.Teams.Flags;

public class FlagTests
{
    [Test]
    public void HasCarrier_WhenFlagHasCarrier_ShouldReturnsTrue()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        flag.Capture(fakeCarrier);

        // Act
        bool actual = flag.HasCarrier;

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void HasCarrier_WhenFlagHasNoCarrier_ShouldReturnsFalse()
    {
        // Arrange
        var flag = CreateFlag();

        // Act
        bool actual = flag.HasCarrier;

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void CarrierName_WhenFlagHasCarrier_ShouldReturnsCarrierName()
    {
        // Arrange
        var flag = CreateFlag();
        var expectedCarrierName = "Bob";
        var fakeCarrier = new FakePlayer(id: 1, expectedCarrierName);

        flag.Capture(fakeCarrier);

        // Act
        string actual = flag.CarrierName;

        // Assert
        actual.Should().Be(expectedCarrierName);
    }

    [Test]
    public void CarrierName_WhenFlagHasNoCarrier_ShouldReturnsNone()
    {
        // Arrange
        var flag = CreateFlag();
        var expectedCarrierName = "None";

        // Act
        string actual = flag.CarrierName;

        // Assert
        actual.Should().Be(expectedCarrierName);
    }

    [Test]
    public void Capture_WhenArgumentIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var flag = CreateFlag();
        Player player = default;

        // Act
        Action act = () => flag.Capture(player);

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithParameterName(nameof(player));
    }

    [Test]
    public void Capture_WhenArgumentIsValid_ShouldSetPlayerAsCarrier()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        // Act
        flag.Capture(fakeCarrier);

        // Assert
        flag.Carrier.Should().Be(fakeCarrier);
    }

    [Test]
    public void Capture_WhenArgumentIsValid_ShouldSetStatusAsCaptured()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        // Act
        flag.Capture(fakeCarrier);

        // Assert
        flag.Status.Should().Be(FlagStatus.Captured);
    }

    [Test]
    public void Capture_WhenFlagAlreadyHasCarrier_ShouldReplaceCarrier()
    {
        // Arrange
        var flag = CreateFlag();
        var player1 = new FakeCarrier();
        var player2 = new FakeCarrier();

        flag.Capture(player1);

        // Act
        flag.Capture(player2);

        // Assert
        flag.Carrier.Should().Be(player2);
        flag.Status.Should().Be(FlagStatus.Captured);
    }

    [Test]
    public void Capture_WhenFlagWasReturnedToBase_ShouldSetNewCarrier()
    {
        // Arrange
        var flag = CreateFlag();
        var player1 = new FakeCarrier();
        var player2 = new FakeCarrier();

        flag.Capture(player1);
        flag.ReturnToBase();

        // Act
        flag.Capture(player2);

        // Assert
        flag.Carrier.Should().Be(player2);
        flag.Status.Should().Be(FlagStatus.Captured);
    }

    [Test]
    public void Take_WhenArgumentIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var flag = CreateFlag();
        Player player = default;

        // Act
        Action act = () => flag.Take(player);

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithParameterName(nameof(player));
    }

    [Test]
    public void Take_WhenArgumentIsValid_ShouldSetPlayerAsCarrier()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        // Act
        flag.Take(fakeCarrier);

        // Assert
        flag.Carrier.Should().Be(fakeCarrier);
    }

    [Test]
    public void Take_WhenArgumentIsValid_ShouldSetStatusAsTaken()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        // Act
        flag.Take(fakeCarrier);

        // Assert
        flag.Status.Should().Be(FlagStatus.Taken);
    }

    [Test]
    public void Take_WhenFlagAlreadyHasCarrier_ShouldReplaceCarrier()
    {
        // Arrange
        var flag = CreateFlag();
        var player1 = new FakeCarrier();
        var player2 = new FakeCarrier();

        flag.Take(player1);

        // Act
        flag.Take(player2);

        // Assert
        flag.Carrier.Should().Be(player2);
        flag.Status.Should().Be(FlagStatus.Taken);
    }

    [Test]
    public void Take_WhenFlagWasDropped_ShouldSetNewCarrier()
    {
        // Arrange
        var flag = CreateFlag();
        var player1 = new FakeCarrier();
        var player2 = new FakeCarrier();

        flag.Capture(player1);
        flag.Drop();

        // Act
        flag.Take(player2);

        // Assert
        flag.Carrier.Should().Be(player2);
        flag.Status.Should().Be(FlagStatus.Taken);
    }

    [Test]
    public void Drop_WhenFlagHasCarrier_ShouldRemoveCarrier()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        flag.Capture(fakeCarrier);

        // Act
        flag.Drop();

        // Assert
        flag.Carrier.Should().BeNull();
    }

    [Test]
    public void Drop_WhenCalled_ShouldSetStatusAsDropped()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        flag.Capture(fakeCarrier);

        // Act
        flag.Drop();

        // Assert
        flag.Status.Should().Be(FlagStatus.Dropped);
    }

    [Test]
    public void Drop_WhenFlagHasNoCarrier_ShouldNotThrowNullReferenceException()
    {
        // Arrange
        var flag = CreateFlag();

        // Act
        Action act = flag.Drop;

        // Assert
        act.Should().NotThrow<NullReferenceException>();
    }

    [Test]
    public void ReturnToBase_WhenFlagHasCarrier_ShouldRemoveCarrier()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        flag.Capture(fakeCarrier);

        // Act
        flag.ReturnToBase();

        // Assert
        flag.Carrier.Should().BeNull();
    }

    [Test]
    public void ReturnToBase_WhenCalled_ShouldSetStatusAsBasePosition()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        flag.Capture(fakeCarrier);

        // Act
        flag.ReturnToBase();

        // Assert
        flag.Status.Should().Be(FlagStatus.BasePosition);
    }

    [Test]
    public void Reset_WhenFlagHasCarrier_ShouldRemoveCarrier()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        flag.Capture(fakeCarrier);

        // Act
        flag.Reset();

        // Assert
        flag.Carrier.Should().BeNull();
    }

    [Test]
    public void Reset_WhenCalled_ShouldSetStatusAsBasePosition()
    {
        // Arrange
        var flag = CreateFlag();
        var fakeCarrier = new FakeCarrier();

        flag.Capture(fakeCarrier);

        // Act
        flag.Reset();

        // Assert
        flag.Status.Should().Be(FlagStatus.BasePosition);
    }

    private static Flag CreateFlag() =>
        new()
        {
            Model = FlagModel.Red,
            Icon = FlagIcon.Red,
            Name = "Red",
            ColorHex = Color.Red
        };
}
