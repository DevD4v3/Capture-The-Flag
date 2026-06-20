namespace CTF.Application.Players.Weapons.Catalogs;

/// <summary>
/// Represents the weapon catalog configuration currently used by the server.
/// The active catalog can be changed at runtime.
/// </summary>
public class WeaponCatalogSettings
{
    /// <summary>
    /// Gets or sets the catalog currently used by the server.
    /// </summary>
    public WeaponCatalogType Type { get; private set; }

    public WeaponCatalogSettings(WeaponCatalogType type = WeaponCatalogType.Walking)
    {
        EnsureValidCatalog(type);
        Type = type;
    }

    /// <summary>
    /// Changes the active weapon catalog used by the server.
    /// </summary>
    /// <param name="type">
    /// The weapon catalog to activate.
    /// </param>
    public void Change(WeaponCatalogType type)
    {
        EnsureValidCatalog(type);
        Type = type;
    }

    private static void EnsureValidCatalog(WeaponCatalogType type)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentOutOfRangeException(
                paramName: nameof(type),
                actualValue: type,
                message: "The weapon catalog type is invalid.");    
    }
}
