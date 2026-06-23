namespace CTF.Application.Tests.Players.Accounts;

public class CanMoveUpToNextRankTests
{
    [Test]
    public void CanMoveUpToNextRank_WhenPlayerHasMaximumRank_ShouldReturnFalse()
    {
        // Arrange
        var player = new PlayerInfo();
        RankId maxRank = RankId.Legendary;
        player.SetRank(maxRank);
        player.SetTotalKills(701);

        // Act
        bool actual = player.CanMoveUpToNextRank();

        // Assert
        actual.Should().BeFalse();
    }

    [TestCase(RankId.Noob,          50)]
    [TestCase(RankId.Noob,          51)]
    [TestCase(RankId.Medium,       100)]
    [TestCase(RankId.Medium,       101)]
    [TestCase(RankId.Junior,       150)]
    [TestCase(RankId.Junior,       151)]
    [TestCase(RankId.SemiAdvance,  200)]
    [TestCase(RankId.SemiAdvance,  201)]
    [TestCase(RankId.Advanced,     250)]
    [TestCase(RankId.Advanced,     251)]
    [TestCase(RankId.Hitman,       300)]
    [TestCase(RankId.Hitman,       301)]
    [TestCase(RankId.Extreme,      350)]
    [TestCase(RankId.Extreme,      351)]
    [TestCase(RankId.Annihilator,  400)]
    [TestCase(RankId.Annihilator,  401)]
    [TestCase(RankId.Maniac,       450)]
    [TestCase(RankId.Maniac,       451)]
    [TestCase(RankId.Invincible,   500)]
    [TestCase(RankId.Invincible,   501)]
    [TestCase(RankId.Senior,       550)]
    [TestCase(RankId.Senior,       551)]
    [TestCase(RankId.GameMaster,   600)]
    [TestCase(RankId.GameMaster,   601)]
    [TestCase(RankId.Professional, 650)]
    [TestCase(RankId.Professional, 651)]
    [TestCase(RankId.SuperPro,     700)]
    [TestCase(RankId.SuperPro,     701)]
    public void CanMoveUpToNextRank_WhenPlayerDoesHaveRequiredKills_ShouldReturnTrue(RankId currentRank, int kills)
    {
        // Arrange
        var player = new PlayerInfo();
        player.SetRank(currentRank);
        player.SetTotalKills(kills);

        // Act
        bool actual = player.CanMoveUpToNextRank();

        // Assert
        actual.Should().BeTrue();
    }

    [TestCase(RankId.Noob,         10)]
    [TestCase(RankId.Noob,         11)]
    [TestCase(RankId.Medium,       50)]
    [TestCase(RankId.Medium,       51)]
    [TestCase(RankId.Junior,       100)]
    [TestCase(RankId.Junior,       101)]
    [TestCase(RankId.SemiAdvance,  150)]
    [TestCase(RankId.SemiAdvance,  151)]
    [TestCase(RankId.Advanced,     200)]
    [TestCase(RankId.Advanced,     201)]
    [TestCase(RankId.Hitman,       250)]
    [TestCase(RankId.Hitman,       251)]
    [TestCase(RankId.Extreme,      300)]
    [TestCase(RankId.Extreme,      301)]
    [TestCase(RankId.Annihilator,  350)]
    [TestCase(RankId.Annihilator,  351)]
    [TestCase(RankId.Maniac,       400)]
    [TestCase(RankId.Maniac,       401)]
    [TestCase(RankId.Invincible,   450)]
    [TestCase(RankId.Invincible,   451)]
    [TestCase(RankId.Senior,       500)]
    [TestCase(RankId.Senior,       501)]
    [TestCase(RankId.GameMaster,   550)]
    [TestCase(RankId.GameMaster,   551)]
    [TestCase(RankId.Professional, 600)]
    [TestCase(RankId.Professional, 601)]
    [TestCase(RankId.SuperPro,     650)]
    [TestCase(RankId.SuperPro,     651)]
    public void CanMoveUpToNextRank_WhenPlayerDoesNotHaveRequiredKills_ShouldReturnFalse(RankId currentRank, int kills)
    {
        // Arrange
        var player = new PlayerInfo();
        player.SetRank(currentRank);
        player.SetTotalKills(kills);

        // Act
        bool actual = player.CanMoveUpToNextRank();

        // Assert
        actual.Should().BeFalse();
    }
}
