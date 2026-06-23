namespace CTF.Application.Tests.Players.Extensions;

public class PlayerScoreExtensionsTests
{
    [Test]
    public void AddScore_WhenCalled_ShouldIncrementScoreByOne()
    {
        // Arrange
        int expectedScore = 11;
        var player = new FakePlayer(id: 1, name: "Bob")
        {
            Score = 10
        };

        // Act
        player.AddScore();

        // Assert
        player.Score.Should().Be(expectedScore);
    }

    [Test]
    public void AddScore_WhenValueIsPositive_ShouldIncreaseScore()
    {
        // Arrange
        int expectedScore = 15;
        var player = new FakePlayer(id: 1, name: "Bob")
        {
            Score = 10
        };

        // Act
        Result result = player.AddScore(5);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        player.Score.Should().Be(expectedScore);
    }

    [Test]
    public void AddScore_WhenValueIsZero_ShouldNotChangeScore()
    {
        // Arrange
        int expectedScore = 10;
        var player = new FakePlayer(id: 1, name: "Bob")
        {
            Score = 10
        };

        // Act
        Result result = player.AddScore(0);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        player.Score.Should().Be(expectedScore);
    }

    [Test]
    public void AddScore_WhenValueIsNegative_ShouldReturnFailureResult()
    {
        // Arrange
        int expectedScore = 10;
        var player = new FakePlayer(id: 1, name: "Bob")
        {
            Score = 10
        };

        // Act
        Result result = player.AddScore(-5);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        player.Score.Should().Be(expectedScore);
    }

    [Test]
    public void SetScore_WhenValueIsPositive_ShouldSetScore()
    {
        // Arrange
        int expectedScore = 20;
        var player = new FakePlayer(id: 1, name: "Bob")
        {
            Score = 10
        };

        // Act
        Result result = player.SetScore(20);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        player.Score.Should().Be(expectedScore);
    }

    [Test]
    public void SetScore_WhenValueIsZero_ShouldSetScoreToZero()
    {
        // Arrange
        int expectedScore = 0;
        var player = new FakePlayer(id: 1, name: "Bob")
        {
            Score = 10
        };

        // Act
        Result result = player.SetScore(0);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        player.Score.Should().Be(expectedScore);
    }

    [Test]
    public void SetScore_WhenValueIsNegative_ShouldReturnFailureResult()
    {
        // Arrange
        int expectedScore = 10;
        var player = new FakePlayer(id: 1, name: "Bob")
        {
            Score = 10
        };

        // Act
        Result result = player.SetScore(-5);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        player.Score.Should().Be(expectedScore);
    }
}
