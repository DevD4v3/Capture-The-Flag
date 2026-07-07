namespace CTF.Application.Tests.GunGames;

public class WeaponProgressionBaseTests
{
    private TestWeaponProgression _progression;

    [SetUp]
    public void Init()
    {
        _progression = new TestWeaponProgression();
    }

    [Test]
    public void WeaponProgression_WhenNoWeaponsAreDefined_ShouldThrowInvalidOperationException()
    {
        // Arrange

        // Act
        Action act = () => _ = new EmptyWeaponProgression();

        // Assert
        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("A weapon progression must define at least one weapon.");
    }

    [Test]
    public void GetWeapon_WhenWeaponLevelIsFirst_ShouldReturnFirstWeapon()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;

        // Act
        IWeapon weapon = _progression.GetWeapon(firstLevel);

        // Assert
        weapon.Should().Be(WeaponDefinitions.Colt45);
    }

    [Test]
    public void GetWeapon_WhenWeaponLevelIsSecond_ShouldReturnSecondWeapon()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_progression.MaxLevel);

        // Act
        IWeapon weapon = _progression.GetWeapon(secondLevel);

        // Assert
        weapon.Should().Be(WeaponDefinitions.Shotgun);
    }

    [Test]
    public void GetWeapon_WhenWeaponLevelIsFinal_ShouldReturnFinalWeapon()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_progression.MaxLevel);
        WeaponLevel thirdLevel = secondLevel.Next(_progression.MaxLevel);
        WeaponLevel finalLevel = thirdLevel.Next(_progression.MaxLevel);

        // Act
        IWeapon weapon = _progression.GetWeapon(finalLevel);

        // Assert
        weapon.Should().Be(WeaponDefinitions.Knife);
    }

    [Test]
    public void GetWeapon_WhenWeaponLevelIsBelowFirst_ShouldThrowInvalidOperationException()
    {
        // Arrange
        WeaponLevel weaponLevel = default;

        // Act
        Action act = () => _progression.GetWeapon(weaponLevel);

        // Assert
        act.Should()
           .Throw<InvalidOperationException>()
           .WithMessage("No weapon defined for level 0");

    }

    [Test]
    public void MaxLevel_ShouldBeEqualToNumberOfWeapons()
    {
        // Arrange

        // Act
        MaxWeaponLevel maxLevel = _progression.MaxLevel;

        // Assert
        maxLevel.Value.Should().Be(4);
    }

    [Test]
    public void IsFinalLevel_WhenWeaponLevelIsMaxLevel_ShouldReturnTrue()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_progression.MaxLevel);
        WeaponLevel thirdLevel = secondLevel.Next(_progression.MaxLevel);
        WeaponLevel finalLevel = thirdLevel.Next(_progression.MaxLevel);

        // Act
        bool isFinalLevel = _progression.IsFinalLevel(finalLevel);

        // Assert
        isFinalLevel.Should().BeTrue();
    }

    [Test]
    public void IsFinalLevel_WhenWeaponLevelIsBelowMaxLevel_ShouldReturnFalse()
    {
        // Arrange
        WeaponLevel firstLevel = WeaponLevel.First;
        WeaponLevel secondLevel = firstLevel.Next(_progression.MaxLevel);

        // Act
        bool isFinalLevel = _progression.IsFinalLevel(secondLevel);

        // Assert
        isFinalLevel.Should().BeFalse();
    }

    private class EmptyWeaponProgression : WeaponProgressionBase
    {
        public override WeaponProgressionType Type => WeaponProgressionType.Classic;

        protected override void Define(List<IWeapon> weapons)
        {
        }
    }
}
