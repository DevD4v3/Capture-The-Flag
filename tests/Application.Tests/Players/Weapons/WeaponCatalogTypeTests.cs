namespace CTF.Application.Tests.Players.Weapons;

public class WeaponCatalogTypeTests
{
    [Test]
    public void AllValues_ShouldHaveDisplayName()
    {
        foreach (WeaponCatalogType type in Enum.GetValues<WeaponCatalogType>())
        {
            Action action = () => type.GetDisplayName();
            action.Should().NotThrow();
        }
    }
}
