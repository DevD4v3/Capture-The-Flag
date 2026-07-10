namespace CTF.Application.Players.AntiCBug;

/// <summary>
/// Provides administrative commands to enable or disable the GTA: San Andreas
/// crouch bug (C-Bug) protection.
/// </summary>
/// <remarks>
/// C-Bug is a bug in GTA: San Andreas that allows players to manipulate the
/// reload animation of certain weapons, particularly the Desert Eagle, to fire
/// much faster than the game's normal mechanics would allow.
/// </remarks>
public class AntiCBugCommands(
    IWorldService worldService,
    AntiCBugSettings antiCBugSettings) : ISystem
{
    [PlayerCommand("anticbugoff")]
    public void Disable(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.Admin))
            return;

        var message = Smart.Format(Messages.DisableAntiCBug, new
        {
            PlayerName = player.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
        antiCBugSettings.Disabled = true;
    }

    [PlayerCommand("anticbugon")]
    public void Enable(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.Admin))
            return;

        var message = Smart.Format(Messages.EnableAntiCBug, new
        {
            PlayerName = player.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
        antiCBugSettings.Disabled = false;
    }
}
