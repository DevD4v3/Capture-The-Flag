namespace CTF.Application.Tests.Players.Vitalities;

public class VitalityTests
{
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(10)]
    [TestCase(20)]
    [TestCase(35)]
    [TestCase(50)]
    [TestCase(100)]
    public void Create_WhenCalledWithValidAmount_ShouldReturnSuccessResult(float amount)
    {
        // Arrange

        // Act
        Result<Vitality> result = Vitality.Create(amount);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Amount.Should().Be(amount);
    }

    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(101)]
    [TestCase(102)]
    public void Create_WhenCalledWithInvalidAmount_ShouldReturnFailureResult(float amount)
    {
        // Arrange
        var expectedMessage = Messages.InvalidVitality;

        // Act
        Result<Vitality> result = Vitality.Create(amount);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }
}
