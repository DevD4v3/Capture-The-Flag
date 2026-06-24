namespace CTF.Application.Players.Weapons.Catalogs;

/// <summary>
/// Represents the available weapon catalogs.
/// </summary>
public enum WeaponCatalogType
{
    [DisplayName("Walking Weapons")]
    Walking,

    [DisplayName("Run Weapons")]
    Run,

    [DisplayName("Mixed Weapons")]
    Mixed,

    [DisplayName("Rifles Only")]
    RifleOnly
}
