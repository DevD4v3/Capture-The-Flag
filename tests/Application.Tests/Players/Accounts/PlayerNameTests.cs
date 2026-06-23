namespace CTF.Application.Tests.Players.Accounts;

public class PlayerNameTests
{
    [Test]
    public void SetName_WhenArgumentIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var player = new PlayerInfo();
        string name = default;

        // Act
        Action act = () => player.SetName(name);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("   ")]
    public void SetName_WhenNameIsEmpty_ShouldReturnFailureResult(string name)
    {
        // Arrange
        var player = new PlayerInfo();
        var expectedMessage = Messages.NameCannotBeEmpty;

        // Act
        Result result = player.SetName(name);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
        player.Name.Should().NotBe(name);
    }

    [TestCase("a")]
    [TestCase("ab")]
    [TestCase("aaaaaaaaaaaaaaaaaaaaa")]
    public void SetName_WhenNameLengthIsInvalid_ShouldReturnFailureResult(string name)
    {
        // Arrange
        var player = new PlayerInfo();
        var expectedMessage = Messages.PlayerNameLength;

        // Act
        Result result = player.SetName(name);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
        player.Name.Should().NotBe(name);
    }

    [TestCase("--##+*&?$¡¿!%{}")]
    [TestCase("/''\\,´¬||\"¨;")]
    [TestCase("ññÑÑáéíóú")]
    [TestCase("ÁÉÍÚÓ")]
    [TestCase("><`°°¬")]
    public void SetName_WhenNickNameHasInvalidCharacters_ShouldReturnFailureResult(string name)
    {
        // Arrange
        var player = new PlayerInfo();
        var expectedMessage = Messages.InvalidNickName;

        // Act
        Result result = player.SetName(name);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
        player.Name.Should().NotBe(name);
    }

    [TestCase("$@_.==[]()")]
    [TestCase("0123456789")]
    [TestCase("QWERTYUIOPASDFGHJKL")]
    [TestCase("ZXCVBNM")]
    [TestCase("qwertyuiopasdfghjkl")]
    [TestCase("zxcvbnm")]
    public void SetName_WhenNickNameHasValidCharacters_ShouldReturnSuccessResult(string name)
    {
        // Arrange
        var player = new PlayerInfo();

        // Act
        Result result = player.SetName(name);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        player.Name.Should().Be(name);
    }
}
