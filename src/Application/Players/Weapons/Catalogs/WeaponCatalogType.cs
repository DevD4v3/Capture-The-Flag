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

    [DisplayName("Run & Walk Weapons")]
    Mixed,

    [DisplayName("Rifles Only")]
    RifleOnly,

    [DisplayName("War Weapons")]
    War,

    [DisplayName("Heavy Weapons")]
    Heavy,

    [DisplayName("Melee Weapons")]
    Melee
}
