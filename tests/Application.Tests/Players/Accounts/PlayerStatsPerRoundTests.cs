namespace CTF.Application.Tests.Players.Accounts;

public class PlayerStatsPerRoundTests
{
    [TestCase(10)]
    [TestCase(9)]
    [TestCase(8)]
    public void HasSufficientCoins_WhenPlayerHasSufficientCoins_ShouldReturnTrue(int amount)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        stats.AddCoins(10);

        // Act
        bool actual = stats.HasSufficientCoins(amount);

        // Assert
        actual.Should().BeTrue();
    }

    [TestCase(11)]
    [TestCase(12)]
    public void HasSufficientCoins_WhenPlayerHasInsufficientCoins_ShouldReturnFalse(int amount)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        stats.AddCoins(10);

        // Act
        bool actual = stats.HasSufficientCoins(amount);

        // Assert
        actual.Should().BeFalse();
    }

    [TestCase(10)]
    [TestCase(9)]
    [TestCase(8)]
    public void HasInsufficientCoins_WhenPlayerHasSufficientCoins_ShouldReturnFalse(int amount)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        stats.AddCoins(10);

        // Act
        bool actual = stats.HasInsufficientCoins(amount);

        // Assert
        actual.Should().BeFalse();
    }

    [TestCase(11)]
    [TestCase(12)]
    public void HasInsufficientCoins_WhenPlayerHasInsufficientCoins_ShouldReturnTrue(int amount)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        stats.AddCoins(10);

        // Act
        bool actual = stats.HasInsufficientCoins(amount);

        // Assert
        actual.Should().BeTrue();
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(101)]
    public void AddCoins_WhenCoinsAreNotBetween1To100_ShouldReturnFailureResult(int value)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        var expectedMessage = Messages.InvalidAddCoins;

        // Act
        Result result = stats.AddCoins(value);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [TestCase(1)]
    [TestCase(2)]
    [TestCase(99)]
    [TestCase(100)]
    public void AddCoins_WhenCoinsAreBetween1To100_ShouldReturnSuccessResult(int value)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();

        // Act
        Result result = stats.AddCoins(value);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        stats.Coins.Should().Be(value);
    }

    [TestCase(21)]
    [TestCase(22)]
    [TestCase(23)]
    [TestCase(100)]
    public void AddCoins_WhenSumOfCoinsExceedsValueOf100_ShouldSetValueTo100(int value)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        int expectedCoins = 100;
        stats.AddCoins(80);

        // Act
        Result result = stats.AddCoins(value);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        stats.Coins.Should().Be(expectedCoins);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(-101)]
    public void SubtractCoins_WhenCoinsAreNotInSpecifiedRange_ShouldReturnFailureResult(int value)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        // Should be in the range of -1 to -100.
        var expectedMessage = Messages.InvalidSubtractCoins;

        // Act
        Result result = stats.SubtractCoins(value);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(-99)]
    [TestCase(-100)]
    public void SubtractCoins_WhenCoinsAreInSpecifiedRange_ShouldReturnSuccessResult(int value)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        int expectedCoins = 0;

        // Act
        Result result = stats.SubtractCoins(value);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        stats.Coins.Should().Be(expectedCoins);
    }

    [TestCase(-11)]
    [TestCase(-12)]
    [TestCase(-13)]
    [TestCase(-100)]
    public void SubtractCoins_WhenSubtractionOfCoinsGivesNegativeResult_ShouldSetValueToZero(int value)
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        int expectedCoins = 0;
        stats.AddCoins(10);

        // Act
        Result result = stats.SubtractCoins(value);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        stats.Coins.Should().Be(expectedCoins);
    }

    [Test]
    public void AddKills_WhenCalledTwice_ShouldBeIncreasedTo2()
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        int expectedKills = 2;

        // Act
        stats.AddKills();
        stats.AddKills();

        // Assert
        stats.Kills.Should().Be(expectedKills);
    }

    [Test]
    public void AddDeaths_WhenCalledTwice_ShouldBeIncreasedTo2()
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        int expectedDeaths = 2;

        // Act
        stats.AddDeaths();
        stats.AddDeaths();

        // Assert
        stats.Deaths.Should().Be(expectedDeaths);
    }

    [Test]
    public void AddKillingSpree_WhenCalledTwice_ShouldBeIncreasedTo2()
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        int expectedKillingSpree = 2;

        // Act
        stats.AddKillingSpree();
        stats.AddKillingSpree();

        // Assert
        stats.KillingSpree.Should().Be(expectedKillingSpree);
    }

    [Test]
    public void ResetStats_WhenCalled_ShouldResetAllStatsToZero()
    {
        // Arrange
        var stats = new PlayerStatsPerRound();
        stats.AddKills();
        stats.AddKills();
        stats.AddDeaths();
        stats.AddDeaths();
        stats.AddKillingSpree();
        stats.AddKillingSpree();
        stats.AddCoins(15);

        // Act
        stats.ResetStats();

        // Asserts
        stats.Kills.Should().Be(0);
        stats.Deaths.Should().Be(0);
        stats.KillingSpree.Should().Be(0);
        stats.Coins.Should().Be(0);
    }
}
