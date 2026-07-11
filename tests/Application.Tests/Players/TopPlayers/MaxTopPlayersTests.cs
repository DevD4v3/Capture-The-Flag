namespace CTF.Application.Tests.Players.TopPlayers;

public class MaxTopPlayersTests
{
    [TestCase(5)]
    [TestCase(6)]
    [TestCase(10)]
    [TestCase(15)]
    public void Create_WhenCalledWithValidValue_ShouldReturnSuccessResult(int value)
    {
        // Arrange

        // Act
        Result<MaxTopPlayers> result = MaxTopPlayers.Create(value);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(value);
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(4)]
    [TestCase(16)]
    [TestCase(20)]
    public void Create_WhenCalledWithInvalidValue_ShouldReturnFailureResult(int value)
    {
        // Arrange
        var expectedMessage = Messages.InvalidMaxTopPlayers;

        // Act
        Result<MaxTopPlayers> result = MaxTopPlayers.Create(value);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }
}
