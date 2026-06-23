namespace CTF.Application.Tests.Players.Ranks;

public class RankCollectionTests
{
    static readonly int[] InvalidRankCases = [-1, 1000, RankCollection.Count];

    [TestCaseSource(nameof(InvalidRankCases))]
    public void GetById_WhenRankIsInvalid_ShouldReturnFailureResult(int value)
    {
        // Arrange
        RankId rankId = (RankId)value;
        string expectedMessage = Messages.InvalidRank;

        // Act
        Result<IRank> result = RankCollection.GetById(rankId);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void GetById_WhenRankIsValid_ShouldReturnSuccessResult()
    {
        // Arrange
        RankId rankId = RankId.Noob;
        string expectedRank = rankId.ToString();

        // Act
        Result<IRank> result = RankCollection.GetById(rankId);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(expectedRank);
        result.Message.Should().BeEmpty();
    }

    [TestCaseSource(nameof(InvalidRankCases))]
    public void GetNextRank_WhenRankIsInvalid_ShouldReturnFailureResult(int value)
    {
        // Arrange
        RankId rankId = (RankId)value;
        string expectedMessage = Messages.InvalidRank;

        // Act
        Result<IRank> result = RankCollection.GetNextRank(rankId);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void GetNextRank_WhenRankIsValid_ShouldReturnSuccessResult()
    {
        // Arrange
        RankId previousRank = RankId.Noob;
        string expectedNextRank = RankId.Medium.ToString();

        // Act
        Result<IRank> result = RankCollection.GetNextRank(previousRank);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(expectedNextRank);
    }

    [Test]
    public void GetNextRank_WhenThereIsNoNextRank_ShouldNotReturnsAnyRank()
    {
        // Arrange
        RankId previousRank = RankId.Legendary;
        string expectedNextRank = "None";

        // Act
        Result<IRank> result = RankCollection.GetNextRank(previousRank);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(expectedNextRank);
    }

    [Test]
    public void GetByRequiredKills_WhenKillsIsNegative_ShouldReturnFailureResult()
    {
        // Arrange
        var expectedMessage = Messages.ValueCannotBeNegative;
        int kills = -1;

        // Act
        Result<IRank> result = RankCollection.GetByRequiredKills(kills);

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [TestCaseSource(typeof(GetRankByRequiredKillsTestCases))]
    public void GetByRequiredKills_WhenRankIsObtainedByKills_ShouldReturnSuccessResult((RankId ExpectedRankId, int Kills) rank)
    {
        // Arrange

        // Act
        Result<IRank> result = RankCollection.GetByRequiredKills(rank.Kills);

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(rank.ExpectedRankId);
    }
}
