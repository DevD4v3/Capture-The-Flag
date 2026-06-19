namespace CTF.Application.Players.Weapons.Catalogs;

/// <summary>
/// Stores the active weapon catalog configuration.
/// </summary>
public class WeaponCatalogSettings
{
    /// <summary>
    /// Gets or sets the catalog currently used by the server.
    /// </summary>
    public WeaponCatalogType Type { get; }

    public WeaponCatalogSettings(WeaponCatalogType type = WeaponCatalogType.Walking)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentOutOfRangeException(nameof(type), type, "The weapon catalog type is invalid.");

        Type = type;
    }
}
