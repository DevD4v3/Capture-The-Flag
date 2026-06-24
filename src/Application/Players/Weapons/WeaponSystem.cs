namespace CTF.Application.Players.Weapons;

public class WeaponSystem(
    IEntityManager entityManager,
    IDialogService dialogService,
    WeaponCatalog weaponCatalog,
    WeaponCatalogSettings weaponCatalogSettings) : ISystem
{
    [Event]
    public void OnPlayerConnect(Player player)
    {
        player.AddComponent<WeaponSelectionComponent>();
    }

    [Event]
    public async Task OnPlayerRequestSpawn(Player player)
    {
        await ShowWeapons(player);
        player.SendClientMessage(Color.Orange, Messages.WeaponListUsage);
        player.SendClientMessage(Color.Orange, Messages.WeaponPackUsage);
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

    [Event]
    public void OnPlayerSpawn(Player player)
    {
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

    [PlayerCommand("p")]
    public void GiveParachute(Player player)
    {
        player.GiveWeapon(Weapon.Parachute, 1);
    }

    [PlayerCommand("weapons")]
    public async Task ShowWeapons(Player player)
    {
        var dialog = new ListDialog("Select Weapons", "Select", "Close");
        var weapons = weaponCatalog.GetAll();
        foreach (IWeapon weapon in weapons)
            dialog.Add(weapon.Name);

        ListDialogResponse response = await dialogService.ShowAsync(player, dialog);
        if (response.IsRightButtonOrDisconnected())
            return;

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

    [PlayerCommand("weaponpack")]
    public async Task ShowWeaponPackage(Player player)
    {
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

    [PlayerCommand("weaponcatalog")]
    public async Task ShowCatalogs(Player player)
    {
        if (player.HasLowerRoleThan(RoleId.Admin))
            return;

        var dialog = new ListDialog("Weapon Catalogs", "Select", "Close");
        foreach (WeaponCatalogType type in Enum.GetValues<WeaponCatalogType>())
            dialog.Add(type.GetDisplayName());

        ListDialogResponse response = await dialogService.ShowAsync(player, dialog);
        if (response.IsRightButtonOrDisconnected())
            return;

        WeaponCatalogType selectedCatalog = (WeaponCatalogType)response.ItemIndex;
        if (weaponCatalogSettings.Type == selectedCatalog)
        {
            player.SendClientMessage(Color.Red, Messages.WeaponCatalogAlreadyActive);
            return;
        }

        weaponCatalogSettings.Change(selectedCatalog);
        var catalogName = selectedCatalog.GetDisplayName();
        var message = Smart.Format(Messages.WeaponCatalogChangedTo, new { Name = catalogName });

        foreach (Player currentPlayer in entityManager.GetComponents<Player>())
        {
            var weaponSelection = currentPlayer.GetComponent<WeaponSelectionComponent>();
            WeaponPack selectedWeapons = weaponSelection.SelectedWeapons;

            // Ensure the player's weapon pack only contains weapons
            // available in the newly selected catalog.
            selectedWeapons.RemoveAll(weapon =>
            {
                bool shouldRemove = !weaponCatalog.Contains(weapon);

                if (shouldRemove)
                    currentPlayer.RemoveWeapon(weapon.Id);

                return shouldRemove;
            });

            currentPlayer.SendClientMessage(Color.Yellow, message);
        }
    }
}
