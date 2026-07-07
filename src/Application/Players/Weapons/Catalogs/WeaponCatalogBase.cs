namespace CTF.Application.Players.Weapons.Catalogs;

/// <summary>
/// Represents a predefined collection of weapons available to players.
/// </summary>
/// <remarks>
/// Derived classes define which weapons belong to a specific catalog.
/// </remarks>
public abstract class WeaponCatalogBase
{
    private readonly List<IWeapon> _weapons = 
    [
        WeaponDefinitions.Knife,
        WeaponDefinitions.Parachute
    ];

    /// <summary>
    /// Initializes the catalog with the weapons that are
    /// always available to players.
    /// </summary>
    protected WeaponCatalogBase()
    {
        Define(_weapons);
    }

    /// <summary>
    /// Gets the catalog type represented by the implementation.
    /// </summary>
    public abstract WeaponCatalogType Type { get; }

    /// <summary>
    /// Populates the catalog with weapon definitions.
    /// </summary>
    protected abstract void Define(List<IWeapon> weapons);

    /// <summary>
    /// Gets the number of weapons in the catalog.
    /// </summary>
    public int Count => _weapons.Count;

    /// <summary>
    /// Gets all weapons defined by the catalog.
    /// </summary>
    public IReadOnlyList<IWeapon> GetAll() => _weapons;

    /// <summary>
    /// Determines whether the specified weapon belongs to this catalog.
    /// </summary>
    public bool Contains(IWeapon weapon) => _weapons.Exists(w => w.Id == weapon.Id);

    /// <summary>
    /// Gets a weapon by its identifier.
    /// </summary>
    public Result<IWeapon> GetById(Weapon id)
    {
        IWeapon weapon = _weapons.FirstOrDefault(w => w.Id == id);
        return weapon is null ? 
            Result<IWeapon>.Failure(Messages.WeaponNotFound) :
            Result<IWeapon>.Success(weapon);
    }

    /// <summary>
    /// Gets a weapon by its display name.
    /// </summary>
    public Result<IWeapon> GetByName(string weaponName)
    {
        ArgumentNullException.ThrowIfNull(weaponName);

        IWeapon weapon = _weapons.FirstOrDefault(
            w => w.Name.Equals(
                weaponName,
                StringComparison.OrdinalIgnoreCase));

        return weapon is null ? 
            Result<IWeapon>.Failure(Messages.WeaponNotFound) :
            Result<IWeapon>.Success(weapon);
    }
}
