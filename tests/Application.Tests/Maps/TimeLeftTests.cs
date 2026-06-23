namespace CTF.Application.Tests.Maps;

public class TimeLeftTests
{
    private readonly TimeLeft _timeLeft;
    public TimeLeftTests()
    {
        _timeLeft = new TimeLeft();
        var minutes = new Minutes(3);
        _timeLeft.SetInterval(minutes);
    }

    [Test]
    public void IsCompleted_WhenTimeLeftIsCompleted_ShouldReturnTrue()
    {
        // Arrange
        var timeLeft = new TimeLeft();
        var seconds = new Seconds(0);
        var expectedText = "00:00";
        timeLeft.SetInterval(seconds);

        // Act
        bool actual = timeLeft.IsCompleted();

        // Asserts
        actual.Should().BeTrue();
        timeLeft.TextDraw.Should().Be(expectedText);
    }

    [Test]
    public void IsCompleted_WhenTimeLeftIsNotCompleted_ShouldReturnFalse()
    {
        // Arrange
        var timeLeft = new TimeLeft();
        var minutes = new Minutes(10);
        var expectedText = "10:00";
        timeLeft.SetInterval(minutes);

        // Act
        bool actual = timeLeft.IsCompleted();

        // Asserts
        actual.Should().BeFalse();
        timeLeft.TextDraw.Should().Be(expectedText);
    }

    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(61)]
    [TestCase(62)]
    public void SetInterval_WhenMinutesIntervalIsOutOfRange_ShouldReturnFailureResult(int value)
    {
        // Arrange
        var timeLeft = new TimeLeft();
        var minutes = new Minutes(value);
        var expectedText = "15:00";

        // Act
        Result result = timeLeft.SetInterval(minutes);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        timeLeft.TextDraw.Should().Be(expectedText);
    }

    [TestCase(0,  "00:00")]
    [TestCase(1,  "01:00")]
    [TestCase(3,  "03:00")]
    [TestCase(5,  "05:00")]
    [TestCase(9,  "09:00")]
    [TestCase(10, "10:00")]
    [TestCase(11, "11:00")]
    [TestCase(12, "12:00")]
    [TestCase(14, "14:00")]
    [TestCase(15, "15:00")]
    [TestCase(30, "30:00")]
    [TestCase(45, "45:00")]
    [TestCase(60, "60:00")]
    public void SetInterval_WhenMinutesIntervalIsNotOutOfRange_ShouldReturnSuccessResult(int value, string expectedText)
    {
        // Arrange
        var timeLeft = new TimeLeft();
        var minutes = new Minutes(value);

        // Act
        Result result = timeLeft.SetInterval(minutes);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        timeLeft.TextDraw.Should().Be(expectedText);
    }

    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(3601)]
    [TestCase(3602)]
    public void SetInterval_WhenSecondsIntervalIsOutOfRange_ShouldReturnFailureResult(int value)
    {
        // Arrange
        var timeLeft = new TimeLeft();
        var seconds = new Seconds(value);
        var expectedText = "15:00";

        // Act
        Result result = timeLeft.SetInterval(seconds);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        timeLeft.TextDraw.Should().Be(expectedText);
    }

    [TestCase(0,    "00:00")]
    [TestCase(1,    "00:01")]
    [TestCase(5,    "00:05")]
    [TestCase(60,   "01:00")]
    [TestCase(300,  "05:00")]
    [TestCase(428,  "07:08")]
    [TestCase(590,  "09:50")]
    [TestCase(608,  "10:08")]
    [TestCase(840,  "14:00")]
    [TestCase(900,  "15:00")]
    [TestCase(1800, "30:00")]
    [TestCase(2700, "45:00")]
    [TestCase(3600, "60:00")]
    public void SetInterval_WhenSecondsIntervalIsNotOutOfRange_ShouldReturnSuccessResult(int value, string expectedText)
    {
        // Arrange
        var timeLeft = new TimeLeft();
        var seconds = new Seconds(value);

        // Act
        Result result = timeLeft.SetInterval(seconds);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        timeLeft.TextDraw.Should().Be(expectedText);
    }

    [Test]
    public void Constructor_WhenObjectIsCreated_TextDrawShouldBeTheDefault()
    {
        // Arrange
        var expectedText = "15:00";

        // Act
        var timeLeft = new TimeLeft();

        // Assert
        timeLeft.TextDraw.Should().Be(expectedText);
    }

    [Test]
    public void Reset()
    {
        // Arrange
        var timeLeft = new TimeLeft();
        var minutes = new Minutes(5);
        var expectedText = "15:00";
        timeLeft.SetInterval(minutes);

        // Act
        timeLeft.Reset();

        // Assert
        timeLeft.TextDraw.Should().Be(expectedText);
    }

    [TestCaseSource(typeof(DecreaseTimeLeftTestCases))]
    public void Decrease_WhenTimeRemainingIsNotCompleted_ShouldContinueToDecrease(string expectedText)
    {
        // Arrange

        // Act
        _timeLeft.Decrease();

        // Assert
        _timeLeft.TextDraw.Should().Be(expectedText);
    }

    [Test]
    public void Decrease_WhenTimeRemainingIsZero_ShouldNotContinueToDecrease()
    {
        // Arrange
        var timeLeft = new TimeLeft();
        var minutes = new Seconds(4);
        var expectedText = "00:00";
        timeLeft.SetInterval(minutes);

        // Act
        timeLeft.Decrease(); // 00:03
        timeLeft.Decrease(); // 00:02
        timeLeft.Decrease(); // 00:01
        timeLeft.Decrease(); // 00:00
        timeLeft.Decrease(); // 00:00
        timeLeft.Decrease(); // 00:00

        // Assert
        timeLeft.TextDraw.Should().Be(expectedText);
    }
}
