namespace CTF.Application.Tests.GunGames;

public class PlayerProgressionTests
{
    private readonly MaxWeaponLevel _maxLevel = new(4);

    [Test]
    public void PlayerProgression_WhenCreated_ShouldStartAtFirstWeaponLevel()
    {
        // Arrange

        // Act
        var progression = new PlayerProgression();

        // Assert
        progression.WeaponLevel.Should().Be(WeaponLevel.First);
        progression.KillsTowardsNextLevel.Should().Be(0);
    }

    [Test]
    public void AddKillsTowardsNextLevel_WhenInvoked_ShouldIncreaseKillsTowardsNextLevel()
    {
        // Arrange
        var progression = new PlayerProgression();

        // Act
        progression.AddKillsTowardsNextLevel();

        // Assert
        progression.KillsTowardsNextLevel.Should().Be(1);
    }

    [Test]
    public void CanLevelUp_WhenRequiredKillsHaveNotBeenReached_ShouldReturnFalse()
    {
        // Arrange
        var progression = new PlayerProgression();
        var requiredKills = new KillsRequiredPerLevel(2);

        progression.AddKillsTowardsNextLevel();

        // Act
        bool canLevelUp = progression.CanLevelUp(requiredKills);

        // Assert
        canLevelUp.Should().BeFalse();
    }

    [Test]
    public void CanLevelUp_WhenRequiredKillsAreReached_ShouldReturnTrue()
    {
        // Arrange
        var progression = new PlayerProgression();
        var requiredKills = new KillsRequiredPerLevel(2);

        progression.AddKillsTowardsNextLevel();
        progression.AddKillsTowardsNextLevel();

        // Act
        bool canLevelUp = progression.CanLevelUp(requiredKills);

        // Assert
        canLevelUp.Should().BeTrue();
    }

    [Test]
    public void CanLevelUp_WhenRequiredKillsHaveBeenExceeded_ShouldReturnTrue()
    {
        // Arrange
        var progression = new PlayerProgression();
        var requiredKills = new KillsRequiredPerLevel(2);

        progression.AddKillsTowardsNextLevel();
        progression.AddKillsTowardsNextLevel();
        progression.AddKillsTowardsNextLevel();

        // Act
        bool canLevelUp = progression.CanLevelUp(requiredKills);

        // Assert
        canLevelUp.Should().BeTrue();
    }

    [Test]
    public void LevelUp_WhenPlayerIsBelowMaxLevel_ShouldAdvanceToNextLevel()
    {
        // Arrange
        var progression = new PlayerProgression();

        // Act
        progression.LevelUp(_maxLevel);

        // Assert
        progression.WeaponLevel.Value.Should().Be(2);
    }

    [Test]
    public void LevelUp_WhenPlayerIsAtMaxLevel_ShouldRemainAtMaxLevel()
    {
        // Arrange
        var progression = new PlayerProgression();

        progression.LevelUp(_maxLevel);
        progression.LevelUp(_maxLevel);
        progression.LevelUp(_maxLevel);

        // Act
        progression.LevelUp(_maxLevel);

        // Assert
        progression.WeaponLevel.Value.Should().Be(_maxLevel.Value);
    }

    [Test]
    public void LevelUp_WhenInvoked_ShouldResetKillsTowardsNextLevel()
    {
        // Arrange
        var progression = new PlayerProgression();

        progression.AddKillsTowardsNextLevel();
        progression.AddKillsTowardsNextLevel();

        // Act
        progression.LevelUp(_maxLevel);

        // Assert
        progression.KillsTowardsNextLevel.Should().Be(0);
    }

    [Test]
    public void LevelDown_WhenPlayerIsAboveFirstLevel_ShouldMoveToPreviousLevel()
    {
        // Arrange
        var progression = new PlayerProgression();

        progression.LevelUp(_maxLevel);
        progression.LevelUp(_maxLevel);

        // Act
        progression.LevelDown();

        // Assert
        progression.WeaponLevel.Value.Should().Be(2);
    }

    [Test]
    public void LevelDown_WhenPlayerIsAtFirstLevel_ShouldRemainAtFirstLevel()
    {
        // Arrange
        var progression = new PlayerProgression();

        // Act
        progression.LevelDown();

        // Assert
        progression.WeaponLevel.Should().Be(WeaponLevel.First);
    }

    [Test]
    public void LevelDown_WhenInvoked_ShouldResetKillsTowardsNextLevel()
    {
        // Arrange
        var progression = new PlayerProgression();

        progression.LevelUp(_maxLevel);
        progression.AddKillsTowardsNextLevel();
        progression.AddKillsTowardsNextLevel();

        // Act
        progression.LevelDown();

        // Assert
        progression.KillsTowardsNextLevel.Should().Be(0);
    }

    [Test]
    public void Reset_WhenInvoked_ShouldRestoreInitialProgression()
    {
        // Arrange
        var progression = new PlayerProgression();

        progression.LevelUp(_maxLevel);
        progression.LevelUp(_maxLevel);
        progression.AddKillsTowardsNextLevel();
        progression.AddKillsTowardsNextLevel();

        // Act
        progression.Reset();

        // Assert
        progression.WeaponLevel.Should().Be(WeaponLevel.First);
        progression.KillsTowardsNextLevel.Should().Be(0);
    }
}
