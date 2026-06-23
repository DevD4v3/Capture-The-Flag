namespace CTF.Application.Tests.Players.Weapons;

public class WeaponTests
{
    [Test]
    public void Is_WhenWeaponIsEqualToKnife_ShouldReturnTrue()
    {
        // Arrange
        IWeapon weapon = WeaponDefinitions.Knife;

        // Act
        bool actual = weapon.Is(Weapon.Knife);

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void Is_WhenWeaponIsNotEqualToKnife_ShouldReturnFalse()
    {
        // Arrange
        IWeapon weapon = WeaponDefinitions.Knife;

        // Act
        bool actual = weapon.Is(Weapon.Deagle);

        // Assert
        actual.Should().BeFalse();
    }

    [Test]
    public void IsNot_WhenWeaponIsNotEqualToKnife_ShouldReturnTrue()
    {
        // Arrange
        IWeapon weapon = WeaponDefinitions.Knife;

        // Act
        bool actual = weapon.IsNot(Weapon.Deagle);

        // Assert
        actual.Should().BeTrue();
    }

    [Test]
    public void IsNot_WhenWeaponIsEqualToKnife_ShouldReturnFalse()
    {
        // Arrange
        IWeapon weapon = WeaponDefinitions.Knife;

        // Act
        bool actual = weapon.IsNot(Weapon.Knife);

        // Assert
        actual.Should().BeFalse();
    }
}
