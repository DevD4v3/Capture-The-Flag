namespace CTF.Application.Players.AntiCBug;

/// <summary>
/// Represents the configuration for the GTA: San Andreas crouch bug (C-Bug) protection.
/// </summary>
/// <remarks>
/// C-Bug is a bug in GTA: San Andreas that allows players to manipulate the
/// reload animation of certain weapons, particularly the Desert Eagle, to fire
/// much faster than the game's normal mechanics would allow.
/// </remarks>
public class AntiCBugSettings
{
    public bool Disabled { get; set; } = false;
}
