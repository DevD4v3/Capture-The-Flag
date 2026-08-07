namespace CTF.Application.Players.Weapons;

/// <summary>
/// Represents the active weapon catalog used by the server.
/// </summary>
/// <remarks>
/// Consumers do not need to know which weapon catalog is active.
/// This class always exposes the catalog selected by the current server configuration.
/// </remarks>
public class ActiveWeaponCatalog(
    WeaponCatalogSettings settings, 
    FrozenDictionary<WeaponCatalogType, WeaponCatalog> catalogs)
{
    private WeaponCatalog Current 
        => catalogs[settings.Type];

    /// <inheritdoc cref="WeaponCatalog.Count"/>
    public int Count 
        => Current.Count;

    /// <inheritdoc cref="WeaponCatalog.GetAll"/>
    public IReadOnlyList<IWeapon> GetAll()
        => Current.GetAll();

    /// <inheritdoc cref="WeaponCatalog.Contains"/>
    public bool Contains(IWeapon weapon)
        => Current.Contains(weapon);

    /// <inheritdoc cref="WeaponCatalog.GetById"/>
    public Result<IWeapon> GetById(Weapon id)
        => Current.GetById(id);

    /// <inheritdoc cref="WeaponCatalog.GetByName"/>
    public Result<IWeapon> GetByName(string weaponName)
        => Current.GetByName(weaponName);
}
