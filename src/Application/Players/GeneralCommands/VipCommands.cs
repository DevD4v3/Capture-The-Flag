namespace CTF.Application.Players.GeneralCommands;

public class VipCommands : ISystem
{
    [PlayerCommand("cmdsvip")]
    public void ShowVipCommands(Player player, IDialogService dialogService)
    {
        if (player.HasLowerRoleThan(RoleId.VIP))
            return;

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
    public void Saw(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.VIP))
            return;

        player.GiveWeapon(Weapon.Chainsaw, 1);
    }

    [PlayerCommand("spray")]
    public void Spray(Player player) 
    {
        if (player.HasLowerRoleThan(RoleId.VIP))
            return;

        player.GiveWeapon(Weapon.Spraycan, IWeapon.UnlimitedAmmo);
    }

    [PlayerCommand("teargas")]
    public void Teargas(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.VIP))
            return;

        player.GiveWeapon(Weapon.Teargas, IWeapon.UnlimitedAmmo);
    }
}
