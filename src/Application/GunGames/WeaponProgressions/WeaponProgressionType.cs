namespace CTF.Application.GunGames.WeaponProgressions;

/// <summary>
/// Identifies the available weapon progression types.
/// </summary>
public enum WeaponProgressionType
{
    [DisplayName("Classic")]
    Classic,

    [DisplayName("Reverse Classic")]
    ReverseClassic,

    [DisplayName("Pistols Only")]
    Pistols,

    [DisplayName("SMGs Only")]
    SMGs,

    [DisplayName("Shotguns Only")]
    Shotguns,

    [DisplayName("Rifles Only")]
    Rifles,

    [DisplayName("Hardcore")]
    Hardcore
}
