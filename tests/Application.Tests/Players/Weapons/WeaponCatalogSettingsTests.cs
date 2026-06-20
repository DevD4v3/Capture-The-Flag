namespace CTF.Application.Tests.Players.Weapons;

public class WeaponCatalogSettingsTests
{
    [Test]
    public void Constructor_WhenCatalogTypeIsInvalid_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        WeaponCatalogType type = (WeaponCatalogType)(-1);

        // Act
        Action act = () => new WeaponCatalogSettings(type);

        // Assert
        act.Should()
           .Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(type));
    }

    [Test]
    public void Constructor_WhenCatalogTypeIsValid_ShouldCreateInstance()
    {
        // Arrange
        WeaponCatalogType type = WeaponCatalogType.Run;

        // Act
        var settings = new WeaponCatalogSettings(type);

        // Assert
        settings.Type.Should().Be(type);
    }

    [Test]
    public void Change_WhenCatalogTypeIsInvalid_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var settings = new WeaponCatalogSettings();
        WeaponCatalogType type = (WeaponCatalogType)(-1);

        // Act
        Action act = () => settings.Change(type);

        // Assert
        act.Should()
           .Throw<ArgumentOutOfRangeException>()
           .WithParameterName(nameof(type));
    }

    [Test]
    public void Change_WhenCatalogTypeIsValid_ShouldUpdateCatalog()
    {
        // Arrange
        var settings = new WeaponCatalogSettings(WeaponCatalogType.Walking);

        // Act
        settings.Change(WeaponCatalogType.Run);

        // Assert
        settings.Type.Should().Be(WeaponCatalogType.Run);
    }
}
