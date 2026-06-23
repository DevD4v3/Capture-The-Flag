namespace CTF.Application.Tests.Players.Weapons;

public class WeaponCatalogBaseTests
{
    [Test]
    public void GetById_WhenWeaponIdIsNotFound_ShouldReturnFailureResult()
    {
        // Arrange
        var catalog = new TestWeaponCatalog();
        Weapon weaponId = Weapon.Connect;
        string expectedMessage = Messages.WeaponNotFound;

        // Act
        Result<IWeapon> result = catalog.GetById(weaponId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void GetById_WhenWeaponIdIsFound_ShouldReturnSuccessResult()
    {
        // Arrange
        var catalog = new TestWeaponCatalog();
        Weapon expectedWeaponId = Weapon.Deagle;

        // Act
        Result<IWeapon> result = catalog.GetById(expectedWeaponId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(expectedWeaponId);
    }

    [TestCase("")]
    [TestCase("  ")]
    [TestCase("Connect")]
    public void GetByName_WhenWeaponNameIsNotFound_ShouldReturnFailureResult(string weaponName)
    {
        // Arrange
        var catalog = new TestWeaponCatalog();
        string expectedMessage = Messages.WeaponNotFound;

        // Act
        Result<IWeapon> result = catalog.GetByName(weaponName);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void GetByName_WhenArgumentIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        var catalog = new TestWeaponCatalog();
        string weaponName = default;

        // Act
        Action act = () => catalog.GetByName(weaponName);

        // Assert
        act.Should()
           .Throw<ArgumentNullException>()
           .WithParameterName(nameof(weaponName));
    }

    [TestCase("Deagle")]
    [TestCase("DEAGLE")]
    [TestCase("deagle")]
    [TestCase("DeAgLe")]
    public void GetByName_WhenWeaponNameIsFound_ShouldReturnSuccessResult(string weaponName)
    {
        // Arrange
        var catalog = new TestWeaponCatalog();
        Weapon expectedWeaponId = Weapon.Deagle;

        // Act
        Result<IWeapon> result = catalog.GetByName(weaponName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(expectedWeaponId);
    }
}
