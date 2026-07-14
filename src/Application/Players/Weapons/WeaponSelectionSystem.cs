namespace CTF.Application.Players.Weapons;

public class WeaponSelectionSystem(
    IDialogService dialogService,
    IGunGameMode gunGameMode,
    WeaponCatalog weaponCatalog) : ISystem
{
    [Event]
    public void OnPlayerConnect(Player player)
    {
        player.AddComponent<WeaponSelectionComponent>();
    }

    [Event]
    public async Task OnPlayerRequestSpawn(Player player)
    {
        if (gunGameMode.IsEnabled)
        {
            player.SendClientMessage(Color.Orange, GunGameMessages.GunGameModeStarted);
            player.SendClientMessage(Color.Orange, GunGameMessages.GunGameModeObjective);
            return;
        }

        await ShowWeapons(player);
        player.SendClientMessage(Color.Orange, Messages.WeaponListUsage);
        player.SendClientMessage(Color.Orange, Messages.WeaponPackUsage);
    }

    [Event]
    public void OnPlayerSpawn(Player player)
    {
        if (gunGameMode.IsEnabled)
            return;

        var weaponSelection = player.GetComponent<WeaponSelectionComponent>();
        WeaponPack selectedWeapons = weaponSelection.SelectedWeapons;
        // Don't user foreach for performance reasons.
        // OnPlayerSpawn is invoked too often.
        for (int i = 0; i < selectedWeapons.TotalItems; i++)
        {
            IWeapon weapon = selectedWeapons[i];
            player.GiveWeapon(weapon.Id, IWeapon.UnlimitedAmmo);
        }
    }

    [Event]
    public async Task OnPlayerKeyStateChange(Player player, Keys newKeys, Keys oldKeys)
    {
        if (KeyUtils.HasPressed(newKeys, oldKeys, Keys.Walk | Keys.CtrlBack))
        {
            GiveParachute(player);
        }
        else if (KeyUtils.HasPressed(newKeys, oldKeys, Keys.Yes))
        {
            await ShowWeapons(player);
        }
        else if (KeyUtils.HasPressed(newKeys, oldKeys, Keys.CtrlBack))
        {
            await ShowWeaponPackage(player);
        }
    }

    [PlayerCommand("p")]
    public void GiveParachute(Player player)
    {
        player.GiveWeapon(Weapon.Parachute, 1);
    }

    [PlayerCommand("weapons")]
    public async Task ShowWeapons(Player player)
    {
        if (gunGameMode.IsEnabled)
        {
            player.SendClientMessage(Color.Red, Messages.WeaponListUnavailable);
            return;
        }

        var dialog = new ListDialog("Select Weapons", "Select", "Close");
        var weapons = weaponCatalog.GetAll();
        foreach (IWeapon weapon in weapons)
            dialog.Add(weapon.Name);

        ListDialogResponse response = await dialogService.ShowAsync(player, dialog);
        if (response.IsRightButtonOrDisconnected())
            return;

        if (gunGameMode.IsEnabled)
        {
            player.SendClientMessage(Color.Red, Messages.WeaponListUnavailable);
            return;
        }

        Result<IWeapon> weaponResult = weaponCatalog.GetByName(response.Item.Text);
        if (weaponResult.IsFailed)
        {
            player.SendClientMessage(Color.Red, Messages.WeaponCatalogChanged);
            return;
        }

        var weaponSelection = player.GetComponent<WeaponSelectionComponent>();
        WeaponPack selectedWeapons = weaponSelection.SelectedWeapons;
        IWeapon weaponSelectedFromDialog = weaponResult.Value;
        if (selectedWeapons.Exists(weaponSelectedFromDialog))
        {
            var message = Smart.Format(Messages.WeaponAlreadyExists, weaponSelectedFromDialog);
            player.SendClientMessage(Color.Red, message);
            await ShowWeapons(player);
            return;
        }

        selectedWeapons.Add(weaponSelectedFromDialog);
        player.GiveWeapon(weaponSelectedFromDialog.Id, IWeapon.UnlimitedAmmo);
        {
            var message = Smart.Format(Messages.WeaponSuccessfullyAdded, weaponSelectedFromDialog);
            player.SendClientMessage(Color.Yellow, message);
        }
        await ShowWeapons(player);
    }

    [PlayerCommand("weaponpack"), Alias("pack")]
    public async Task ShowWeaponPackage(Player player)
    {
        if (gunGameMode.IsEnabled)
        {
            player.SendClientMessage(Color.Red, Messages.WeaponPackUnavailable);
            return;
        }

        var weaponSelection = player.GetComponent<WeaponSelectionComponent>();
        WeaponPack selectedWeapons = weaponSelection.SelectedWeapons;
        if (selectedWeapons.IsEmpty())
        {
            player.SendClientMessage(Color.Red, Messages.EmptyWeaponPackage);
            return;
        }

        var dialog = new ListDialog("Your Weapon Pack", "Remove", "Close");
        foreach (IWeapon weapon in selectedWeapons)
            dialog.Add(weapon.Name);

        ListDialogResponse response = await dialogService.ShowAsync(player, dialog);
        if (response.IsRightButtonOrDisconnected())
            return;

        if (gunGameMode.IsEnabled)
        {
            player.SendClientMessage(Color.Red, Messages.WeaponPackUnavailable);
            return;
        }

        Result<IWeapon> weaponResult = weaponCatalog.GetByName(response.Item.Text);
        if (weaponResult.IsFailed)
        {
            player.SendClientMessage(Color.Red, Messages.WeaponNoLongerAvailable);
            return;
        }

        IWeapon weaponSelectedFromDialog = weaponResult.Value;
        var message = Smart.Format(Messages.WeaponSuccessfullyRemoved, weaponSelectedFromDialog);
        player.SendClientMessage(Color.Red, message);
        selectedWeapons.Remove(weaponSelectedFromDialog);
        player.RemoveWeapon(weaponSelectedFromDialog.Id);
        await ShowWeaponPackage(player);
    }
}
