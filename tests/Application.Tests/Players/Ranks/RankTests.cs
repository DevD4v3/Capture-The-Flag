namespace CTF.Application.Tests.Players.Ranks;

public class RankTests
{
    [Test]
    public void IsMax_WhenRankIsMaximum_ShouldReturnTrue()
    {
        // Arrange
        RankId rankId = RankId.Legendary;
        Result<IRank> result = RankCollection.GetById(rankId);
        IRank rank = result.Value;

        // Act
        bool actual = rank.IsMax();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsMax_WhenRankIsNotMaximum_ShouldReturnFalse()
    {
        // Arrange
        RankId rankId = RankId.Junior;
        Result<IRank> result = RankCollection.GetById(rankId);
        IRank rank = result.Value;

        // Act
        bool actual = rank.IsMax();

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void IsNotMax_WhenRankIsNotMaximum_ShouldReturnTrue() 
    {
        // Arrange
        RankId rankId = RankId.Junior;
        Result<IRank> result = RankCollection.GetById(rankId);
        IRank rank = result.Value;

        // Act
        bool actual = rank.IsNotMax();

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsNotMax_WhenRankIsMaximum_ShouldReturnFalse()
    {
        // Arrange
        RankId rankId = RankId.Legendary;
        Result<IRank> result = RankCollection.GetById(rankId);
        IRank rank = result.Value;

        // Act
        bool actual = rank.IsNotMax();

        // Assert
        actual.Should().BeFalse();
    }
}
