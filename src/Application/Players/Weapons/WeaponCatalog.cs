namespace CTF.Application.Players.Weapons;

/// <summary>
/// Provides access to the active weapon catalog.
/// </summary>
/// <remarks>
/// Consumers do not need to know which catalog implementation is currently
/// active. The catalog selection is resolved internally based on the
/// server configuration.
/// </remarks>
public class WeaponCatalog
{
    private readonly WeaponCatalogSettings _settings;
    private readonly Dictionary<WeaponCatalogType, WeaponCatalogBase> _catalogs;
    private WeaponCatalogBase Current => _catalogs[_settings.Type];

    public WeaponCatalog(
        WeaponCatalogSettings settings,
        IEnumerable<WeaponCatalogBase> catalogs)
    {
        _settings = settings;       
        _catalogs = catalogs.ToDictionary(catalog => catalog.Type);
    }

    /// <summary>
    /// Gets the number of weapons available in the active catalog.
    /// </summary>
    public int Count => Current.Count;

    /// <summary>
    /// Gets all weapons available in the active catalog.
    /// </summary>
    public IReadOnlyList<IWeapon> GetAll() => Current.GetAll();

    /// <summary>
    /// Determines whether the specified weapon belongs to this active catalog.
    /// </summary>
    public bool Contains(IWeapon weapon) => Current.Contains(weapon);

    /// <summary>
    /// Gets a weapon from the active catalog by its identifier.
    /// </summary>
    public Result<IWeapon> GetById(Weapon id) => Current.GetById(id);

    /// <summary>
    /// Gets a weapon from the active catalog by its display name.
    /// </summary>
    public Result<IWeapon> GetByName(string weaponName) => Current.GetByName(weaponName);

    /// <summary>
    /// Gets a weapon from the active catalog by its index.
    /// </summary>
    public Result<IWeapon> GetByIndex(int index) => Current.GetByIndex(index);
}
