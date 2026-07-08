namespace CTF.Application.Tests.GunGames;

public class WeaponLevelTests
{
    private readonly MaxWeaponLevel _maxLevel = new(4);

    [Test]
    public void Next_WhenWeaponLevelIsBelowMaxLevel_ShouldAdvanceToNextLevel()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;

        // Act
        WeaponLevel secondLevel = firstLevel.Next(_maxLevel);

        // Assert
        secondLevel.Value.Should().Be(2);
    }

    [Test]
    public void Next_WhenWeaponLevelIsAtMaxLevel_ShouldRemainAtMaxLevel()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_maxLevel);
        WeaponLevel thirdLevel = secondLevel.Next(_maxLevel);
        WeaponLevel finalLevel = thirdLevel.Next(_maxLevel);

        // Act
        WeaponLevel nextLevel = finalLevel.Next(_maxLevel);

        // Assert
        nextLevel.Value.Should().Be(_maxLevel.Value);
    }

    [Test]
    public void Previous_WhenWeaponLevelIsAboveFirstLevel_ShouldMoveToPreviousLevel()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_maxLevel);
        WeaponLevel thirdLevel = secondLevel.Next(_maxLevel);

        // Act
        WeaponLevel previousLevel = thirdLevel.Previous();

        // Assert
        previousLevel.Should().Be(secondLevel);
    }

    [Test]
    public void Previous_WhenWeaponLevelIsAtFirstLevel_ShouldRemainAtFirstLevel()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;

        // Act
        WeaponLevel previousLevel = firstLevel.Previous();

        // Assert
        previousLevel.Should().Be(firstLevel);
    }

    [Test]
    public void IsMax_WhenWeaponLevelIsMaxLevel_ShouldReturnTrue()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_maxLevel);
        WeaponLevel thirdLevel = secondLevel.Next(_maxLevel);
        WeaponLevel finalLevel = thirdLevel.Next(_maxLevel);

        // Act
        bool isMax = finalLevel.IsMax(_maxLevel);

        // Assert
        isMax.Should().BeTrue();
    }

    [Test]
    public void IsMax_WhenWeaponLevelIsBelowMaxLevel_ShouldReturnFalse()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_maxLevel);

        // Act
        bool isMax = secondLevel.IsMax(_maxLevel);

        // Assert
        isMax.Should().BeFalse();
    }

    [Test]
    public void ToString_ShouldReturnWeaponLevelValue()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_maxLevel);

        // Act
        string value = secondLevel.ToString();

        // Assert
        value.Should().Be("2");
    }

    [Test]
    public void EqualityOperator_WhenWeaponLevelsAreEqual_ShouldReturnTrue()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel left = firstLevel.Next(_maxLevel);
        WeaponLevel right = firstLevel.Next(_maxLevel);

        // Act
        bool areEqual = left == right;

        // Assert
        areEqual.Should().BeTrue();
    }

    [Test]
    public void GreaterThanOperator_WhenLeftWeaponLevelIsGreater_ShouldReturnTrue()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_maxLevel);

        // Act
        bool isGreater = secondLevel > firstLevel;

        // Assert
        isGreater.Should().BeTrue();
    }
}
