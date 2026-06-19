namespace CTF.Application.Tests.Players.Weapons;

public class WeaponCatalogSettingsTests
{
    [Test]
    public void Constructor_WhenTypeIsInvalid_ShouldThrowArgumentOutOfRangeException()
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
    public void Constructor_WhenTypeIsValid_ShouldCreateInstance()
    {
        // Arrange
        WeaponCatalogType type = WeaponCatalogType.Run;

        // Act
        var settings = new WeaponCatalogSettings(type);

        // Assert
        settings.Type.Should().Be(type);
    }
}
