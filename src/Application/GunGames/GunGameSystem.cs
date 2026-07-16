namespace CTF.Application.GunGames;

public class GunGameSystem(
    IEntityManager entityManager,
    IWorldService worldService,
    IDialogService dialogService,
    IDictionary<GunGameResult, IGunGameResultHandler> handlers,
    WeaponProgression weaponProgression,
    GunGameSession gunGameSession,
    GunGameReward gunGameReward) : ISystem, IGunGameMode
{
    public bool IsEnabled { get; private set; }

    [Event]
    public void OnPlayerConnect(Player player)
    {
        player.AddComponent<PlayerProgression>();
    }

    [Event]
    public void OnPlayerSpawn(Player player)
    {
        if (!IsEnabled)
            return;

        var playerProgression = player.GetComponent<PlayerProgression>();
        player.GiveWeapon(Weapon.Knife, 1);

        IWeapon weapon = weaponProgression.GetWeapon(playerProgression.WeaponLevel);
        player.GiveWeapon(weapon.Id, IWeapon.UnlimitedAmmo);
    }

    [Event]
    public void OnPlayerDeath(Player victim, Player killer, Weapon reason)
    {
        if (!IsEnabled || killer is null)
            return;

        var killerProgression = killer.GetComponent<PlayerProgression>();
        var victimProgression = victim.GetComponent<PlayerProgression>();
        var gunGame = new GunGame(weaponProgression, gunGameSession.KillsRequiredPerLevel);

        IWeapon expectedWeapon = weaponProgression.GetWeapon(killerProgression.WeaponLevel);

        // open.mp reports explosive and fire-based kills using generic death reasons,
        // without preserving the weapon that caused the kill. Since GunGame restricts
        // players to a single expected weapon, these ambiguous reasons can be mapped
        // back to the expected weapon before being processed by the domain.
        if (reason is Weapon.Explosion or Weapon.FlameThrower)
            reason = expectedWeapon.Id;

        GunGameResult result = gunGame.ProcessKill(killerProgression, victimProgression, reason);
        if (result == GunGameResult.None)
            return;

        IGunGameResultHandler handler = handlers[result];
        handler.Handle(new KillContext(victim, killer, reason));

        if (result == GunGameResult.ScoredFinalKill)
        {
            FinishGunGame();
            gunGameReward.Give(winner: killer);
        }
    }

    [Event]
    public void OnLoadingMap()
    {
        if (!IsEnabled)
            return; 

        var players = entityManager.GetComponents<PlayerProgression>();
        foreach (PlayerProgression player in players)
            player.Reset();
    }

    [PlayerCommand("gungameon")]
    [RequiresRole(RoleId.Moderator)]
    public async Task GunGameOn(Player player, int killsRequiredPerLevel)
    {
        if (IsEnabled)
        {
            player.SendClientMessage(Color.Red, GunGameMessages.GunGameModeAlreadyActive);
            return;
        }

        if (killsRequiredPerLevel < 1)
        {
            player.SendClientMessage(Color.Red, GunGameMessages.InvalidKillsRequiredPerLevel);
            return;
        }

        var dialog = new ListDialog("Select a Weapon Progression", "Select", "Close");
        foreach (WeaponProgressionType type in Enum.GetValues<WeaponProgressionType>())
            dialog.Add(type.GetDisplayName());

        ListDialogResponse response = await dialogService.ShowAsync(player, dialog);
        if (response.IsRightButtonOrDisconnected())
            return;

        var selectedProgression = (WeaponProgressionType)response.ItemIndex;
        gunGameSession.WeaponProgressionType = selectedProgression;
        gunGameSession.KillsRequiredPerLevel = new KillsRequiredPerLevel(killsRequiredPerLevel);

        var message = Smart.Format(GunGameMessages.GunGameConfiguration, new
        {
            Progression = selectedProgression.GetDisplayName(),
            KillsRequiredPerLevel = killsRequiredPerLevel
        });

        player.SendClientMessage(Color.Yellow, message);
        StartGunGame();
    }

    [PlayerCommand("gungameoff")]
    [RequiresRole(RoleId.Moderator)]
    public void GunGameOff(Player player)
    {
        if (!IsEnabled)
        {
            player.SendClientMessage(Color.Red, GunGameMessages.GunGameModeNotActive);
            return;
        }

        FinishGunGame();
        worldService.SendClientMessage(Color.Orange, GunGameMessages.GunGameModeDisabled);
    }

    private void StartGunGame()
    {
        IsEnabled = true;
        var players = entityManager.GetComponents<Player>();
        IWeapon initialWeapon = weaponProgression.GetWeapon(WeaponLevel.First);

        foreach (Player player in players)
        {
            player.GetComponent<PlayerProgression>().Reset();
            player.ResetWeapons();
            player.GiveWeapon(Weapon.Knife, 1);
            player.GiveWeapon(initialWeapon.Id, IWeapon.UnlimitedAmmo);
        }

        worldService.SendClientMessage(Color.Orange, GunGameMessages.GunGameModeStarted);
        worldService.SendClientMessage(Color.Orange, GunGameMessages.GunGameModeObjective);
        worldService.GameText(
            GunGameMessages.GunGameModeStartedGameText,
            TimeSpan.FromSeconds(5),
            GameTextStyle.Style3
        );
    }

    private void FinishGunGame()
    {
        IsEnabled = false;
        var players = entityManager.GetComponents<Player>();

        foreach (Player player in players)
            RestorePlayerWeapons(player);

        worldService.GameText(
            GunGameMessages.GunGameModeFinishedGameText,
            TimeSpan.FromSeconds(5),
            GameTextStyle.Style3
        );
    }

    private static void RestorePlayerWeapons(Player player)
    {
        player.GetComponent<PlayerProgression>().Reset();
        player.ResetWeapons();
        var weaponSelection = player.GetComponent<WeaponSelectionComponent>();
        WeaponPack selectedWeapons = weaponSelection.SelectedWeapons;

        foreach (IWeapon weapon in selectedWeapons)
            player.GiveWeapon(weapon.Id, IWeapon.UnlimitedAmmo);
    }
}
