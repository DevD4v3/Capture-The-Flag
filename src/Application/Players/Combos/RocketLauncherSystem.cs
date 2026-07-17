namespace CTF.Application.Players.Combos;

public class RocketLauncherSystem(
    IWorldService worldService,
    ComboSettings comboSettings) : ISystem
{
    [Event]
    public void OnLoadingMap()
    {
        comboSettings.IsRocketLauncherDisabled = true;
    }

    [PlayerCommand("rpgon")]
    [RequiresMinimumRole(RoleId.Moderator)]
    public void EnableRocketLauncher(Player player)
    {
        var message = Smart.Format(Messages.EnableRocketLauncher, new
        {
            PlayerName = player.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
        comboSettings.IsRocketLauncherDisabled = false;
    }

    [PlayerCommand("rpgoff")]
    [RequiresMinimumRole(RoleId.Moderator)]
    public void DisableRocketLauncher(Player player)
    {
        var message = Smart.Format(Messages.DisableRocketLauncher, new
        {
            PlayerName = player.Name
        });
        worldService.SendClientMessage(Color.Yellow, message);
        comboSettings.IsRocketLauncherDisabled = true;
    }
}
