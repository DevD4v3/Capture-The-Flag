namespace CTF.Application.Players.GeneralCommands;

public class VipCommands : ISystem
{
    [PlayerCommand("cmdsvip")]
    [RequiresMinimumRole(RoleId.VIP)]
    public void ShowVipCommands(Player player, IDialogService dialogService)
    {
        var content = Smart.Format(DetailedCommandInfo.VIP, new 
        { 
            Color1 = Color.Yellow,
            Color2 = Color.White
        });

        var dialog = new MessageDialog(
            caption: "VIP Commands", 
            content, 
            button1: "Close"
        );

        dialogService.ShowAsync(player, dialog);
    }

    [PlayerCommand("saw")]
    [RequiresMinimumRole(RoleId.VIP)]
    public void Saw(Player player)
    {
        player.GiveWeapon(Weapon.Chainsaw, 1);
    }

    [PlayerCommand("spray")]
    [RequiresMinimumRole(RoleId.VIP)]
    public void Spray(Player player) 
    {
        player.GiveWeapon(Weapon.Spraycan, IWeapon.UnlimitedAmmo);
    }

    [PlayerCommand("teargas")]
    [RequiresMinimumRole(RoleId.VIP)]
    public void Teargas(Player player)
    {
        player.GiveWeapon(Weapon.Teargas, IWeapon.UnlimitedAmmo);
    }
}
