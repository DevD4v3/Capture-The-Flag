namespace CTF.Application.Players.Weapons;

public class WeaponCatalogSystem(
    IEntityManager entityManager,
    IDialogService dialogService,
    IGunGameMode gunGameMode,
    WeaponCatalog weaponCatalog,
    WeaponCatalogSettings weaponCatalogSettings) : ISystem
{
    [PlayerCommand("weaponcatalog")]
    [RequiresMinimumRole(RoleId.Admin)]
    public async Task ShowCatalogs(Player player)
    {
        if (gunGameMode.IsEnabled)
        {
            player.SendClientMessage(Color.Red, Messages.WeaponCatalogUnavailable);
            return;
        }

        var dialog = new ListDialog("Weapon Catalogs", "Select", "Close");
        foreach (WeaponCatalogType type in Enum.GetValues<WeaponCatalogType>())
            dialog.Add(type.GetDisplayName());

        ListDialogResponse response = await dialogService.ShowAsync(player, dialog);
        if (response.IsRightButtonOrDisconnected())
            return;

        if (gunGameMode.IsEnabled)
        {
            player.SendClientMessage(Color.Red, Messages.WeaponCatalogUnavailable);
            return;
        }

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
